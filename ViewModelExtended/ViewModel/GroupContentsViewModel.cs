using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: selecting a group does not refresh the view, and in addition adds a stray new empty GroupNote which should not be possible. may or may not need to call Clear() or RefreshList(). UPDATE: found issue and one possible cause: NextId/PreviousId are overwritten when calling Add on a record existing in db. UPDATE 2: ListViewModel.Add() does not assign Next because it erroneously assumes the next item in the queue will assign it at the same time it is modified. This combined with UpdateNodes immediately after takes that nonexistent Next node and converts it into a null id in the database. Since this is a readonly operation, either make an add function that does not perform Node persistence or remove the node db updating code from ListViewModel and move it to a higher level, so when Add is called, whether or not UpdateNodes is called is optional. ***There are two instances when persistence of the underlying node data upon Add() is required: 1) when a GroupObject is created and added to the GroupContents on DragDrop. 2) When external test data is added.

// TODO: DragLeave on GroupContentsView should remove the dragged item from the list before dropping it (this means every time an item is dragged it will create and destroy a new item - this would be an instance where not saving the object first could be useful)

namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying any notes in a selected group
	/// </summary>
	public class GroupContentsViewModel : ViewModelBase, IObservableList
	{
		private IViewModelResource Resource { get; set; }

		/// <summary>
		/// the base ListViewModel
		/// </summary>
		private ListViewModel ListViewModel { get; set; }

		public IEnumerable<IListItem> Items {
			get { return ListViewModel.Items; }
		}

		/// <summary>
		/// Group that is handed off from NoteTabsViewModel.SelectedGroup - represents an interface to SelectedGroup
		/// </summary>
		private GroupListObjectViewModel? m_ContentData = null;
		public GroupListObjectViewModel? ContentData {
			get { return m_ContentData; }
			set {
				Set(ref m_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Color));
			}
		}

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

		private IListItem? m_Highlighted;
		public IListItem? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
				ListViewModel.Highlighted = m_Highlighted;
			}
		}

		#endregion



		#region Commands

		public ICommand ReorderCommand {
			get { return ListViewModel.ReorderCommand ?? throw new NullReferenceException("command not assigned"); }
			set { if (ListViewModel.ReorderCommand == null) ListViewModel.ReorderCommand = value; }
		}

		public ICommand PreselectCommand {
			get { return ListViewModel.PreselectCommand ?? throw new NullReferenceException("command not assigned"); }
			set { if (ListViewModel.PreselectCommand == null) ListViewModel.PreselectCommand = value; }
		}

		#endregion



		#region Constructor

		public GroupContentsViewModel (IViewModelResource resource)
		{
			Resource = resource;
			ListViewModel = new ListViewModel(Resource);
			Resource.CommandBuilder.MakeGroup(this);
		}

		//public GroupContentsViewModel (IViewModelResource resource, Group groop) : this(resource)
		//{
		//	SetGroup(groop);
		//}

		#endregion



		#region Access

		public void Add (IListItem item)
		{
			ListViewModel.Add(item);
		}

		public void Insert (IListItem? target, IListItem input)
		{
			ListViewModel.Insert(target, input);
		}

		public void Reorder (IListItem source, IListItem target)
		{
			ListViewModel.Reorder(source, target);
		}

		public void Remove (IListItem obj)
		{
			//using (IDbContext dbContext = Resource.CreateDbContext()) {
			//	dbContext.DeleteGroupItem(dbContext.GroupItems.Find(obj.ItemId));
			//}
			ListViewModel.Remove(obj);
			Resource.ViewModelCreator.DestroyGroupObjectViewModel((GroupObjectViewModel)obj);
		}

		public int Index (IListItem input)
		{
			return ListViewModel.Index(input);
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
			ListViewModel.RefreshListView();
		}

		#endregion



		#region Set Group

		public void SetGroup (GroupListObjectViewModel groop)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<IListItem> unsortedObjects =
					Resource.DbQueryHelper.GetGroupObjectsInGroup(dbContext, groop.Data.Data);

				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects, ListViewModel);
			}
		}

		public void Clear ()
		{
			ListViewModel.Clear();
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
