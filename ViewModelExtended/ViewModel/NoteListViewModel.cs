using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: there should be allowed zero notes - zero notes would close or block the NoteTextView

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

		private NoteListObjectViewModel? m_Highlighted;
		public NoteListObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
			}
		}

		#endregion



		#region Dirty List (used by Reorder/ReorderCommit)

		private Dictionary<string, IListItem?> m_OriginalState;
		private Queue<Tuple<NoteListObjectViewModel, NoteListObjectViewModel>> m_DirtyNotes;

		#endregion


		#region Commands

		public ICommand ReorderCommand {
			get { return m_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (m_ReorderCommand == null) m_ReorderCommand = value; }
		}

		private ICommand? m_ReorderCommand;

		public ICommand DropCommand {
			get { return m_DropCommand ?? throw new MissingCommandException(); }
			set { if (m_DropCommand == null) m_DropCommand = value; }
		}

		private ICommand? m_DropCommand;

		public ICommand CancelDropCommand {
			get { return m_CancelDropCommand ?? throw new MissingCommandException(); }
			set { if (m_CancelDropCommand == null) m_CancelDropCommand = value; }
		}

		private ICommand? m_CancelDropCommand;

		public ICommand PickupCommand {
			get { return m_PickupCommand ?? throw new MissingCommandException(); }
			set { if (m_PickupCommand == null) m_PickupCommand = value; }
		}

		private ICommand? m_PickupCommand;

		#endregion



		#region Constructor

		public NoteListViewModel (IViewModelResource resource)
		{
			m_OriginalState = new Dictionary<string, IListItem?>();

			m_DirtyNotes = new Queue<Tuple<NoteListObjectViewModel, NoteListObjectViewModel>>();
			Resource = resource;
			Resource.CommandBuilder.MakeNoteList(this);
			m_Highlighted = null;
			List = Resource.ViewModelCreator.CreateList<NoteListObjectViewModel>();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<NoteListObjectViewModel> unsortedObjects = Resource.DbQueryHelper.GetAllNoteListObjects(dbContext);
				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects.ToList(), List);
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
			if (!m_OriginalState.ContainsKey("sourcePrevious")) m_OriginalState["sourcePrevious"] = source.Previous;
			if (!m_OriginalState.ContainsKey("source")) m_OriginalState["source"] = source;
			if (!m_OriginalState.ContainsKey("sourceNext")) m_OriginalState["sourceNext"] = source.Next;
			if (!m_OriginalState.ContainsKey("targetPrevious")) m_OriginalState["targetPrevious"] = target.Previous;
			if (!m_OriginalState.ContainsKey("target")) m_OriginalState["target"] = target;
			if (!m_OriginalState.ContainsKey("targetNext")) m_OriginalState["targetNext"] = target.Next;

			List.Reorder(source, target);

			m_DirtyNotes.Enqueue(new Tuple<NoteListObjectViewModel, NoteListObjectViewModel>(source, target));
		}

		public void ReorderCommit ()
		{
			if (!m_DirtyNotes.Any()) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				while (m_DirtyNotes.Any()) {
					Tuple<NoteListObjectViewModel, NoteListObjectViewModel> notePair = m_DirtyNotes.Dequeue();
					Resource.DbListHelper.UpdateAfterReorder(dbContext, notePair.Item1, notePair.Item2);
				}
				dbContext.Save();
			}
			m_DirtyNotes.Clear();
			m_OriginalState.Clear();
		}

		public void CancelReorder ()
		{
			if (!m_OriginalState.ContainsKey("source") || !m_OriginalState.ContainsKey("target")) return;

			IListItem? source = m_OriginalState["source"];
			IListItem? target = m_OriginalState["target"];
			if (source == null || target == null) return;
			source.Previous = m_OriginalState["sourcePrevious"];
			source.Next = m_OriginalState["sourceNext"];
			target.Previous = m_OriginalState["targetPrevious"];
			target.Next = m_OriginalState["targetNext"];
			Reorder((NoteListObjectViewModel)source, (NoteListObjectViewModel)target);
			m_DirtyNotes.Clear();
			m_OriginalState.Clear();
		}

		public void Remove (NoteListObjectViewModel input)
		{
			List.Remove(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterRemove(dbContext, input);
				dbContext.Save();
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
