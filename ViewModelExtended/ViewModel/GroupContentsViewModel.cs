using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: DragOver and MouseUp after DragOver should be two separate operations: DragOver should set a real value for Incoming (not an attached proxy for Outgoing called Incoming, as is currently extant). As outgoing is the NoteListObject, incoming should be GroupObject. This way, if MouseUp event is fired before DragLeave, the GroupObject save operation is performed. If DragLeave is triggered, delete the GroupObject.

// TODO: DragLeave on GroupContentsView should remove the dragged item from the list before dropping it (this means every time an item is dragged it will create and destroy a new item - this would be an instance where not saving the object first could be useful)

namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying any notes in a selected group
	/// </summary>
	public class GroupContentsViewModel : ViewModelBase
	{
		private IViewModelResource Resource { get; set; }

		/// <summary>
		/// the base ListViewModel
		/// </summary>
		private IObservableList List { get; set; }

		public IEnumerable<IListItem> Items {
			get { return List.Items; }
		}

		/// <summary>
		/// Group that is handed off from NoteTabsViewModel.SelectedGroup - represents an interface to SelectedGroup
		/// </summary>
		public GroupListObjectViewModel? ContentData {
			get { return m_ContentData; }
			set {
				Set(ref m_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Color));
			}
		}

		private GroupListObjectViewModel? m_ContentData = null;

		public string Title {
			get {
				if (m_ContentData != null) return m_ContentData.Title;
				return "";
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Title, value)) return;
					m_ContentData.Title = value;
					NotifyPropertyChanged(nameof(Title));
				}
			}
		}

		public string Color {
			get {
				if (m_ContentData != null) return m_ContentData.Color;
				return "";
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Color, value)) return;
					m_ContentData.Color = value;
					NotifyPropertyChanged(nameof(Color));
				}
			}
		}

		#region Cross-View Data

		public GroupObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
			}
		}

		private GroupObjectViewModel? m_Highlighted;

		#endregion



		#region Commands

		public ICommand ReorderCommand {
			get { return m_ReorderCommand ?? throw new NullReferenceException("command not assigned"); }
			set { if (m_ReorderCommand == null) m_ReorderCommand = value; }
		}

		private ICommand? m_ReorderCommand;

		public ICommand PreselectCommand {
			get { return m_PreselectCommand ?? throw new NullReferenceException("command not assigned"); }
			set { if (m_PreselectCommand == null) m_PreselectCommand = value; }
		}

		private ICommand? m_PreselectCommand;

		#endregion



		#region Constructor

		public GroupContentsViewModel (IViewModelResource resource)
		{
			Resource = resource;
			List = Resource.ViewModelCreator.CreateList();
			Resource.CommandBuilder.MakeGroup(this);
		}

		#endregion



		#region Access

		public void Add (GroupObjectViewModel input)
		{
			List.Add(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterAdd(dbContext, input);
				dbContext.Save();
			}
		}

		public void Insert (GroupObjectViewModel? target, GroupObjectViewModel input)
		{
			List.Insert(target, input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterInsert(dbContext, target, input);
				dbContext.Save();
			}
		}

		public void Reorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			List.Reorder(source, target);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterReorder(dbContext, source, target);
				dbContext.Save();
			}
		}

		public void Remove (GroupObjectViewModel input)
		{
			List.Remove(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterRemove(dbContext, input);
				dbContext.Save();
			}

			Resource.ViewModelCreator.DestroyGroupObjectViewModel(input);
		}

		public int Index (GroupObjectViewModel input)
		{
			return List.Index(input);
		}

		#endregion



		#region Create

		public GroupObjectViewModel Create (Group groop, Note data)
		{
			return Resource.ViewModelCreator.CreateGroupObjectViewModel(groop, data);
		}

		#endregion



		#region Refresh

		public void Refresh ()
		{
			Utility.RefreshListView(List.Items);
		}

		#endregion



		#region Set Group

		public void SetGroup (GroupListObjectViewModel groop)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<IListItem> unsortedObjects =
					Resource.DbQueryHelper.GetGroupObjectsInGroup(dbContext, groop.Model.Data);

				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects, List);
			}
		}

		public void Clear ()
		{
			List.Clear();
		}

		#endregion



		//#region Add Note to Group

		//public void AddNoteToGroup (IListItem input)
		//{
		//	// if no group is selected, bail out
		//	if (ContentData == null) {
		//		return;
		//	}

		//	// associate a newly created GroupObject with the given NoteListObject
		//	NoteListObjectViewModel originalNote = (NoteListObjectViewModel)input;
		//	GroupObjectViewModel groupNote =
		//		Resource.ViewModelCreator.CreateGroupObjectViewModel(ContentData.Data.Data, originalNote.Data.Data);

		//	// add the GroupObject to the contents list
		//	Add(groupNote);
		//}

		//#endregion
	}
}
