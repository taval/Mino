﻿using System;
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
			get { return f_Contents.Items; }
		}

		///// <summary>
		///// the internal interface to the selected contents list
		///// </summary>
		private GroupContents f_Contents;

		#endregion



		#region ViewModelResource

		/// <summary>
		/// the viewmodel datacontext
		/// </summary>
		private IViewModelResource f_Resource;

		#endregion



		#region Delegate List

		/// <summary>
		/// the list of event handlers and their respective subjects
		/// - int represents the observer's id
		/// - NoteListObjectViewModel is subject
		/// - PropertyChangedHandler is the delegate
		/// </summary>
		private NoteDelegateDictionary f_Delegates;

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
					f_Contents.List = f_Contents.GetListByGroupKey(null);
					f_Changes.Items = f_Changes.GetListByGroupKey(null);
					HasGroup = false;
					return;
				}

				HasGroup = true;

				Group groop = value.Model.Data;

				if (groop != null) {
					// select the dirty list
					f_Changes.Items = f_Changes.GetListByGroupKey(groop);

					// populate the GroupContents list
					f_Contents.List = f_Contents.GetListByGroupKey(groop);

					using (IDbContext dbContext = f_Resource.CreateDbContext()) {
						if (!f_Contents.List.Any()) {
							IList<GroupObjectViewModel> tempList = new List<GroupObjectViewModel>();

							PopulateGroup(dbContext, tempList, groop);

							IEnumerable<GroupObjectViewModel> sortedObjects =
								f_Resource.DbListHelper.SortListObjects(tempList);

							f_Contents.List.Clear();
							f_Contents.List.AddSortedRange(sortedObjects);
						}
					}
				}

				Set(ref f_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Color));
				NotifyPropertyChanged(nameof(Items));
			}
		}

		private GroupListObjectViewModel? f_ContentData;



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
				f_TempGroupObjectViewModel = CreateTemp(ContentData.Model.Data, value.Model.Data);

				// set the incoming note for further reference
				f_Incoming = value;
			}
		}

		private NoteListObjectViewModel? f_Incoming;

		/// <summary>
		/// the NoteListObjectViewModel received from an external list, e.g. from NoteListViewModel
		/// </summary>
		private GroupObjectViewModel? f_TempGroupObjectViewModel;

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
		private GroupChangeQueue f_Changes;

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
			// resources for object component construction
			IComponentCreator componentCreator = new ComponentCreator();
			IGroupContentsComponentCreator groupContentsComponentCreator = new GroupContentsComponentCreator();
			f_Resource = resource;

			// attach commands
			f_Resource.CommandBuilder.MakeGroup(this);

			// attach handlers
			SetPropertyChangedEventHandler(f_Resource.StatusBarViewModel);

			// init contents container
			f_Contents = groupContentsComponentCreator.CreateGroupContents(
				() => componentCreator.CreateObservableList<GroupObjectViewModel>());

			ItemCount = f_Contents.ItemCount;

			// init change 'queue'
			f_Changes = groupContentsComponentCreator.CreateGroupChangeQueue(
				() => componentCreator.CreateListItemDictionary());

			// init content metadata (the 'group')
			f_ContentData = null;

			// init delegate reference
			f_Delegates = groupContentsComponentCreator.CreateNoteDelegateDictionary();

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

			f_Contents.Add(input);
			ItemCount = f_Contents.ItemCount;

			f_Changes.QueueOnAdd(input);
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

			f_Contents.Insert(target, input);
			ItemCount = f_Contents.ItemCount;

			f_Changes.QueueOnInsert(target, input);
		}

		/// <summary>
		/// rearrange two objects in the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			f_Contents.Reorder(source, target);

			f_Changes.QueueOnReorder(source, target);
		}


		/// <summary>
		/// remove the object from the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (GroupObjectViewModel input)
		{
			UnsetNoteObserver(input);

			f_Changes.QueueOnRemove(input);

			f_Contents.Remove(input);
			ItemCount = f_Contents.ItemCount;

			using (IDbContext dbContext = f_Resource.CreateDbContext()) {
				f_Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, input);
			}
		}

		/// <summary>
		/// return the position of the given input
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Index (GroupObjectViewModel input)
		{
			return f_Contents.Index(input);
		}

		#endregion



		#region Query

		public GroupObjectViewModel Find (Func<GroupObjectViewModel, bool> predicate)
		{
			return f_Contents.Find(predicate);
		}

		public NoteListObjectViewModel FindNote (GroupObjectViewModel input)
		{
			return f_Resource.NoteListViewModel.Find((noteListViewModel) => noteListViewModel.DataId == input.DataId);
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
			using (IDbContext dbContext = f_Resource.CreateDbContext()) {
				GroupObjectViewModel output =
					f_Resource.ViewModelCreator.CreateTempGroupObjectViewModel(dbContext, groop, data);

				f_Contents.Add(output);

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
			using (IDbContext dbContext = f_Resource.CreateDbContext()) {
				GroupObjectViewModel output =
					f_Resource.ViewModelCreator.CreateGroupObjectViewModel(dbContext, groop, data);

				Add(output);

				return output;
			}
		}

		public void SetNoteObserver (NoteListObjectViewModel subject, GroupObjectViewModel observer)
		{
			int observerId = observer.ItemId;

			if (f_Delegates.ContainsKey(observerId)) return;

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

			f_Delegates.Add(observerId, f_Delegates.Create(subject, handler));
		}

		public void UnsetNoteObserver (GroupObjectViewModel observer)
		{
			int observerId = observer.ItemId;
			NoteHandler noteHandler = f_Delegates.GetHandlerByObserverId(observerId);
			NoteListObjectViewModel subject = noteHandler.Subject;
			PropertyChangedEventHandler handler = noteHandler.Handler;

			subject.PropertyChanged -= handler;

			f_Delegates.Remove(observerId);
		}

		#endregion



		#region Methods: Clear

		/// <summary>
		/// destroy a Group's contents and the associated list
		/// </summary>
		/// <param name="groop"></param>
		public void DestroyList (Group groop)
		{
			if (!f_Contents.Lists.ContainsKey(groop)) return;

			IObservableList<GroupObjectViewModel> groupObjs = f_Contents.Lists[groop];

			foreach (GroupObjectViewModel obj in groupObjs.Items) Remove(obj);

			f_Contents.Lists.Remove(groop);
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
			foreach (KeyValuePair<Group, IObservableList<GroupObjectViewModel>> list in f_Contents.Lists) {
				if (f_Changes.Dictionaries.ContainsKey(list.Key)) f_Changes.Dictionaries[list.Key].Clear();
				ClearList(list.Value);
			}
			ItemCount = f_Contents.ItemCount;
		}

		#endregion



		#region Methods: GroupContents

		/// <summary>
		/// cancels the incoming GroupObject
		/// </summary>
		public void HoldGroupNote ()
		{
			using (IDbContext dbContext = f_Resource.CreateDbContext()) {
				if (f_TempGroupObjectViewModel != null) {
					f_Contents.Remove(f_TempGroupObjectViewModel);
					f_TempGroupObjectViewModel = null;
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
			if (ContentData == null || Incoming == null || f_TempGroupObjectViewModel == null) return;

			// remove the temp from list
			f_Contents.Remove(f_TempGroupObjectViewModel);

			Group groop = ContentData.Model.Data;
			Note note = f_TempGroupObjectViewModel.Model.Data;

			// associate a newly created GroupObject with the given temporary GroupObject
			GroupObjectViewModel groupNote = Create(groop, note);

			// add the GroupObject to the contents list
			f_TempGroupObjectViewModel = null;
			Incoming = null;
		}

		/// <summary>
		/// set the given list with all sorted GroupObjects (notes) within a Group
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="list"></param>
		/// <param name="groop"></param>
		private void PopulateGroup (IDbContext dbContext, IList<GroupObjectViewModel> list, Group groop)
		{
			/** a GroupObjectViewModel loaded from db is constructed partially from live data.
			 * - iterate through the database items for ObjectId used to identify the associated Note data
			 * - construct the GroupObjectViewModel
			 * - set GroupObjectViewModel's event handler on NoteListObjectViewModel
			 * - add it to list
			 */
			IQueryable<Tuple<GroupItem, ObjectRoot>> unsortedObjects =
				f_Resource.DbQueryHelper.GetGroupItemsInGroup(dbContext, groop);
			IList<Tuple<GroupItem, ObjectRoot>> groupItemsInGroup = unsortedObjects.ToList();

			foreach (Tuple<GroupItem, ObjectRoot> item in groupItemsInGroup) {
				IEnumerable<NoteListObjectViewModel> noteMatch =
					f_Resource.NoteListViewModel.Items.Where((noteVM) => noteVM.DataId == item.Item1.ObjectId);

				if (!noteMatch.Any()) {
					throw new Exception("no NoteListObjectViewModel matching the GroupObjectViewModel could be found");
				}

				NoteListObjectViewModel subject = noteMatch.Single();

				GroupObjectViewModel observer = f_Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item.Item1, item.Item2, groop, subject.Model.Data));

				SetNoteObserver(subject, observer);

				list.Add(observer);
			}
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
			using (IDbContext dbContext = f_Resource.CreateDbContext()) {
				// get the unbound data objects
				IQueryable<GroupObjectViewModel> groupObjectsByNote =
					f_Resource.DbQueryHelper.GetGroupObjectsByNote(dbContext, note);

				// for each group represented in original query (can just iterate over the query since one group exists per group object)
				foreach (GroupObjectViewModel obj in groupObjectsByNote) {
					IObservableList<GroupObjectViewModel>? list = null;

					// select the list of the particular group or the display group
					list = f_Contents.GetListByGroupKey(obj.Model.Group);

					// populate the viewmodel list of that group
					if (!list.Any()) {
						IList<GroupObjectViewModel> tempList = new List<GroupObjectViewModel>();

						PopulateGroup(dbContext, tempList, obj.Model.Group);

						IEnumerable<GroupObjectViewModel> sortedObjects =
							f_Resource.DbListHelper.SortListObjects(tempList);

						list.Clear();
						list.AddSortedRange(sortedObjects);
					}
					
					// select bound group object matching the unbound group object in the original query
					IEnumerable<GroupObjectViewModel> match =
						list.Items.Where((n) => n.DataId == note.Id);

					// remove the bound group object from the output list
					if (match.Any()) {
						GroupObjectViewModel item = match.Single();

						// remove event handlers
						UnsetNoteObserver(item);

						// remove from dirty list - the appropriate group's dictionary is found via item's Group
						f_Changes.QueueOnRemove(item);

						// remove from list
						list.Remove(item);
						if (list == f_Contents.List) ItemCount = f_Contents.ItemCount;

						// destroy the database record
						f_Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, item);
					}
				}
			}
		}

		#endregion



		#region Shutdown

		private void SaveListOrder ()
		{
			if (!f_Changes.IsDirty) return;

			foreach (KeyValuePair<Group, IListItemDictionary> kvChangesInGroup in f_Changes.Dictionaries) {
				Group key = kvChangesInGroup.Key;
				IListItemDictionary changesInGroup = kvChangesInGroup.Value;

				if (!changesInGroup.Any()) continue;

				using (IDbContext dbContext = f_Resource.CreateDbContext()) {
					foreach (KeyValuePair<IListItem, int> obj in changesInGroup) {
						f_Resource.DbListHelper.UpdateNodes(dbContext, obj.Key);
						changesInGroup.Remove(obj.Key);
					}

					dbContext.Save();
				}
				changesInGroup.Clear();
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