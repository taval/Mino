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
		private IObservableList<NoteListObjectViewModel> List { get; set; }

		public IEnumerable<NoteListObjectViewModel> Items {
			get { return List.Items; }
		}

		private IViewModelResource Resource { get; set; }



		#region Cross-View Data

		public NoteListObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set { Set(ref m_Highlighted, value); }
		}

		private NoteListObjectViewModel? m_Highlighted;

		public int RecordCount {
			get { return m_RecordCount; }
			private set { Set(ref m_RecordCount, value); }
		}

		private int m_RecordCount;

		#endregion



		#region Dirty List (used by Reorder/ReorderCommit)

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		private Dictionary<IListItem, int> m_DirtyListItems;

		#endregion


		#region Commands

		public ICommand ReorderCommand {
			get { return m_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (m_ReorderCommand == null) m_ReorderCommand = value; }
		}

		private ICommand? m_ReorderCommand;

		public ICommand PickupCommand {
			get { return m_PickupCommand ?? throw new MissingCommandException(); }
			set { if (m_PickupCommand == null) m_PickupCommand = value; }
		}

		private ICommand? m_PickupCommand;

		#endregion



		#region Constructor

		public NoteListViewModel (IViewModelResource resource)
		{
			m_DirtyListItems = new Dictionary<IListItem, int>();
			Resource = resource;
			Resource.CommandBuilder.MakeNoteList(this);
			m_Highlighted = null;
			List = Resource.ViewModelCreator.CreateList<NoteListObjectViewModel>();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<NoteListObjectViewModel> unsortedObjects =
					Resource.DbQueryHelper.GetAllNoteListObjects(dbContext);

				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects.ToList(), List);
			}

			RecordCount = List.Items.Count();
		}

		#endregion



		#region Access

		public void Add (NoteListObjectViewModel input)
		{
			List.Add(input);
			RecordCount = List.Items.Count();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				//Resource.DbListHelper.UpdateAfterAdd(dbContext, input);
				//dbContext.Save();
				m_DirtyListItems.Add(input, m_DirtyListItems.Count());
			}
		}

		public void Insert (NoteListObjectViewModel? target, NoteListObjectViewModel input)
		{
			List.Insert(target, input);
			RecordCount = List.Items.Count();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				//Resource.DbListHelper.UpdateAfterInsert(dbContext, target, input);
				//dbContext.Save();
				m_DirtyListItems.Add(input, m_DirtyListItems.Count());
				if (target != null) m_DirtyListItems.Add(target, m_DirtyListItems.Count());
			}
		}

		public void Reorder (NoteListObjectViewModel source, NoteListObjectViewModel target)
		{
			List.Reorder(source, target);

			if (!m_DirtyListItems.ContainsKey(source)) m_DirtyListItems.Add(source, m_DirtyListItems.Count());
			if (!m_DirtyListItems.ContainsKey(target)) m_DirtyListItems.Add(target, m_DirtyListItems.Count());
		}

		public void SaveListOrder ()
		{
			if (!m_DirtyListItems.Any()) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				//while (m_DirtyListItems.Any()) {
				//attempt 1
					//Tuple<NoteListObjectViewModel, NoteListObjectViewModel> notePair = m_DirtyListItems.Dequeue();
					//Resource.DbListHelper.UpdateAfterReorder(dbContext, notePair.Item1, notePair.Item2);
					// attempt 2
					//IListItem currentItem = m_DirtyListItems.Dequeue();
					//Resource.DbListHelper.UpdateNodes(dbContext, currentItem);
				//}
				// attempt 3
				IEnumerable<KeyValuePair<IListItem, int>> items =
					from item in m_DirtyListItems
					orderby item.Value ascending
					select item;

				foreach (KeyValuePair<IListItem, int> item in items) {
					Resource.DbListHelper.UpdateNodes(dbContext, item.Key);
					m_DirtyListItems.Remove(item.Key);
				}

				dbContext.Save();
			}
			m_DirtyListItems.Clear();
		}

		public void Remove (NoteListObjectViewModel input)
		{
			List.Remove(input);
			RecordCount = List.Items.Count();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				//Resource.DbListHelper.UpdateAfterRemove(dbContext, input);
				//dbContext.Save();
				if (input.Previous != null && !m_DirtyListItems.ContainsKey(input.Previous))
					m_DirtyListItems.Add(input.Previous, m_DirtyListItems.Count());

				if (input.Next != null && !m_DirtyListItems.ContainsKey(input.Next))
					m_DirtyListItems.Add(input.Next, m_DirtyListItems.Count());

				if (m_DirtyListItems.ContainsKey(input)) m_DirtyListItems.Remove(input);
				
				Resource.ViewModelCreator.DestroyNoteListObjectViewModel(dbContext, input);
			}
		}

		public int Index (NoteListObjectViewModel input)
		{
			return List.Index(input);
		}

		public void Clear ()
		{
			RemoveAllEventHandlers();
			List.Clear();
			RecordCount = List.Items.Count();
		}

		#endregion



		#region Query

		public NoteListObjectViewModel Find (Func<NoteListObjectViewModel, bool> predicate)
		{
			return List.Find(predicate);
		}

		#endregion



		#region Create

		public NoteListObjectViewModel Create ()
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				return Resource.ViewModelCreator.CreateNoteListObjectViewModel(dbContext);
			}
		}

		#endregion
	}
}
