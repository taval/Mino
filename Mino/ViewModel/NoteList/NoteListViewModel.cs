using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Mino.Model;

// TODO: Group should be renamed Tag, as in 'this entry is tagged with x, y, and z.'

namespace Mino.ViewModel
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
			get { return f_List.Items; }
		}

		private IObservableList<NoteListObjectViewModel> f_List;

		#endregion



		#region ViewModelResource

		/// <summary>
		/// facilities for constructing and modifying ViewModels
		/// </summary>
		private IViewModelKit f_ViewModelKit;

		/// <summary>
		/// viewmodel component creation methods
		/// </summary>
		private IComponentCreator f_ComponentCreator;

		#endregion



		#region EventHandler

		IDictionary<string, PropertyChangedEventHandler> f_Handlers;

		#endregion



		#region Cross-View Data

		/// <summary>
		/// visually depicts the most recently clicked item
		/// </summary>
		public NoteListObjectViewModel? Highlighted {
			get { return f_Highlighted; }
			set {
				if (Highlighted != null) Highlighted.PropertyChanged -= f_Handlers["DropReady"];
				Set(ref f_Highlighted, value);
				if (Highlighted != null) Highlighted.PropertyChanged += f_Handlers["DropReady"];
				NotifyPropertyChanged(nameof(IsDropReady));
			}
		}

		private NoteListObjectViewModel? f_Highlighted;

		/// <summary>
		/// the number of items in the container
		/// </summary>
		public int ItemCount {
			get { return f_List.Items.Count(); }
		}

		public string DefaultText {
			get { return f_DefaultText ?? String.Empty; }
			set { Set(ref f_DefaultText, value); }
		}

		private string? f_DefaultText;

		public bool IsDropReady {
			get {
				if (Highlighted == null) return false;
				return
					NoteTitleRule.IsValidNoteTitle(Highlighted.Title) &&
					PriorityRule.IsValidPriority(Highlighted.Priority);
			}
		}
		#endregion



		#region Dirty List

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		private IChangeQueue<NoteListObjectViewModel> f_Changes;

		#endregion



		#region Commands

		/// <summary>
		/// sets the serialized equivalent of the default document
		/// </summary>
		public ICommand SetDefaultTextCommand {
			get { return f_SetDefaultTextCommand ?? throw new MissingCommandException(); }
			set { if (f_SetDefaultTextCommand == null) f_SetDefaultTextCommand = value; }
		}

		private ICommand? f_SetDefaultTextCommand;

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

		public NoteListViewModel (IViewModelKit viewModelKit)
		{
			// set default text value - not guaranteed to be compatible with any user of NoteListViewModel
			DefaultText = String.Empty;

			// set references to creation classes
			f_ViewModelKit = viewModelKit;
			f_ComponentCreator = new ComponentCreator();

			f_Handlers = new Dictionary<string, PropertyChangedEventHandler>();

			// init change 'queue'
			f_Changes = f_ComponentCreator.CreateChangeQueue<NoteListObjectViewModel>();

			// init highlighted
			f_Highlighted = null;

			// init handler used for any object assigned at Highlighted
			AddHighlightedHandler();

			f_List = f_ComponentCreator.CreateObservableList<NoteListObjectViewModel>();

			// notify the viewmodel list count has changed (zero is a size too)
			NotifySizeChanged();
		}

		private void AddHighlightedHandler ()
		{
			f_Handlers["DropReady"] = (sender, e) =>
			{
				if (e.PropertyName.Equals("Title") || e.PropertyName.Equals("Priority")) {
					NotifyPropertyChanged(nameof(IsDropReady));
				}
			};
		}

		#endregion



		#region Load

		/// <summary>
		/// populate list:
		/// - set the viewmodel list to a new IObservableList
		/// - get list objects from database
		/// - sort the instantiated list
		/// - add the data to the viewmodel's list
		/// </summary>

		public void Load ()
		{
			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				IQueryable<NoteListObjectViewModel> unsortedObjects =
					f_ViewModelKit.DbQueryHelper.GetAllNoteListObjects(dbContext);

				IEnumerable<NoteListObjectViewModel> sortedObjects =
					f_ViewModelKit.DbListHelper.SortListObjects(unsortedObjects.ToList());

				f_List.Clear();
				f_List.AddSortedRange(sortedObjects);
			}

			// notify the viewmodel list count has changed
			NotifySizeChanged();

			if (SetDefaultTextCommand.CanExecute(null)) {
				SetDefaultTextCommand.Execute(null);
			}
		}

		#endregion



		#region List Access

		private void NotifySizeChanged ()
		{
			NotifyPropertyChanged(nameof(ItemCount));
		}

		/// <summary>
		/// add item to end of list
		/// </summary>
		/// <param name="input"></param>
		public void Add (NoteListObjectViewModel input)
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
		public void Insert (NoteListObjectViewModel? target, NoteListObjectViewModel input)
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
		public void Reorder (NoteListObjectViewModel source, NoteListObjectViewModel target)
		{
			f_List.Reorder(source, target);

			f_Changes.QueueOnReorder(source, target);
		}

		/// <summary>
		/// remove object from list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (NoteListObjectViewModel input)
		{
			f_Changes.QueueOnRemove(input);

			f_List.Remove(input);

			NotifySizeChanged();

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				f_ViewModelKit.ViewModelCreator.DestroyNoteListObjectViewModel(dbContext, input);
			}
		}

		/// <summary>
		/// return position of given list item
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Index (NoteListObjectViewModel input)
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
		public NoteListObjectViewModel Find (Func<NoteListObjectViewModel, bool> predicate)
		{
			return f_List.Find(predicate);
		}

		#endregion



		#region Create

		/// <summary>
		/// create a new free object
		/// </summary>
		/// <returns></returns>
		public NoteListObjectViewModel Create ()
		{
			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				return f_ViewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(dbContext,
					(vm) => { vm.Text = DefaultText; });
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

		private void RemoveHighlightedHandler ()
		{
			if (Highlighted != null) {
				Highlighted.PropertyChanged -= f_Handlers["DropReady"];
			}

			f_Handlers.Clear();
		}

		/// <summary>
		/// do housekeeping (save changes, clear resources, etc.)
		/// </summary>
		public void Shutdown ()
		{
			SaveListOrder();
			Clear();
			RemoveHighlightedHandler();
			RemoveAllEventHandlers();
		}

		#endregion
	}
}
