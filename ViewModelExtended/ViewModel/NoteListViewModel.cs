using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		#region Container

		/// <summary>
		/// the base ListViewModel
		/// </summary>
		public IEnumerable<NoteListObjectViewModel> Items {
			get { return List.Items; }
		}

		private IObservableList<NoteListObjectViewModel> List { get; set; }

		#endregion



		#region ViewModelResource

		/// <summary>
		/// the viewmodel datacontext
		/// </summary>
		private IViewModelResource Resource { get; set; }

		#endregion



		#region Cross-View Data

		/// <summary>
		/// visually depicts the most recently clicked item
		/// </summary>
		public NoteListObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set { Set(ref m_Highlighted, value); }
		}

		private NoteListObjectViewModel? m_Highlighted;

		/// <summary>
		/// the number of items in the container
		/// </summary>
		public int ItemCount {
			get { return m_ItemCount; }
			private set { Set(ref m_ItemCount, value); }
		}

		private int m_ItemCount;

		#endregion



		#region Dirty List

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		private Dictionary<IListItem, int> m_DirtyListItems;

		#endregion



		#region Commands

		/// <summary>
		/// rearrange two nodes
		/// </summary>
		public ICommand ReorderCommand {
			get { return m_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (m_ReorderCommand == null) m_ReorderCommand = value; }
		}

		private ICommand? m_ReorderCommand;

		/// <summary>
		/// gets data for beginning of drag-drop operation
		/// </summary>
		public ICommand PickupCommand {
			get { return m_PickupCommand ?? throw new MissingCommandException(); }
			set { if (m_PickupCommand == null) m_PickupCommand = value; }
		}

		private ICommand? m_PickupCommand;

		#endregion



		#region Constructor

		public NoteListViewModel (IViewModelResource resource)
		{
			Resource = resource;
			Resource.CommandBuilder.MakeNoteList(this);
			SetPropertyChangedEventHandler(Resource.StatusBarViewModel);
			m_DirtyListItems = new Dictionary<IListItem, int>();
			m_Highlighted = null;
			List = Resource.ViewModelCreator.CreateList<NoteListObjectViewModel>();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<NoteListObjectViewModel> unsortedObjects =
					Resource.DbQueryHelper.GetAllNoteListObjects(dbContext);

				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects.ToList(), List);
			}

			ItemCount = List.Items.Count();
		}

		/// <summary>
		/// create a selection of listeners on this object
		/// </summary>
		/// <param name="observer"></param>
		private void SetPropertyChangedEventHandler (StatusBarViewModel observer)
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "ItemCount") {
					int _ = observer.ItemCount;
				}
			};

			PropertyChanged += handler;
		}

		#endregion



		#region List Access

		/// <summary>
		/// add item to end of list
		/// </summary>
		/// <param name="input"></param>
		public void Add (NoteListObjectViewModel input)
		{
			List.Add(input);
			ItemCount = List.Items.Count();
			m_DirtyListItems.Add(input, m_DirtyListItems.Count());
		}

		/// <summary>
		/// add item to list in the position given by target object
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (NoteListObjectViewModel? target, NoteListObjectViewModel input)
		{
			List.Insert(target, input);
			ItemCount = List.Items.Count();
			m_DirtyListItems.Add(input, m_DirtyListItems.Count());
			if (target != null && !m_DirtyListItems.ContainsKey(target))
				m_DirtyListItems.Add(target, m_DirtyListItems.Count());
		}

		/// <summary>
		/// rearrange two objects in list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (NoteListObjectViewModel source, NoteListObjectViewModel target)
		{
			List.Reorder(source, target);

			if (!m_DirtyListItems.ContainsKey(source)) m_DirtyListItems.Add(source, m_DirtyListItems.Count());
			if (!m_DirtyListItems.ContainsKey(target)) m_DirtyListItems.Add(target, m_DirtyListItems.Count());
		}

		/// <summary>
		/// remove object from list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (NoteListObjectViewModel input)
		{
			List.Remove(input);
			ItemCount = List.Items.Count();

			if (input.Previous != null && !m_DirtyListItems.ContainsKey(input.Previous))
				m_DirtyListItems.Add(input.Previous, m_DirtyListItems.Count());

			if (input.Next != null && !m_DirtyListItems.ContainsKey(input.Next))
				m_DirtyListItems.Add(input.Next, m_DirtyListItems.Count());

			if (m_DirtyListItems.ContainsKey(input)) m_DirtyListItems.Remove(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.ViewModelCreator.DestroyNoteListObjectViewModel(dbContext, input);
			}
		}

		/// <summary>
		/// return position of given list item
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Index (NoteListObjectViewModel input)
		{
			return List.Index(input);
		}

		/// <summary>
		/// empty the list
		/// </summary>
		public void Clear ()
		{
			m_DirtyListItems.Clear();
			List.Clear();
			ItemCount = List.Items.Count();
		}

		#endregion



		#region Query

		/// <summary>
		/// return an item based on conditions provided via callback
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public NoteListObjectViewModel Find (Func<NoteListObjectViewModel, bool> predicate)
		{
			return List.Find(predicate);
		}

		#endregion



		#region Create

		/// <summary>
		/// create a new free object
		/// </summary>
		/// <returns></returns>
		public NoteListObjectViewModel Create ()
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				return Resource.ViewModelCreator.CreateNoteListObjectViewModel(dbContext);
			}
		}

		#endregion



		#region Shutdown

		/// <summary>
		/// persist list node order
		/// </summary>
		private void SaveListOrder ()
		{
			if (!m_DirtyListItems.Any()) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				foreach (KeyValuePair<IListItem, int> obj in Resource.DbQueryHelper.SortDictionary(m_DirtyListItems)) {
					Resource.DbListHelper.UpdateNodes(dbContext, obj.Key);
					m_DirtyListItems.Remove(obj.Key);
				}

				dbContext.Save();
			}
			m_DirtyListItems.Clear();
		}

		/// <summary>
		/// do housekeeping (save changes, clear resources, etc.)
		/// </summary>
		public void Shutdown ()
		{
			SaveListOrder();
			Clear();
			RemoveAllEventHandlers();
		}

		#endregion
	}
}
