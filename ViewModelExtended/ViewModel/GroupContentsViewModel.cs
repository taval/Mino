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
	/// ViewModel data for displaying any notes in a selected group
	/// </summary>
	public class GroupContentsViewModel : ViewModelBase
	{
		#region Container

		///// <summary>
		///// the public enumerable interface for a list of GroupObjects - represents the contents of the selected group
		///// </summary>
		public IEnumerable<GroupObjectViewModel> Items {
			get { return Contents.Items; }
		}

		///// <summary>
		///// the internal interface to the selected contents list
		///// </summary>
		private GroupContents Contents { get; set; }

		#endregion



		#region ViewModelResource

		/// <summary>
		/// the viewmodel datacontext
		/// </summary>
		private IViewModelResource p_Resource { get; set; }

		#endregion



		#region Delegate List

		/// <summary>
		/// the list of event handlers and their respective subjects
		/// - int represents the observer's id
		/// - NoteListObjectViewModel is subject
		/// - PropertyChangedHandler is the delegate
		/// </summary>
		private Dictionary<int, KeyValuePair<NoteListObjectViewModel, PropertyChangedEventHandler>>
			p_Delegates { get; set; }

		#endregion



		#region ContentData

		/// <summary>
		/// Group that is handed off from client.SelectedGroup - represents an interface to SelectedGroup metadata
		/// </summary>
		public GroupListObjectViewModel? ContentData {
			get { return f_ContentData; }
			set {
				// if ContentData was set to null, assign the list an empty value
				if (value == null) {
					//Contents.GetListByGroupKey(null);
					Contents.List = Contents.GetListByGroupKey(null);
					//DirtyLists.GetListByGroupKey(null);
					DirtyLists.List = DirtyLists.GetListByGroupKey(null);
					HasGroup = false;
					return;
				}

				HasGroup = true;

				Group groop = value.Model.Data;

				if (groop != null) {
					// select the dirty list
					//DirtyLists.GetListByGroupKey(groop);
					DirtyLists.List = DirtyLists.GetListByGroupKey(groop);

					// populate the GroupContents list
					Contents.List = Contents.GetListByGroupKey(groop);

					using (IDbContext dbContext = p_Resource.CreateDbContext()) {
						//if (!list.Items.Any()) PopulateGroup(dbContext, list, groop);
						if (!Contents.List.Items.Any()) {
							IList<GroupObjectViewModel> tempList = new List<GroupObjectViewModel>();

							PopulateGroup(dbContext, tempList, groop);

							p_Resource.DbQueryHelper.GetSortedListObjects(tempList, Contents.List);
						}
					}
				}

				Set(ref f_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Color));
				NotifyPropertyChanged(nameof(Items));
			}
		}

		private GroupListObjectViewModel? f_ContentData = null;



		/// <summary>
		/// if any groups exist, return true, otherwise return false
		/// </summary>
		public bool HasGroup {
			get { return f_HasGroup; }
			private set { Set(ref f_HasGroup, value); }
		}

		private bool f_HasGroup;

		/// <summary>
		/// the group's title
		/// </summary>
		public string Title {
			get {
				if (f_ContentData != null) return f_ContentData.Title;
				return string.Empty;
			}
			set {
				if (f_ContentData != null) {
					if (Equals(f_ContentData.Title, value)) return;
					f_ContentData.Title = value;
					NotifyPropertyChanged(nameof(Title));
				}
			}
		}

		/// <summary>
		/// the group's associated color
		/// </summary>
		public string Color {
			get {
				if (f_ContentData != null) return f_ContentData.Color;
				return string.Empty;
			}
			set {
				if (f_ContentData != null) {
					if (Equals(f_ContentData.Color, value)) return;
					f_ContentData.Color = value;
					NotifyPropertyChanged(nameof(Color));
				}
			}
		}

		#endregion



		#region Cross-View Data

		/// <summary>
		/// visually depicts the most recently clicked item
		/// </summary>
		public GroupObjectViewModel? Highlighted {
			get { return f_Highlighted; }
			set {
				Set(ref f_Highlighted, value);
			}
		}

		private GroupObjectViewModel? f_Highlighted;

		/// <summary>
		/// the drag-drop data object to be the basis for the GroupObjectViewModel
		/// </summary>
		public NoteListObjectViewModel? Incoming {
			private get { return f_Incoming; }
			set {
				if (value == null) {
					f_Incoming = null;
					return;
				}

				// if no group is selected, bail out
				if (ContentData == null) return;

				// if Note already exists in Group, bail out
				if (Items.Contains(value, new ListDataEqualityComparer())) return;

				// create a temporary GroupObject with the given NoteListObject
				p_TempGroupObjectViewModel = CreateTemp(ContentData.Model.Data, value.Model.Data);

				// set the incoming note for further reference
				f_Incoming = value;
			}
		}

		private NoteListObjectViewModel? f_Incoming;

		/// <summary>
		/// the NoteListObjectViewModel received from an external list, e.g. from NoteListViewModel
		/// (set by ReceiveGroupNote())
		/// </summary>
		private GroupObjectViewModel? p_TempGroupObjectViewModel { get; set; }

		/// <summary>
		/// the number of items in the container
		/// </summary>
		public int ItemCount {
			get { return f_ItemCount; }
			private set { Set(ref f_ItemCount, value); }
		}

		private int f_ItemCount;

		#endregion



		#region Dirty Lists

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		GroupChangeQueue DirtyLists;

		#endregion



		#region Commands

		/// <summary>
		/// swap one list item's order with another
		/// </summary>
		public ICommand ReorderCommand {
			get { return f_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (f_ReorderCommand == null) f_ReorderCommand = value; }
		}

		private ICommand? f_ReorderCommand;

		/// <summary>
		/// set the data to be dropped in a DragDrop operation
		/// </summary>
		public ICommand PickupCommand {
			get { return f_PickupCommand ?? throw new MissingCommandException(); }
			set { if (f_PickupCommand == null) f_PickupCommand = value; }
		}

		private ICommand? f_PickupCommand;

		/// <summary>
		/// sends a Note to a Group
		/// </summary>
		public ICommand NoteReceiveCommand {
			get { return f_NoteReceiveCommand ?? throw new MissingCommandException(); }
			set { if (f_NoteReceiveCommand == null) f_NoteReceiveCommand = value; }
		}

		private ICommand? f_NoteReceiveCommand;

		#endregion



		#region Constructor

		public GroupContentsViewModel (IViewModelResource resource)
		{
			// attach commands
			p_Resource = resource;
			p_Resource.CommandBuilder.MakeGroup(this);

			// attach handlers
			SetPropertyChangedEventHandler(p_Resource.StatusBarViewModel);

			// init contents container
			Contents = new GroupContents(() => p_Resource.ViewModelCreator.CreateList<GroupObjectViewModel>());
			ItemCount = Contents.ItemCount;

			// init change 'queue'
			DirtyLists = new GroupChangeQueue(() => new Dictionary<IListItem, int>());

			// init content metadata (the 'group')
			f_ContentData = null;

			// init delegate reference
			p_Delegates = new Dictionary<int, KeyValuePair<NoteListObjectViewModel, PropertyChangedEventHandler>>();

			// init highlighted
			f_Highlighted = null;
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
		/// add an object to the end of the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="input"></param>
		public void Add (GroupObjectViewModel input)
		{
			NoteListObjectViewModel match = FindNote(input);

			SetNoteObserver(match, input);

			Contents.Add(input);
			ItemCount = Contents.ItemCount;

			DirtyLists.QueueOnAdd(input);
		}

		/// <summary>
		/// insert an object into the CURRENTLY VISIBLE list at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (GroupObjectViewModel? target, GroupObjectViewModel input)
		{
			NoteListObjectViewModel match = FindNote(input);

			SetNoteObserver(match, input);

			Contents.Insert(target, input);
			ItemCount = Contents.ItemCount;

			DirtyLists.QueueOnInsert(target, input);
		}

		/// <summary>
		/// rearrange two objects in the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			Contents.Reorder(source, target);

			DirtyLists.QueueOnReorder(source, target);
		}


		/// <summary>
		/// remove the object from the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (GroupObjectViewModel input)
		{
			UnsetNoteObserver(input);

			DirtyLists.QueueOnRemove(input);

			Contents.Remove(input);
			ItemCount = Contents.ItemCount;

			using (IDbContext dbContext = p_Resource.CreateDbContext()) {
				p_Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, input);
			}
		}

		/// <summary>
		/// return the position of the given input
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Index (GroupObjectViewModel input)
		{
			return Contents.Index(input);
		}

		#endregion



		#region Query

		public GroupObjectViewModel Find (Func<GroupObjectViewModel, bool> predicate)
		{
			return Contents.List.Find(predicate);
		}

		public NoteListObjectViewModel FindNote (GroupObjectViewModel input)
		{
			return p_Resource.NoteListViewModel.Find((noteListViewModel) => noteListViewModel.DataId == input.DataId);
		}

		#endregion



		#region Methods: Create

		/// <summary>
		/// create a temporary GroupObject with the given NoteListObject
		/// </summary>
		/// <param name="groop"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public GroupObjectViewModel CreateTemp (Group groop, Note data)
		{
			using (IDbContext dbContext = p_Resource.CreateDbContext()) {
				GroupObjectViewModel output =
					p_Resource.ViewModelCreator.CreateTempGroupObjectViewModel(dbContext, groop, data);

				Contents.Add(output);

				return output;
			}
		}

		/// <summary>
		/// create a persistent GroupObject with the given NoteListObject
		/// </summary>
		/// <param name="groop"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public GroupObjectViewModel Create (Group groop, Note data)
		{
			using (IDbContext dbContext = p_Resource.CreateDbContext()) {
				GroupObjectViewModel output =
					p_Resource.ViewModelCreator.CreateGroupObjectViewModel(dbContext, groop, data);

				Add(output);

				return output;
			}
		}

		public void SetNoteObserver (NoteListObjectViewModel subject, GroupObjectViewModel observer)
		{
			int observerId = observer.ItemId;

			if (p_Delegates.ContainsKey(observerId)) return;

			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "Title") {
					string _ = observer.Title;
				}
				else if (e.PropertyName == "Text") {
					string _ = observer.Text;
				}
			};

			subject.PropertyChanged += handler;

			p_Delegates.Add(
				observerId, new KeyValuePair<NoteListObjectViewModel, PropertyChangedEventHandler>(subject, handler));
		}

		public void UnsetNoteObserver (GroupObjectViewModel observer)
		{
			int observerId = observer.ItemId;
			NoteListObjectViewModel subject = p_Delegates[observerId].Key;
			PropertyChangedEventHandler handler = p_Delegates[observerId].Value;

			subject.PropertyChanged -= handler;

			p_Delegates.Remove(observerId);
		}

		#endregion





		#region Methods: Clear

		/// <summary>
		/// destroy a Group's contents and the associated list
		/// </summary>
		/// <param name="groop"></param>
		public void DestroyList (Group groop)
		{
			if (!Contents.Lists.ContainsKey(groop)) return;

			IObservableList<GroupObjectViewModel> groupObjs = Contents.Lists[groop];

			foreach (GroupObjectViewModel obj in groupObjs.Items) Remove(obj);

			Contents.Lists.Remove(groop);
		}

		/// <summary>
		/// non-destructively clear a list
		/// </summary>
		private void ClearList (IObservableList<GroupObjectViewModel> list)
		{
			foreach (GroupObjectViewModel obj in list.Items) {
				UnsetNoteObserver(obj);
			}
			list.Clear();
		}

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ()
		{
			foreach (KeyValuePair<Group, IObservableList<GroupObjectViewModel>> list in Contents.Lists) {
				if (DirtyLists.Lists.ContainsKey(list.Key)) DirtyLists.Lists[list.Key].Clear();
				ClearList(list.Value);
			}
			ItemCount = Contents.ItemCount;
		}

		#endregion



		#region Methods: GroupContents

		/// <summary>
		/// cancels the incoming GroupObject
		/// </summary>
		public void HoldGroupNote ()
		{
			using (IDbContext dbContext = p_Resource.CreateDbContext()) {
				if (p_TempGroupObjectViewModel != null) {
					Contents.Remove(p_TempGroupObjectViewModel);
					p_TempGroupObjectViewModel = null;
					Incoming = null;
				}
			}
		}

		/// <summary>
		/// adds a dependent GroupObject converted from given owner NoteListObject
		/// </summary>
		/// <param name="input"></param>
		public void AddNoteToGroup ()
		{
			if (ContentData == null || Incoming == null || p_TempGroupObjectViewModel == null) return;

			// remove the temp from list
			Contents.Remove(p_TempGroupObjectViewModel);

			Group groop = ContentData.Model.Data;
			Note note = p_TempGroupObjectViewModel.Model.Data;

			// associate a newly created GroupObject with the given temporary GroupObject
			GroupObjectViewModel groupNote = Create(groop, note);

			// add the GroupObject to the contents list
			p_TempGroupObjectViewModel = null;
			Incoming = null;
		}

		/// <summary>
		/// set the given list with all sorted GroupObjects (notes) within a Group
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="list"></param>
		/// <param name="groop"></param>
		//private void PopulateGroup (IDbContext dbContext, IObservableList<GroupObjectViewModel> list, Group groop)
		//{
		//	/** a GroupObjectViewModel loaded from db is constructed partially from live data.
		//	 * - iterate through the database items for ObjectId used to identify the associated Note data
		//	 * - construct the GroupObjectViewModel
		//	 * - set GroupObjectViewModel's event handler on NoteListObjectViewModel
		//	 * - add it to list
		//	 */
		//	IQueryable<Tuple<GroupItem, ObjectRoot>> unsortedObjects =
		//		p_Resource.DbQueryHelper.GetGroupItemsInGroup(dbContext, groop);
		//	IList<Tuple<GroupItem, ObjectRoot>> groupItemsInGroup = unsortedObjects.ToList();
		//	IList<GroupObjectViewModel> tempList = new List<GroupObjectViewModel>();

		//	foreach (Tuple<GroupItem, ObjectRoot> item in groupItemsInGroup) {
		//		IEnumerable<NoteListObjectViewModel> noteMatch =
		//			p_Resource.NoteListViewModel.Items.Where((noteVM) => noteVM.DataId == item.Item1.ObjectId);

		//		if (!noteMatch.Any()) {
		//			throw new Exception("no NoteListObjectViewModel matching the GroupObjectViewModel could be found");
		//		}

		//		NoteListObjectViewModel subject = noteMatch.Single();

		//		GroupObjectViewModel observer = p_Resource.ViewModelCreator.CreateGroupObjectViewModel(
		//			dbContext.CreateGroupObject(item.Item1, item.Item2,groop, subject.Model.Data));

		//		SetNoteObserver(subject, observer);

		//		tempList.Add(observer);
		//	}

		//	p_Resource.DbQueryHelper.GetSortedListObjects(tempList, list);
		//}
		private void PopulateGroup (IDbContext dbContext, IList<GroupObjectViewModel> list, Group groop)
		{
			/** a GroupObjectViewModel loaded from db is constructed partially from live data.
			 * - iterate through the database items for ObjectId used to identify the associated Note data
			 * - construct the GroupObjectViewModel
			 * - set GroupObjectViewModel's event handler on NoteListObjectViewModel
			 * - add it to list
			 */
			IQueryable<Tuple<GroupItem, ObjectRoot>> unsortedObjects =
				p_Resource.DbQueryHelper.GetGroupItemsInGroup(dbContext, groop);
			IList<Tuple<GroupItem, ObjectRoot>> groupItemsInGroup = unsortedObjects.ToList();
			//IList<GroupObjectViewModel> tempList = new List<GroupObjectViewModel>();

			foreach (Tuple<GroupItem, ObjectRoot> item in groupItemsInGroup) {
				IEnumerable<NoteListObjectViewModel> noteMatch =
					p_Resource.NoteListViewModel.Items.Where((noteVM) => noteVM.DataId == item.Item1.ObjectId);

				if (!noteMatch.Any()) {
					throw new Exception("no NoteListObjectViewModel matching the GroupObjectViewModel could be found");
				}

				NoteListObjectViewModel subject = noteMatch.Single();

				GroupObjectViewModel observer = p_Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item.Item1, item.Item2, groop, subject.Model.Data));

				SetNoteObserver(subject, observer);

				list.Add(observer);
			}

			//p_Resource.DbQueryHelper.GetSortedListObjects(list, list);
		}

		#endregion



		#region Methods: Lists



		/// <summary>
		/// removes all dependent GroupObjects (notes) associated with a given NoteListObject
		/// </summary>
		/// <param name="note"></param>
		/// <exception cref="NullReferenceException"></exception>
		public void RemoveGroupObjectsByNote (Note note)
		{
			using (IDbContext dbContext = p_Resource.CreateDbContext()) {
				// get the unbound data objects
				IQueryable<GroupObjectViewModel> groupObjectsByNote =
					p_Resource.DbQueryHelper.GetGroupObjectsByNote(dbContext, note);

				// for each group represented in original query (can just iterate over the query since one group exists per group object)
				foreach (GroupObjectViewModel obj in groupObjectsByNote) {
					IObservableList<GroupObjectViewModel>? list = null;
					Dictionary<IListItem, int>? dirtyList = null;

					// select the list of the particular group or the display group
					// select the dirtyList of the particular group or the display group
					list = Contents.GetListByGroupKey(obj.Model.Group);
					dirtyList = DirtyLists.GetListByGroupKey(obj.Model.Group);

					
					// populate the viewmodel list of that group
					//if (!list.Items.Any()) PopulateGroup(dbContext, list, obj.Model.Group);
					if (!list.Items.Any()) {
						IList<GroupObjectViewModel> tempList = new List<GroupObjectViewModel>();

						PopulateGroup(dbContext, tempList, obj.Model.Group);

						p_Resource.DbQueryHelper.GetSortedListObjects(tempList, Contents.List);
					}
					

					// select bound group object matching the unbound group object in the original query
					IEnumerable<GroupObjectViewModel> match =
						list.Items.Where((n) => n.DataId == note.Id);

					// remove the bound group object from the output list
					if (match.Any()) {
						GroupObjectViewModel item = match.Single();

						// remove event handlers
						UnsetNoteObserver(item);

						// remove from dirty list
						DirtyLists.QueueOnRemove(item);

						// remove from list
						list.Remove(item);
						if (list == Contents.List) ItemCount = Contents.ItemCount;

						// destroy the database record
						p_Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, item);
					}
				}
			}
		}

		#endregion



		#region Shutdown

		private void SaveListOrder ()
		{
			//if (!DirtyLists.IsDirty) return;

			foreach (KeyValuePair<Group, Dictionary<IListItem, int>> dirtyList in DirtyLists.Lists) {
				Group key = dirtyList.Key;
				Dictionary<IListItem, int> list = dirtyList.Value;

				if (!list.Any()) continue;

				using (IDbContext dbContext = p_Resource.CreateDbContext()) {
					foreach (KeyValuePair<IListItem, int> obj in p_Resource.DbQueryHelper.SortDictionary(list)) {
						p_Resource.DbListHelper.UpdateNodes(dbContext, obj.Key);
						list.Remove(obj.Key);
					}

					dbContext.Save();
				}
				list.Clear();
			}
			DirtyLists.Clear();
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

// TODO: a write is occurring on delete that somehow stores node data -- since node data has not been added to the dbcontext, it just saves a bunch of nulls and screws everything up - the DestroyViewModel methods are the culprit

// UPDATE 2: instant db update via propertychanged handler has same problem with skipping updates as calling update on list functions. node queue is still necessary for large lists. try removing the UpdateNodes call from the Destroy method

// TODO/NOTE: group must be in sync with its contents list and any input to its access points must match an object existing within the currently selected group or bad things will happen. This is not a problem as long as valid existing inputs are made or the newly created inputs are destined for the selected group. There are currently no checks as to whether or not the input exists outside the currently selected list and an object nonexistent within the list should throw an exception