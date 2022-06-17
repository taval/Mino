﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mino.Model;



namespace Mino.ViewModel
{
	public class GroupTabsViewModel : ViewModelBase
	{
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
				Set(ref f_SelectedGroupViewModel, value);
				NotifyPropertyChanged("GroupTitle");
				if (f_SelectedGroupViewModel == null) return;
				f_SelectedGroupViewModel.PropertyChanged += (sender, e) =>
				{
					if (e.PropertyName == "Title") {
						NotifyPropertyChanged("GroupTitle");
					}
				};
			}
		}

		/// <summary>
		/// the selected group's title
		/// </summary>
		public string GroupTitle {
			get { return (SelectedGroupViewModel != null) ? SelectedGroupViewModel.Title : string.Empty; }
		}

		private GroupListObjectViewModel? f_SelectedGroupViewModel;

		/// <summary>
		/// the NoteViewModel selected in the Group Contents tab
		/// </summary>
		public GroupObjectViewModel? SelectedGroupNoteViewModel {
			get { return f_SelectedGroupNoteViewModel; }
			set { Set(ref f_SelectedGroupNoteViewModel, value); }
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

		public bool HasGroup {
			get { return GroupListViewModel.HasGroup; }
		}

		/// <summary>
		/// outputs whether or not a group is selected
		/// </summary>
		public bool IsGroupSelected {
			get { return GroupContentsViewModel.IsGroupSelected; }
		}


		#endregion



		#region Kit

		private IViewModelKit f_ViewModelKit;

		#endregion



		#region Context

		private StateViewModel f_StateViewModel;
		public GroupListViewModel GroupListViewModel { get; private set; }
		public GroupContentsViewModel GroupContentsViewModel { get; private set; }

		#endregion



		#region GroupList Commands

		public ICommand SwitchTabsCommand {
			get { return f_SwitchTabsCommand ?? throw new MissingCommandException(); }
			set { if (f_SwitchTabsCommand == null) f_SwitchTabsCommand = value; }
		}

		private ICommand? f_SwitchTabsCommand;

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
			IViewModelKit viewModelKit,
			StateViewModel stateViewModel,
			GroupListViewModel groupListViewModel,
			GroupContentsViewModel groupContentsViewModel)
		{
			// set viewmodel resources
			f_ViewModelKit = viewModelKit;

			// set viewmodel context dependencies
			f_StateViewModel = stateViewModel;
			GroupListViewModel = groupListViewModel;
			GroupContentsViewModel = groupContentsViewModel;

			// default to no selected item

			f_SelectedGroupViewModel = null;
			f_SelectedGroupNoteViewModel = null;
			SelectedTabIndex = 0;

			// attach commands
			//f_ViewModelKit.CommandBuilder.MakeGroupTabs(this);

			// attach event handlers
			SetGroupCountChangedEventHandler();
			SetGroupNoteCountChangedEventHandler();
		}

		private void SetGroupCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "ItemCount") {
					NotifyPropertyChanged(nameof(GroupCount));
				}
				else if (e.PropertyName == "HasGroup") {
					NotifyPropertyChanged(nameof(HasGroup));
				}
			};

			GroupListViewModel.PropertyChanged += handler;
		}

		private void SetGroupNoteCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "ItemCount") {
					NotifyPropertyChanged(nameof(GroupNoteCount));
				}
				else if (e.PropertyName == "IsGroupSelected") {
					NotifyPropertyChanged(nameof(IsGroupSelected));
				}
			};

			GroupContentsViewModel.PropertyChanged += handler;
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
				SelectGroup(highlightedGroup);
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
				SelectGroupNote(highlightedGroupNote);
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
			SelectGroup(output);

			return output;
		}

		/// <summary>
		/// set the Contents viewer to the selected group (this is generally the Highlighted item passed from GroupList to GroupTabs)
		/// </summary>
		/// <param name="group"></param>
		public void SelectGroup (GroupListObjectViewModel? groop)
		{
			SelectedGroupViewModel = groop;
			if (SelectedGroupViewModel != null) {
				SelectedGroupViewModel.IsSelected = true;
				f_StateViewModel.SelectedGroupListItemId = SelectedGroupViewModel.ItemId;
			}
			else {
				f_StateViewModel.SelectedGroupListItemId = null;
			}

			GroupContentsViewModel.ContentData = groop;
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
			//	SelectGroup(newGroup);
			//	f_ViewModelKit.GroupListViewModel.Highlighted = newGroup;
			//}
			if (GroupListViewModel.Items.Count() == 1) {
				SelectGroup(null);
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
						SelectGroup((GroupListObjectViewModel)input.Next);
					}
					else if (input.Previous != null) {
						SelectGroup((GroupListObjectViewModel)input.Previous);
					}
					GroupListViewModel.Highlighted = SelectedGroupViewModel;
				}
				else if (GroupListViewModel.Highlighted != null) {
					SelectGroup(GroupListViewModel.Highlighted);
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
		/// // assign Group's selected note
		/// </summary>
		/// <param name="note"></param>
		public void SelectGroupNote (GroupObjectViewModel note)
		{
			SelectedGroupNoteViewModel = note;

			if (SelectedGroupNoteViewModel != null) {
				SelectedGroupNoteViewModel.IsSelected = true;
				f_StateViewModel.SelectedGroupItemId = SelectedGroupNoteViewModel.ItemId;
			}
			else {
				f_StateViewModel.SelectedGroupItemId = null;
			}
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
						SelectGroupNote((GroupObjectViewModel)input.Next);
					}
					else if (input.Previous != null) {
						SelectGroupNote((GroupObjectViewModel)input.Previous);
					}
					GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
				}
				else if (GroupContentsViewModel.Highlighted != null) {
					SelectGroupNote(GroupContentsViewModel.Highlighted);
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
			GroupContentsViewModel.Shutdown();
			GroupListViewModel.Shutdown();
		}

		#endregion
	}
}