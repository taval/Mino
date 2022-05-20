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
	/// ViewModel data for displaying the master list of all Groups
	/// </summary>
	public class GroupListViewModel : ViewModelBase
	{
		#region Container

		/// <summary>
		/// the base ListViewModel
		/// </summary>
		public IEnumerable<GroupListObjectViewModel> Items {
			get { return List.Items; }
		}

		private IObservableList<GroupListObjectViewModel> List { get; set; }

		#endregion



		#region Resource

		/// <summary>
		/// the viewmodel datacontext
		/// </summary>
		private IViewModelResource Resource { get; set; }

		#endregion



		#region Cross-View Data

		/// <summary>
		/// visually depicts the most recently clicked item
		/// </summary>
		public GroupListObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set { Set(ref m_Highlighted, value); }
		}

		private GroupListObjectViewModel? m_Highlighted;

		/// <summary>
		/// the number of items in the container
		/// </summary>
		public int ItemCount {
			get { return m_ItemCount; }
			private set { Set(ref m_ItemCount, value); } // TODO: display this on StatusBar
		}

		private int m_ItemCount;

		#endregion



		#region Dirty List

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		private IChangeQueue<GroupListObjectViewModel> DirtyList { get; set; }

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

		/// <summary>
		/// change the title
		/// </summary>
		public ICommand ChangeTitleCommand {
			get { return m_ChangeTitleCommand ?? throw new MissingCommandException(); }
			set { if (m_ChangeTitleCommand == null) m_ChangeTitleCommand = value; }
		}

		private ICommand? m_ChangeTitleCommand;

		/// <summary>
		/// change the color
		/// </summary>
		public ICommand ChangeColorCommand {
			get { return m_ChangeColorCommand ?? throw new MissingCommandException(); }
			set { if (m_ChangeColorCommand == null) m_ChangeColorCommand = value; }
		}

		private ICommand? m_ChangeColorCommand;

		/// <summary>
		/// perform operations based on highlighting an item
		/// </summary>
		public ICommand HighlightCommand {
			get { return m_HighlightCommand ?? throw new MissingCommandException(); }
			set { if (m_HighlightCommand == null) m_HighlightCommand = value; }
		}

		private ICommand? m_HighlightCommand;

		#endregion



		#region Constructor

		public GroupListViewModel (IViewModelResource resource)
		{
			// attach commands
			Resource = resource;
			Resource.CommandBuilder.MakeGroupList(this);

			// attach handlers
			SetPropertyChangedEventHandler(Resource.StatusBarViewModel);

			// init change 'queue'
			DirtyList = new ChangeQueue<GroupListObjectViewModel>(new Dictionary<IListItem, int>());
			m_Highlighted = null;

			// populate list
			List = Resource.ViewModelCreator.CreateList<GroupListObjectViewModel>();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<GroupListObjectViewModel> unsortedObjects =
					Resource.DbQueryHelper.GetAllGroupListObjects(dbContext);

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
		public void Add (GroupListObjectViewModel input)
		{
			List.Add(input);
			ItemCount = List.Items.Count();

			DirtyList.QueueOnAdd(input);
		}

		/// <summary>
		/// add item to list in the position given by target object
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (GroupListObjectViewModel? target, GroupListObjectViewModel input)
		{
			List.Insert(target, input);
			ItemCount = List.Items.Count();

			DirtyList.QueueOnInsert(target, input);
		}

		/// <summary>
		/// rearrange two objects in list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (GroupListObjectViewModel source, GroupListObjectViewModel target)
		{
			List.Reorder(source, target);

			DirtyList.QueueOnReorder(source, target);
		}

		/// <summary>
		/// remove object from list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (GroupListObjectViewModel input)
		{
			DirtyList.QueueOnRemove(input);

			List.Remove(input);
			ItemCount = List.Items.Count();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.ViewModelCreator.DestroyGroupListObjectViewModel(dbContext, input);
			}
		}

		/// <summary>
		/// return position of given list item
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Index (GroupListObjectViewModel input)
		{
			return List.Index(input);
		}

		/// <summary>
		/// empty the list
		/// </summary>
		public void Clear ()
		{
			DirtyList.Clear();
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
		public GroupListObjectViewModel Find (Func<GroupListObjectViewModel, bool> predicate)
		{
			return List.Find(predicate);
		}

		#endregion



		#region Create

		/// <summary>
		/// create a new free object
		/// </summary>
		/// <returns></returns>
		public GroupListObjectViewModel Create ()
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				return Resource.ViewModelCreator.CreateGroupListObjectViewModel(dbContext);
			}
		}

		#endregion



		#region Update

		public void UpdateTitle ()
		{
			if (m_Highlighted == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.UpdateGroup(m_Highlighted.Model.Data, m_Highlighted.Title, null);
				dbContext.Save();
			}
		}

		public void UpdateColor ()
		{
			if (m_Highlighted == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.UpdateGroup(m_Highlighted.Model.Data, m_Highlighted.Color, null);
				dbContext.Save();
			}
		}

		#endregion



		#region Shutdown

		/// <summary>
		/// persist list node order
		/// </summary>
		private void SaveListOrder ()
		{
			if (!DirtyList.List.Any()) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				foreach (KeyValuePair<IListItem, int> obj in Resource.DbQueryHelper.SortDictionary(DirtyList.List)) {
					Resource.DbListHelper.UpdateNodes(dbContext, obj.Key);
					DirtyList.List.Remove(obj.Key);
				}

				dbContext.Save();
			}
			DirtyList.Clear();
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

// TODO: zero groups should gray out the Contents tab. An existing group should ungray/activate it.

// TODO: double-clicking the group's title should only select an item upon the first double-click. Subsequent double-clicks should cause it to edit the text.
// UPDATE: implemented single/double click choice behavior - review and determine if this is sufficient

// TODO: each group is assigned a color. each associated NoteListObjectViewModel should receive a color if it is associated with a group. when removed from group, it goes back to colorless.