using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying the master list of all Notes
	/// </summary>
	public class NoteListViewModel : ViewModelBase
	{
		/// <summary>
		/// the base ListViewModel
		/// </summary>
		private IObservableList List { get; set; }

		public IEnumerable<IListItem> Items {
			get { return List.Items; }
		}

		private IViewModelResource Resource { get; set; }



		#region Cross-View Data

		private NoteListObjectViewModel? m_Highlighted;
		public NoteListObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
			}
		}

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

		public NoteListViewModel (IViewModelResource resource)
		{
			Resource = resource;
			List = Resource.ViewModelCreator.CreateList();
			Resource.CommandBuilder.MakeNoteList(this);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<IListItem> unsortedObjects = Resource.DbQueryHelper.GetAllNoteListObjects(dbContext);
				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects, List);
			}
		}

		#endregion



		#region Access

		public void Add (NoteListObjectViewModel input)
		{
			List.Add(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterAdd(dbContext, input);
				dbContext.Save();
			}
		}

		public void Insert (NoteListObjectViewModel? target, NoteListObjectViewModel input)
		{
			List.Insert(target, input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterInsert(dbContext, target, input);
				dbContext.Save();
			}
		}

		public void Reorder (NoteListObjectViewModel source, NoteListObjectViewModel target)
		{
			List.Reorder(source, target);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterReorder(dbContext, source, target);
				dbContext.Save();
			}
		}

		public void Remove (NoteListObjectViewModel input)
		{
			List.Remove(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterRemove(dbContext, input);
				dbContext.Save();
			}

			Resource.ViewModelCreator.DestroyNoteListObjectViewModel(input);
		}

		public int Index (NoteListObjectViewModel input)
		{
			return List.Index(input);
		}

		public void Clear ()
		{
			List.Clear();
		}

		#endregion



		#region Create

		public NoteListObjectViewModel Create ()
		{
			return Resource.ViewModelCreator.CreateNoteListObjectViewModel();
		}

		#endregion



		#region Refresh

		public void Refresh ()
		{
			Utility.RefreshListView(List.Items);
		}

		#endregion
	}
}
