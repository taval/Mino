using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Mino.Model;



namespace Mino.ViewModel
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
			get { return f_List.Items; }
		}

		private IObservableList<GroupListObjectViewModel> f_List;

		#endregion



		#region Kit

		/// <summary>
		/// facilities for constructing and modifying ViewModels
		/// </summary>
		private IViewModelKit f_ViewModelKit;

		/// <summary>
		/// viewmodel component creation methods
		/// </summary>
		private IComponentCreator f_ComponentCreator;

		#endregion



		#region Cross-View Data

		/// <summary>
		/// visually depicts the most recently clicked item
		/// </summary>
		public GroupListObjectViewModel? Highlighted {
			get { return f_Highlighted; }
			set { Set(ref f_Highlighted, value); }
		}

		private GroupListObjectViewModel? f_Highlighted;

		/// <summary>
		/// the number of items in the container
		/// </summary>
		public int ItemCount {
			get { return f_List.Items.Count(); }
		}

		#endregion



		#region Dirty List

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		private IChangeQueue<GroupListObjectViewModel> f_Changes;

		#endregion



		#region Color Generator

		private readonly ColorGenerator f_ColorGenerator;

		#endregion



		#region Commands

		/// <summary>
		/// rearrange two nodes
		/// </summary>
		public ICommand ReorderCommand {
			get { return f_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (f_ReorderCommand == null) f_ReorderCommand = value; }
		}

		private ICommand? f_ReorderCommand;

		/// <summary>
		/// gets data for beginning of drag-drop operation
		/// </summary>
		public ICommand PickupCommand {
			get { return f_PickupCommand ?? throw new MissingCommandException(); }
			set { if (f_PickupCommand == null) f_PickupCommand = value; }
		}

		private ICommand? f_PickupCommand;

		/// <summary>
		/// perform operations based on highlighting an item
		/// </summary>
		public ICommand HighlightCommand {
			get { return f_HighlightCommand ?? throw new MissingCommandException(); }
			set { if (f_HighlightCommand == null) f_HighlightCommand = value; }
		}

		private ICommand? f_HighlightCommand;

		#endregion



		#region Constructor

		public GroupListViewModel (IViewModelKit viewModelKit)
		{
			f_ColorGenerator = new ColorGenerator();

			f_ComponentCreator = new ComponentCreator();

			// set reference to viewmodelkit
			f_ViewModelKit = viewModelKit;

			// init change 'queue'
			f_Changes = f_ComponentCreator.CreateChangeQueue<GroupListObjectViewModel>();
			f_Highlighted = null;

			// init the observables list
			f_List = f_ComponentCreator.CreateObservableList<GroupListObjectViewModel>();

			// notify the viewmodel list count has changed (zero is a size too)
			NotifySizeChanged();
		}

		#endregion



		#region Load

		/// <summary>
		/// post-view-load initialization
		/// </summary>
		public void Load ()
		{
			/** populate list:
			 * - set the viewmodel list to a new IObservableList
			 * - get list objects from database
			 * - sort the instantiated list
			 * - add the data to the viewmodel's list
			 */
			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				IQueryable<GroupListObjectViewModel> unsortedObjects =
					f_ViewModelKit.DbQueryHelper.GetAllGroupListObjects(dbContext);

				IEnumerable<GroupListObjectViewModel> sortedObjects =
					f_ViewModelKit.DbListHelper.SortListObjects(unsortedObjects.ToList());

				f_List.Clear();
				f_List.AddSortedRange(sortedObjects);
			}

			NotifySizeChanged();
		}

		#endregion



		#region List Access

		/// <summary>
		/// notify that the viewmodel list count has changed
		/// </summary>
		private void NotifySizeChanged ()
		{
			NotifyPropertyChanged(nameof(ItemCount));
		}

		/// <summary>
		/// add item to end of list
		/// </summary>
		/// <param name="input"></param>
		public void Add (GroupListObjectViewModel input)
		{
			f_List.Add(input);

			NotifySizeChanged();

			f_Changes.QueueOnAdd(input);
		}

		/// <summary>
		/// add item to list in the position given by target object
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (GroupListObjectViewModel? target, GroupListObjectViewModel input)
		{
			f_List.Insert(target, input);

			NotifySizeChanged();

			f_Changes.QueueOnInsert(target, input);
		}

		/// <summary>
		/// rearrange two objects in list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (GroupListObjectViewModel source, GroupListObjectViewModel target)
		{
			f_List.Reorder(source, target);

			f_Changes.QueueOnReorder(source, target);
		}

		/// <summary>
		/// remove object from list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (GroupListObjectViewModel input)
		{
			f_Changes.QueueOnRemove(input);

			f_List.Remove(input);

			NotifySizeChanged();

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				f_ViewModelKit.ViewModelCreator.DestroyGroupListObjectViewModel(dbContext, input);
			}
		}

		/// <summary>
		/// return position of given list item
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Index (GroupListObjectViewModel input)
		{
			return f_List.Index(input);
		}

		/// <summary>
		/// empty the list
		/// </summary>
		public void Clear ()
		{
			f_Changes.Clear();
			f_List.Clear();

			NotifySizeChanged();
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
			return f_List.Find(predicate);
		}

		#endregion



		#region Create

		/// <summary>
		/// create a new free object
		/// </summary>
		/// <returns></returns>
		public GroupListObjectViewModel Create ()
		{
			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				GroupListObjectViewModel output =
					f_ViewModelKit.ViewModelCreator.CreateGroupListObjectViewModel(dbContext);

				output.Color = f_ColorGenerator.GenerateHtml();

				return output;
			}
		}

		public GroupListObjectViewModel Create (Action<GroupListObjectViewModel> action)
		{
			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				GroupListObjectViewModel output =
					f_ViewModelKit.ViewModelCreator.CreateGroupListObjectViewModel(dbContext, action);

				output.Color = f_ColorGenerator.GenerateHtml();

				return output;
			}
		}

		#endregion



		#region Update

		// while it is known that the highlighted/selected group is to be updated, the caller needs to be aware of it so it can be determined whether or not NoteText should be updated

		/// <summary>
		/// update the group's title in the database
		/// </summary>
		public void UpdateTitle (GroupListObjectViewModel target)
		{
			if (f_Highlighted == null) return;

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				//dbContext.UpdateGroup(f_Highlighted.Model.Data, f_Highlighted.Title, null);
				dbContext.UpdateGroup(target.Model.Data, target.Title, null);
				dbContext.Save();
			}
		}

		/// <summary>
		/// update the group's color in the database
		/// </summary>
		public void UpdateColor (GroupListObjectViewModel target)
		{
			if (f_Highlighted == null) return;

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				//dbContext.UpdateGroup(f_Highlighted.Model.Data, f_Highlighted.Color, null);
				dbContext.UpdateGroup(target.Model.Data, target.Color, null);
				dbContext.Save();
			}
		}

		#endregion



		#region Shutdown

		/// <summary>
		/// persist list node order
		/// </summary>
		public void SaveListOrder ()
		{
			if (!f_Changes.IsDirty) return;

			IEnumerable<KeyValuePair<IListItem, int>> sortedChanges =
				f_ViewModelKit.DbListHelper.SortDictionaryObjects(f_Changes.Items);

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				foreach (KeyValuePair<IListItem, int> obj in sortedChanges) {
					f_ViewModelKit.DbListHelper.UpdateNodes(dbContext, obj.Key);
					f_Changes.Remove(obj.Key);
				}

				dbContext.Save();
			}
			f_Changes.Clear();
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

// TODO: double-clicking the group's title should only select an item upon the first double-click. Subsequent double-clicks should cause it to edit the text.
// UPDATE: implemented single/double click choice behavior - review and determine if this is sufficient


