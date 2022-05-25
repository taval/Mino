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
			get { return f_Highlighted; }
			set { Set(ref f_Highlighted, value); }
		}

		private NoteListObjectViewModel? f_Highlighted;

		/// <summary>
		/// the number of items in the container
		/// </summary>
		public int ItemCount {
			get { return f_ItemCount; }
			private set { Set(ref f_ItemCount, value); }
		}

		private int f_ItemCount;

		#endregion



		#region Dirty List

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		private IChangeQueue<NoteListObjectViewModel> f_Changes;

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

		#endregion



		#region Constructor

		public NoteListViewModel (IViewModelResource resource)
		{
			IComponentCreator componentCreator = new ComponentCreator();

			// attach commands
			Resource = resource;
			Resource.CommandBuilder.MakeNoteList(this);

			// attach handlers
			SetPropertyChangedEventHandler(Resource.StatusBarViewModel);

			// init change 'queue'
			//DirtyList = new ChangeQueue<NoteListObjectViewModel>(new Dictionary<IListItem, int>());
			//Changes = new ChangeQueue<NoteListObjectViewModel>(
			//	new Dictionary<IListItem, int>(new ListItemEqualityComparer()));
			//f_Changes = new ChangeQueue<NoteListObjectViewModel>(resource);
			f_Changes = componentCreator.CreateChangeQueue<NoteListObjectViewModel>();

			// init highlighted
			f_Highlighted = null;

			// populate list
			//List = Resource.ViewModelCreator.CreateList<NoteListObjectViewModel>();
			List = componentCreator.CreateObservableList<NoteListObjectViewModel>();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<NoteListObjectViewModel> unsortedObjects =
					Resource.DbQueryHelper.GetAllNoteListObjects(dbContext);

				//Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects.ToList(), List);
				IEnumerable<NoteListObjectViewModel> sortedObjects =
					Resource.DbListHelper.SortListObjects(unsortedObjects.ToList());

				List.Clear();
				List.AddSortedRange(sortedObjects);
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

			f_Changes.QueueOnAdd(input);
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

			f_Changes.QueueOnInsert(target, input);
		}

		/// <summary>
		/// rearrange two objects in list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (NoteListObjectViewModel source, NoteListObjectViewModel target)
		{
			List.Reorder(source, target);

			f_Changes.QueueOnReorder(source, target);
		}

		/// <summary>
		/// remove object from list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (NoteListObjectViewModel input)
		{
			f_Changes.QueueOnRemove(input);

			List.Remove(input);
			ItemCount = List.Items.Count();

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
			f_Changes.Clear();
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
			if (!f_Changes.IsDirty) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				//foreach (KeyValuePair<IListItem, int> obj in Resource.DbQueryHelper.SortDictionary(Changes.Items)) {
				foreach (KeyValuePair<IListItem, int> obj in f_Changes) {
					Resource.DbListHelper.UpdateNodes(dbContext, obj.Key);
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
