using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Mino.Model;



namespace Mino.ViewModel
{
	public class GroupTabsViewModel : ViewModelBase
	{
		private Dictionary<string, PropertyChangedEventHandler> f_Handlers;

		#region Cross-View Data

		public int SelectedTabIndex {
			get { return f_SelectedTabIndex; }
			set { Set(ref f_SelectedTabIndex, value); }
		}

		private int f_SelectedTabIndex;

		/// <summary>
		/// the GroupViewModel (data displayed by GroupView)
		/// NOTE: this is bound for purpose of GroupView population operation
		/// </summary>
		public GroupListObjectViewModel? SelectedGroupViewModel {
			get { return f_SelectedGroupViewModel; }
			set {
				if (f_SelectedGroupViewModel != null) {
					f_SelectedGroupViewModel.IsSelected = false;
					f_SelectedGroupViewModel.PropertyChanged -= f_Handlers["SelectedGroupViewModel"];
				}

				if (value != null) {
					value.PropertyChanged += f_Handlers["SelectedGroupViewModel"];
					value.IsSelected = true;
					f_StateViewModel.SelectedGroupListItemId = value.ItemId;
				}
				else {
					f_StateViewModel.SelectedGroupListItemId = null;
				}

				GroupContentsViewModel.ContentData = value;

				Set(ref f_SelectedGroupViewModel, value);

				NotifyPropertyChanged(nameof(SelectedGroupTitle));
				NotifyPropertyChanged(nameof(IsGroupSelectable));
				NotifyPropertyChanged(nameof(ContentsTabColor));
			}
		}

		private GroupListObjectViewModel? f_SelectedGroupViewModel;

		/// <summary>
		/// the selected group's title
		/// </summary>
		public string SelectedGroupTitle {
			get { return (f_SelectedGroupViewModel != null) ? f_SelectedGroupViewModel.Title : String.Empty; }
		}

		public bool IsGroupSelectable {
			get {
				return
					(SelectedGroupViewModel != null) &&
					GroupTitleRule.IsValidGroupTitle(SelectedGroupViewModel.Title);
			}
		}

		public string ContentsTabColor {
			get { return (IsGroupSelectable) ? "#eee" : "#fdd"; }
		}

		/// <summary>
		/// the NoteViewModel selected in the Group Contents tab
		/// </summary>
		public GroupObjectViewModel? SelectedGroupNoteViewModel {
			get { return f_SelectedGroupNoteViewModel; }
			set {
				if (SelectedGroupNoteViewModel != null) {
					SelectedGroupNoteViewModel.IsSelected = false;
				}
				if (value != null) {

					value.IsSelected = true;
					f_StateViewModel.SelectedGroupItemId = value.ItemId;
				}
				else {
					f_StateViewModel.SelectedGroupItemId = null;
				}
				Set(ref f_SelectedGroupNoteViewModel, value);
			}
		}

		private GroupObjectViewModel? f_SelectedGroupNoteViewModel;


		/// <summary>
		/// the number of items in the GroupList container
		/// </summary>
		public int GroupCount {
			get { return GroupListViewModel.ItemCount; }
		}

		/// <summary>
		/// the number of items in the container
		/// </summary>
		public int GroupNoteCount {
			get { return GroupContentsViewModel.ItemCount; }
		}

		#endregion



		#region Context

		private StateViewModel f_StateViewModel;
		public GroupListViewModel GroupListViewModel { get; private set; }
		public GroupContentsViewModel GroupContentsViewModel { get; private set; }

		#endregion



		#region GroupList Commands

		public ICommand GroupSelectCommand {
			get { return f_GroupSelectCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupSelectCommand == null) f_GroupSelectCommand = value; }
		}

		private ICommand? f_GroupSelectCommand;

		public ICommand GroupCreateAtCommand {
			get { return f_GroupCreateAtCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupCreateAtCommand == null) f_GroupCreateAtCommand = value; }
		}

		private ICommand? f_GroupCreateAtCommand;

		/// <summary>
		/// destroy a group
		/// </summary>
		public ICommand GroupDestroyCommand {
			get { return f_GroupDestroyCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupDestroyCommand == null) f_GroupDestroyCommand = value; }
		}

		private ICommand? f_GroupDestroyCommand;

		#endregion



		#region Group Commands

		public ICommand GroupNoteSelectCommand {
			get { return f_GroupNoteSelectCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupNoteSelectCommand == null) f_GroupNoteSelectCommand = value; }
		}

		private ICommand? f_GroupNoteSelectCommand;

		public ICommand GroupNoteDestroyCommand {
			get { return f_GroupNoteDestroyCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupNoteDestroyCommand == null) f_GroupNoteDestroyCommand = value; }
		}

		private ICommand? f_GroupNoteDestroyCommand;

		#endregion



		#region Constructor

		public GroupTabsViewModel (
			StateViewModel stateViewModel,
			GroupListViewModel groupListViewModel,
			GroupContentsViewModel groupContentsViewModel)
		{
			// set viewmodel context dependencies
			f_StateViewModel = stateViewModel;
			GroupListViewModel = groupListViewModel;
			GroupContentsViewModel = groupContentsViewModel;

			// default to no selected item

			f_SelectedGroupViewModel = null;
			f_SelectedGroupNoteViewModel = null;

			SelectedTabIndex = 0;

			f_Handlers = new Dictionary<string, PropertyChangedEventHandler>();

			// attach event handlers
			AddSelectedChangedHandler();
			SetGroupCountChangedEventHandler();
			SetGroupNoteCountChangedEventHandler();
		}

		/// <summary>
		/// create and store the handler for the selected group
		/// </summary>
		private void AddSelectedChangedHandler ()
		{
			f_Handlers["SelectedGroupViewModel"] = (sender, e) =>
			{
				if (e.PropertyName == "Title") {
					NotifyPropertyChanged(nameof(SelectedGroupTitle));
					NotifyPropertyChanged(nameof(IsGroupSelectable));
					NotifyPropertyChanged(nameof(ContentsTabColor));
				}
			};
		}

		private void UnsetSelectedChangedHandler ()
		{
			if (f_SelectedGroupViewModel != null) {
				f_SelectedGroupViewModel.PropertyChanged -= f_Handlers["SelectedGroupViewModel"];
			}

			f_Handlers.Remove("SelectedGroupViewModel");
		}

		/// <summary>
		/// create and assign the stored count handler for GroupList
		/// </summary>
		private void SetGroupCountChangedEventHandler ()
		{
			f_Handlers["GroupCount"] = (sender, e) =>
			{
				if (e.PropertyName == "ItemCount") {
					NotifyPropertyChanged(nameof(GroupCount));
				}
			};

			GroupListViewModel.PropertyChanged += f_Handlers["GroupCount"];
		}

		private void UnsetGroupCountChangedEventHandler ()
		{
			GroupListViewModel.PropertyChanged -= f_Handlers["GroupCount"];
			f_Handlers.Remove("GroupCount");
		}

		/// <summary>
		/// create and assign the stored count handler for GroupContents
		/// </summary>
		private void SetGroupNoteCountChangedEventHandler ()
		{
			f_Handlers["GroupNoteCount"] = (sender, e) =>
			{
				if (e.PropertyName == "ItemCount") {
					NotifyPropertyChanged(nameof(GroupNoteCount));
				}
			};

			GroupContentsViewModel.PropertyChanged += f_Handlers["GroupNoteCount"];
		}

		private void UnsetGroupNoteCountChangedEventHandler ()
		{
			GroupContentsViewModel.PropertyChanged -= f_Handlers["GroupNoteCount"];
			f_Handlers.Remove("GroupNoteCount");
		}

		#endregion



		#region Load

		/// <summary>
		/// post-view-load behavior and assignments for the viewmodel
		/// </summary>
		public void Load ()
		{
			// NOTE: NOT creating a new group if none exist - must be done manually by design
			if (GroupListViewModel.Items.Count() == 0) return;

			// select the most recently selected or first group
			IEnumerable<GroupListObjectViewModel> groupMatch =
				GroupListViewModel.Items.Where((item) => f_StateViewModel.SelectedGroupListItemId == item.ItemId);

			if (groupMatch.Any()) {
				GroupListViewModel.Highlighted = groupMatch.First();
			}
			else {
				GroupListViewModel.Highlighted = GroupListViewModel.Items.First();
			}

			GroupListObjectViewModel? highlightedGroup = GroupListViewModel.Highlighted;

			if (highlightedGroup != null) {
				SelectedGroupViewModel = highlightedGroup;
			}

			// if no notes exist in the group, do nothing
			if (GroupContentsViewModel.Items.Count() == 0) return;

			// select the first note in the selected group
			IEnumerable<GroupObjectViewModel> groupNoteMatch =
				GroupContentsViewModel.Items.Where((item) => f_StateViewModel.SelectedGroupItemId == item.ItemId);

			if (groupNoteMatch.Any()) {
				GroupContentsViewModel.Highlighted = groupNoteMatch.First();
			}
			else {
				GroupContentsViewModel.Highlighted = GroupContentsViewModel.Items.First();
			}

			GroupObjectViewModel highlightedGroupNote = GroupContentsViewModel.Highlighted;

			if (highlightedGroupNote != null) {
				SelectedGroupNoteViewModel = highlightedGroupNote;
			}
		}

		#endregion



		#region Events: GroupList

		/// <summary>
		/// adds a Group at the end of the group list
		/// </summary>
		/// <param name="input"></param>
		public void AddGroup (GroupListObjectViewModel input)
		{
			GroupListViewModel.Add(input);
		}

		/// <summary>
		/// inserts a new group into the group list; selects the newly created group
		/// </summary>
		/// <param name="target">the location at where the item will be inserted</param>
		public GroupListObjectViewModel CreateGroupAt (GroupListObjectViewModel? target)
		{
			GroupListObjectViewModel output = GroupListViewModel.Create();

			// if target is null, try to use the selected item
			if (target == null) {
				target = SelectedGroupViewModel;
			}

			// de-select the target
			if (target != null) {
				target.IsSelected = false;
			}

			GroupListViewModel.Insert(target, output);

			// set selected group to display
			SelectedGroupViewModel = output;

			return output;
		}

		/// <summary>
		/// update the group's title in the database
		/// </summary>
		public void UpdateGroupTitle (GroupListObjectViewModel target)
		{
			GroupListViewModel.UpdateTitle(target);
		}

		/// <summary>
		/// update the group's color in the database
		/// </summary>
		public void UpdateGroupColor (GroupListObjectViewModel target)
		{
			GroupListViewModel.UpdateColor(target);
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyGroup (GroupListObjectViewModel input)
		{
			// select a backup list in case selected 'group' (GroupListObject) is destroyed
			AutoSelectGroupFailSafe(input);

			// add a list item if none remain
			//if (f_ViewModelKit.GroupListViewModel.Items.Count() == 1) {
			//	GroupListObjectViewModel newGroup = f_ViewModelKit.GroupListViewModel.Create();
			//	CreateGroup(null, newGroup);
			//	SelectedGroupViewModel = newGroup;
			//	f_ViewModelKit.GroupListViewModel.Highlighted = newGroup;
			//}
			if (GroupListViewModel.Items.Count() == 1) {
				SelectedGroupViewModel = null;
				GroupListViewModel.Highlighted = null;
			}

			// destroy the GroupObjects (dependent Notes) in the group
			GroupContentsViewModel.DestroyList(input.Model.Data);

			// destroy the Group
			GroupListViewModel.Remove(input);
		}

		private void AutoSelectGroupFailSafe (GroupListObjectViewModel input)
		{
			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (GroupListViewModel.Highlighted == null) {
				GroupListViewModel.Highlighted = SelectedGroupViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedGroupViewModel == input) {
				if (GroupListViewModel.Highlighted == input) {
					if (input.Next != null) {
						SelectedGroupViewModel = (GroupListObjectViewModel)input.Next;
					}
					else if (input.Previous != null) {
						SelectedGroupViewModel = (GroupListObjectViewModel)input.Previous;
					}
					GroupListViewModel.Highlighted = SelectedGroupViewModel;
				}
				else if (GroupListViewModel.Highlighted != null) {
					SelectedGroupViewModel = GroupListViewModel.Highlighted;
				}
			}
			else {
				if (GroupListViewModel.Highlighted == input) {
					GroupListViewModel.Highlighted = SelectedGroupViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}
		}

		#endregion



		#region Events: Group

		public void HoldGroupNote ()
		{
			GroupContentsViewModel.HoldGroupNote();
		}

		/// <summary>
		/// adds a dependent note to a group as a GroupObject
		/// </summary>
		public void AddNoteToGroup ()
		{
			GroupContentsViewModel.AddNoteToGroup();
		}

		/// <summary>
		/// removes all dependent GroupObjects (notes) associated with a given NoteListObject
		/// </summary>
		/// <param name="note"></param>
		public void RemoveGroupObjectsByNote (Note note)
		{
			GroupObjectViewModel? match = null;
			IEnumerable<GroupObjectViewModel> visibleGroupObj = GroupContentsViewModel.Items.Where(
				(item) => item.DataId == note.Id);

			if (visibleGroupObj.Any()) match = visibleGroupObj?.Single();

			if (match != null) AutoSelectGroupNoteFailSafe(match);

			GroupContentsViewModel.RemoveGroupObjectsByNote(note);
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyGroupNote (GroupObjectViewModel input)
		{
			if (SelectedGroupViewModel == null) {
				throw new NullReferenceException("no Group is selected from which to remove a Note");
			}

			// select a backup list in case selected 'groupNote' (GroupObjectViewModel) is destroyed
			AutoSelectGroupNoteFailSafe(input);

			GroupContentsViewModel.Remove(input);
		}

		/// <summary>
		/// selects another object linked to the input, for if/when the input becomes unavailable
		/// </summary>
		/// <param name="input"></param>
		private void AutoSelectGroupNoteFailSafe (GroupObjectViewModel input)
		{
			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (GroupContentsViewModel.Highlighted == null) {
				GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedGroupNoteViewModel == input) {
				if (GroupContentsViewModel.Highlighted == input) {
					if (input.Next != null) {
						SelectedGroupNoteViewModel = (GroupObjectViewModel)input.Next;
					}
					else if (input.Previous != null) {
						SelectedGroupNoteViewModel = (GroupObjectViewModel)input.Previous;
					}
					GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
				}
				else if (GroupContentsViewModel.Highlighted != null) {
					SelectedGroupNoteViewModel = GroupContentsViewModel.Highlighted;
				}
			}
			else {
				if (GroupContentsViewModel.Highlighted == input) {
					GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}
		}

		#endregion



		#region Shutdown

		public void Shutdown ()
		{
			// unset event handlers
			UnsetSelectedChangedHandler();
			UnsetGroupCountChangedEventHandler();
			UnsetGroupNoteCountChangedEventHandler();

			// run shutdown
			GroupContentsViewModel.Shutdown();
			GroupListViewModel.Shutdown();
		}

		#endregion
	}
}
