using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: if selecting a different group then going back to the first one selected, the groupnotes no longer trigger their callbacks

namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying any notes in a selected group
	/// </summary>
	public class GroupContentsViewModel : ViewModelBase
	{
		#region ViewModelResource

		/// <summary>
		/// ViewModelResource
		/// </summary>
		private IViewModelResource Resource { get; set; }

		#endregion



		#region Lists

		/// <summary>
		/// A dictionary of GroupContents lists
		/// </summary>
		private Dictionary<Group, IObservableList<GroupObjectViewModel>> Lists { get; set; }

		private Dictionary<int, KeyValuePair<NoteListObjectViewModel, PropertyChangedEventHandler>>
			Delegates { get; set; }

		#endregion



		#region ContentData

		/// <summary>
		/// Group that is handed off from NoteTabsViewModel.SelectedGroup - represents an interface to SelectedGroup
		/// </summary>
		public GroupListObjectViewModel? ContentData {
			get { return m_ContentData; }
			set {
				// if ContentData was set to null, assign the list an empty value
				if (value == null) {
					m_List = null;
					HasGroup = false;
					return;
				}

				HasGroup = true;

				// populate the GroupContents list
				IObservableList<GroupObjectViewModel>? list = null;
				Group groop = value.Model.Data;

				using (IDbContext dbContext = Resource.CreateDbContext()) {
					if (groop != null) {
						list = GetListByGroupKey(groop);
						PopulateGroup(dbContext, list, groop);
					}

					m_List = list;
				}

				Set(ref m_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Color));
				NotifyPropertyChanged(nameof(Items));
			}
		}

		private GroupListObjectViewModel? m_ContentData = null;

		/// <summary>
		/// the base ListViewModel
		/// </summary>
		private IObservableList<GroupObjectViewModel> List {
			get {
				if (m_List == null) return Resource.ViewModelCreator.CreateList<GroupObjectViewModel>();
				//if (m_List == null) throw new NullReferenceException("GroupObject list not set");

				return m_List;
			}
		}

		private IObservableList<GroupObjectViewModel>? m_List;

		/// <summary>
		/// the public enumerable interface for a list of GroupObjects
		/// </summary>
		public IEnumerable<GroupObjectViewModel> Items {
			get { return List.Items; }
		}

		/// <summary>
		/// if any groups exist, return true, otherwise return false
		/// </summary>
		public bool HasGroup {
			get { return m_HasGroup; }
			private set { Set(ref m_HasGroup, value); }
		}

		private bool m_HasGroup;

		/// <summary>
		/// the group's title
		/// </summary>
		public string Title {
			get {
				if (m_ContentData != null) return m_ContentData.Title;
				return string.Empty;
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Title, value)) return;
					m_ContentData.Title = value;
					NotifyPropertyChanged(nameof(Title));
				}
			}
		}

		/// <summary>
		/// the group's associated color
		/// </summary>
		public string Color {
			get {
				if (m_ContentData != null) return m_ContentData.Color;
				return string.Empty;
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Color, value)) return;
					m_ContentData.Color = value;
					NotifyPropertyChanged(nameof(Color));
				}
			}
		}

		#endregion



		#region Cross-View Data

		/// <summary>
		/// the currently highlighted note
		/// </summary>
		public GroupObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
			}
		}

		private GroupObjectViewModel? m_Highlighted;

		private NoteListObjectViewModel? m_Incoming;

		public NoteListObjectViewModel? Incoming {
			private get { return m_Incoming; }
			set {
				if (value == null) {
					m_Incoming = null;
					return;
				}

				// if no group is selected, bail out
				if (ContentData == null) return;

				// if Note already exists in Group, bail out
				if (Items.Contains(value, new ListDataEqualityComparer())) return;

				// create a temporary GroupObject with the given NoteListObject
				TempGroupObjectViewModel = CreateTemp(ContentData.Model.Data, value.Model.Data);

				// set the incoming note for further reference
				m_Incoming = value;
			}
		}

		/// <summary>
		/// the NoteListObjectViewModel received from an external list, e.g. from NoteListViewModel
		/// (set by ReceiveGroupNote())
		/// </summary>
		private GroupObjectViewModel? TempGroupObjectViewModel { get; set; }

		#endregion



		#region Commands

		/// <summary>
		/// swap one list item's order with another
		/// </summary>
		public ICommand ReorderCommand {
			get { return m_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (m_ReorderCommand == null) m_ReorderCommand = value; }
		}

		private ICommand? m_ReorderCommand;

		/// <summary>
		/// set the data to be dropped in a DragDrop operation
		/// </summary>
		public ICommand PickupCommand {
			get { return m_PickupCommand ?? throw new MissingCommandException(); }
			set { if (m_PickupCommand == null) m_PickupCommand = value; }
		}

		private ICommand? m_PickupCommand;

		/// <summary>
		/// sends a Note to a Group
		/// </summary>
		public ICommand NoteReceiveCommand {
			get { return m_NoteReceiveCommand ?? throw new MissingCommandException(); }
			set { if (m_NoteReceiveCommand == null) m_NoteReceiveCommand = value; }
		}

		private ICommand? m_NoteReceiveCommand;

		#endregion



		#region Constructor

		public GroupContentsViewModel (IViewModelResource resource)
		{
			Resource = resource;
			Resource.CommandBuilder.MakeGroup(this);
			m_ContentData = null;
			m_List = null;
			m_Highlighted = null;
			Lists = new Dictionary<Group, IObservableList<GroupObjectViewModel>>();
			Delegates = new Dictionary<
				int, KeyValuePair<NoteListObjectViewModel, PropertyChangedEventHandler>>();
		}

		#endregion



		#region Query

		public GroupObjectViewModel Find (Func<GroupObjectViewModel, bool> predicate)
		{
			return List.Find(predicate);
		}

		public NoteListObjectViewModel FindNote (GroupObjectViewModel input)
		{
			return Resource.NoteListViewModel.Find((noteListViewModel) => noteListViewModel.DataId == input.DataId);
		}

		#endregion



		#region Methods: Access

		public void Add (GroupObjectViewModel input)
		{
			if (m_List == null) return;

			NoteListObjectViewModel match = FindNote(input);

			SetPropertyChangedEventHandler(match, input);

			List.Add(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterAdd(dbContext, input);
				dbContext.Save();
			}
		}

		public void Insert (GroupObjectViewModel? target, GroupObjectViewModel input)
		{
			if (m_List == null) return;

			NoteListObjectViewModel match = FindNote(input);

			SetPropertyChangedEventHandler(match, input);

			List.Insert(target, input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterInsert(dbContext, target, input);
				dbContext.Save();
			}
		}

		public void Reorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			if (m_List == null) return;

			List.Reorder(source, target);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterReorder(dbContext, source, target);
				dbContext.Save();
			}
		}

		public void Remove (GroupObjectViewModel input)
		{
			if (m_List == null) return;

			UnsetPropertyChangedEventHandler(input);
			List.Remove(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterRemove(dbContext, input);
				dbContext.Save();
				Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, input);
			}
		}

		public int Index (GroupObjectViewModel input)
		{
			if (m_List == null) return -1;

			return List.Index(input);
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
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				GroupObjectViewModel output =
					Resource.ViewModelCreator.CreateTempGroupObjectViewModel(dbContext, groop, data);

				List.Add(output);

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
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				GroupObjectViewModel output =
					Resource.ViewModelCreator.CreateGroupObjectViewModel(dbContext, groop, data);

				Add(output);

				return output;
			}
		}

		public void SetPropertyChangedEventHandler (NoteListObjectViewModel subject, GroupObjectViewModel observer)
		{
			int observerId = observer.ItemId;

			if (Delegates.ContainsKey(observerId)) return;

			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "Title") {
					string _ = observer.Title;
				}
				else if (e.PropertyName == "Text") {
					string _ = observer.Text;
				}
				//MessageBox.Show(
				//	$"GroupObject: { observer.ItemId }\n" +
				//	$"\tTitle: { observer.Title }\n" +
				//	$"\tText: { observer.Text }\n" +
				//	$"Note: { subject.ItemId }\n" +
				//	$"\tTitle: { subject.Title }\n" +
				//	$"\tText: { subject.Text }");
			};

			subject.PropertyChanged += handler;

			Delegates.Add(
				observerId, new KeyValuePair<NoteListObjectViewModel, PropertyChangedEventHandler>(subject, handler));

			//MessageBox.Show("delegate was added to NoteListViewModel");
		}

		public void UnsetPropertyChangedEventHandler (GroupObjectViewModel observer)
		{
			int observerId = observer.ItemId;
			NoteListObjectViewModel subject = Delegates[observerId].Key;
			PropertyChangedEventHandler handler = Delegates[observerId].Value;

			subject.PropertyChanged -= handler;

			Delegates.Remove(observerId);
		}

		#endregion





		#region Methods: Clear

		/// <summary>
		/// destroy the selected Group's contents and the associated list
		/// </summary>
		public void ClearList ()
		{
			if (m_List == null) return;

			Group key = Lists.Where((list) => list.Value == m_List).Single().Key;

			foreach (GroupObjectViewModel obj in m_List.Items) Remove(obj);

			Lists.Remove(key);
			//List.Clear();
		}

		/// <summary>
		/// destroy a Group's contents and the associated list
		/// </summary>
		/// <param name="groop"></param>
		public void ClearList (Group groop)
		{
			if (!Lists.ContainsKey(groop)) return;

			IObservableList<GroupObjectViewModel> groupObjs = Lists[groop];

			foreach (GroupObjectViewModel obj in groupObjs.Items) Remove(obj);

			Lists.Remove(groop);
		}

		/// <summary>
		/// clears all lists
		/// </summary>
		public void Clear ()
		{
			foreach (KeyValuePair<Group, IObservableList<GroupObjectViewModel>> groupObjs in Lists) {
				foreach (GroupObjectViewModel obj in groupObjs.Value.Items) Remove(obj);

				Lists.Remove(groupObjs.Key);
			}

			RemoveAllEventHandlers();
		}

		#endregion



		#region Methods: GroupContents

		///// <summary>
		///// accepts an object from NoteList and converts it into a temporary GroupListObject
		///// </summary>
		///// <param name="input"></param>
		//public void ReceiveGroupNote (NoteListObjectViewModel input)
		//{
		//	using (IDbContext dbContext = Resource.CreateDbContext()) {
		//		// if no group is selected, bail out
		//		if (ContentData == null) return;

		//		// if Note already exists in Group, bail out
		//		if (Items.Contains(input, new GroupNoteObjectEqualityComparer())) return;

		//		// create a temporary GroupObject with the given NoteListObject
		//		GroupObjectViewModel groupNote =
		//			Resource.ViewModelCreator.CreateTempGroupObjectViewModel(
		//				dbContext, ContentData.Model.Data, input.Model.Data);

		//		TempGroupObjectViewModel = groupNote;

		//		List.Add(groupNote);
		//	}
		//}

		/// <summary>
		/// cancels the incoming GroupObject
		/// </summary>
		public void HoldGroupNote ()
		{
			if (m_List == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				if (TempGroupObjectViewModel != null) {
					List.Remove(TempGroupObjectViewModel);
					TempGroupObjectViewModel = null;
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
			if (ContentData == null || Incoming == null || TempGroupObjectViewModel == null) return;

			// remove the temp from list
			List.Remove(TempGroupObjectViewModel);

			Group groop = ContentData.Model.Data;
			Note note = TempGroupObjectViewModel.Model.Data;

			// associate a newly created GroupObject with the given temporary GroupObject
			GroupObjectViewModel groupNote = Create(groop, note);

			// add the GroupObject to the contents list
			TempGroupObjectViewModel = null;
			Incoming = null;
		}

		/// <summary>
		/// set the given list with all sorted GroupObjects (notes) within a Group
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="list"></param>
		/// <param name="groop"></param>
		private void PopulateGroup (IDbContext dbContext, IObservableList<GroupObjectViewModel> list, Group groop)
		{
			//IQueryable<GroupObjectViewModel> unsortedObjects = Resource.DbQueryHelper.GetGroupObjectsInGroup(dbContext, groop);
			//IList<GroupObjectViewModel> tempList = unsortedObjects.ToList();

			//foreach (GroupObjectViewModel observer in tempList) {
			//	NoteListObjectViewModel subject = FindNote(observer);

			//	SetPropertyChangedEventHandler(subject, observer);
			//}

			//Resource.DbQueryHelper.GetSortedListObjects(tempList, list);

			/** a GroupObjectViewModel loaded from db is constructed partially from live data.
			 * - iterate through the database items for ObjectId used to identify the associated Note data
			 * - construct the GroupObjectViewModel
			 * - set GroupObjectViewModel's event handler on NoteListObjectViewModel
			 * - add it to list
			 */
			IQueryable<Tuple<GroupItem, ObjectRoot>> unsortedObjects =
				Resource.DbQueryHelper.GetGroupItemsInGroup(dbContext, groop);
			IList<Tuple<GroupItem, ObjectRoot>> groupItemsInGroup = unsortedObjects.ToList();
			IList<GroupObjectViewModel> tempList = new List<GroupObjectViewModel>();

			foreach (Tuple<GroupItem, ObjectRoot> item in groupItemsInGroup) {
				IEnumerable<NoteListObjectViewModel> noteMatch =
					Resource.NoteListViewModel.Items.Where((noteVM) => noteVM.DataId == item.Item1.ObjectId);

				if (!noteMatch.Any()) {
					throw new Exception("no NoteListObjectViewModel matching the GroupObjectViewModel could be found");
				}

				NoteListObjectViewModel subject = noteMatch.Single();

				GroupObjectViewModel observer = Resource.ViewModelCreator.CreateGroupObjectViewModel(
					dbContext.CreateGroupObject(item.Item1, item.Item2,groop, subject.Model.Data));

				SetPropertyChangedEventHandler(subject, observer);

				tempList.Add(observer);
			}

			Resource.DbQueryHelper.GetSortedListObjects(tempList, list);

		}

		#endregion



		#region Methods: Lists

		/// <summary>
		/// gets the list by using Group as key
		/// </summary>
		/// <param name="groop"></param>
		/// <returns>the GroupObject list associated with the given Group key</returns>
		private IObservableList<GroupObjectViewModel> GetListByGroupKey (Group groop)
		{
			IEnumerable<KeyValuePair<Group, IObservableList<GroupObjectViewModel>>> selectedList =
				Lists.Where((kv) => kv.Key == groop);

			if (selectedList.Any()) {
				return selectedList.Single().Value;
			}
			else {
				IObservableList<GroupObjectViewModel> list =
					Resource.ViewModelCreator.CreateList<GroupObjectViewModel>();

				Lists.Add(groop, list);
				return list;
			}
		}

		/// <summary>
		/// removes all dependent GroupObjects (notes) associated with a given NoteListObject
		/// </summary>
		/// <param name="note"></param>
		/// <exception cref="NullReferenceException"></exception>
		public void RemoveGroupObjectsByNote (Note note)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// get the unbound data objects
				IQueryable<GroupObjectViewModel> groupObjectsByNote =
					Resource.DbQueryHelper.GetGroupObjectsByNote(dbContext, note);

				// for each group represented in original query (can just iterate over the query since one group exists per group object)
				foreach (GroupObjectViewModel obj in groupObjectsByNote) {
					IObservableList<GroupObjectViewModel>? list = null;

					// check if temp list is same as displayed list to prevent populating same list twice
					// select the list of the particular group or the display group
					bool isActiveGroup = obj.Model.Group.Id == ContentData?.Model.Data.Id;

					if (isActiveGroup) {
						list = List;
					}
					else {
						list = GetListByGroupKey(obj.Model.Group);
					}

					if (list == null) {
						throw new NullReferenceException("temporary Group contents list could not be set");
					}

					if (!isActiveGroup) {
						// populate the viewmodel list of that group
						PopulateGroup(dbContext, list, obj.Model.Group);
					}

					// select bound group object matching the unbound group object in the original query
					IEnumerable<GroupObjectViewModel> match =
						list.Items.Where((n) => n.DataId == note.Id);

					// remove the bound group object from the output list
					if (match.Any()) {
						GroupObjectViewModel item = match.Single();

						list.Remove(item);
						Resource.DbListHelper.UpdateAfterRemove(dbContext, item);
						dbContext.Save();
						Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, item);
					}
				}

			}
		}

		#endregion
	}
}
