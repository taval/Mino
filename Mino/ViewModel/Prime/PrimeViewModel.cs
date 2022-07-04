using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Mino.Model;

// TODO: factor out 'controllers' from each section of PrimeViewModel (NoteList, GroupTabs) and sub-GroupTabs (GroupList, GroupContents)
//       (GroupTabs is not an arbitrary distinction but its own target. It should be equivalent in the call hierarchy to a GroupList or GroupContents though how it is organized file-wise may be different)
// UPDATE: the controller pattern may look something like the controller being the place where commands are set, and the viewmodels just expose the actions that the controllers call. if taken in absolute terms then CommandBuilder is essentially ControllerBuilder

// UPDATE 2: the mid-tier 'viewmodels' like NoteListViewModel and GroupListViewModel and GroupContentsViewModel would be considered services which handle the business logic, while Prime and GroupTabs would be closer to representing the ViewModel counterparts to the view. In this vein, components of those mid-tier objects not directly related to the view should be moved up and out of those classes and into objects closer to 'ViewModel'. Also consider that Prime spans multiple services but do not encompass the full functionality of the mid-tier objects, and while the mid-tier handles stuff closer to their specific view, they should also be broken down, so it might go something like PrimeViewModel -> NoteSectionViewModel -> NoteListViewModel -> NoteListService -> (data layer). PrimeViewModel is more or less just a facade for the app-level viewmodels at that point. NoteSectionViewModel would represent the note-adjacent (NoteList, NoteText) parts of the original Prime, NoteListViewModel would represent the view-facing elements of original NoteListViewModel, and NoteListService would represent the business logic. Likewise: PrimeViewModel -> GroupSectionViewModel -> GroupTabsVM/GroupListVM->GroupListService/GroupContentsVM -> GroupContentsService

// UPDATE 3: split vm factory functions into individual classes mirroring their products. split up the 'builder' command attachment class along the same lines. Actual builder pattern would be like builder.SetTitle("sometitle").SetPriority(1).Build() but unclear if this is really necessary, the separation/merging into individual factory classes is the key point here. These can be queued up in App whether using the existing methodology or implementing .NET DI features.

// TODO: incomplete/invalid Notes should be disallowed from saving in db

// TODO: incomplete/invalid Groups should be disallowed from saving in db

namespace Mino.ViewModel
{
	public class PrimeViewModel : ViewModelBase
	{
		#region Cross-View Data

		/// <summary>
		/// the selected NoteListObjectViewModel (data displayed by NoteTextView)
		/// NOTE: this is bound for purpose of NoteText population operation
		/// </summary>
		public NoteListObjectViewModel? SelectedNoteViewModel {
			get { return f_SelectedNoteViewModel; }
			set {
				if (SelectedNoteViewModel != null) {
					SelectedNoteViewModel.IsSelected = false;
				}
				if (value != null) {
					value.IsSelected = true;
					StateViewModel.SelectedNoteListItemId = value.ItemId;
					StatusBarViewModel.SelectedItemId = value.ItemId;
					StatusBarViewModel.SelectedDateCreated = value.DateCreated;
				}
				else {
					StatusBarViewModel.SelectedItemId = null;
					StatusBarViewModel.SelectedDateCreated = null;
				}
				NoteTextViewModel.ContentData = value;

				Set(ref f_SelectedNoteViewModel, value);
				NotifyPropertyChanged(nameof(SelectedNoteTitle));
				NotifyPropertyChanged(nameof(HasSelected));
			}
		}

		private NoteListObjectViewModel? f_SelectedNoteViewModel;

		public string SelectedNoteTitle {
			get {
				if (f_SelectedNoteViewModel != null) {
					return f_SelectedNoteViewModel.Title;
				}
				return String.Empty;
			}
		}

		public bool HasSelected {
			get { return (SelectedNoteViewModel != null); }
		}

		#endregion



		#region Commands: Note

		/// <summary>
		/// selects a Note in the NoteList
		/// </summary>
		public ICommand NoteSelectCommand {
			get { return f_NoteSelectCommand ?? throw new MissingCommandException(); }
			set { if (f_NoteSelectCommand == null) f_NoteSelectCommand = value; }
		}

		private ICommand? f_NoteSelectCommand;

		/// <summary>
		/// inserts an empty note into NoteList
		/// </summary>
		public ICommand NoteCreateAtCommand {
			get { return f_NoteCreateAtCommand ?? throw new MissingCommandException(); }
			set { if (f_NoteCreateAtCommand == null) f_NoteCreateAtCommand = value; }
		}

		private ICommand? f_NoteCreateAtCommand;

		/// <summary>
		/// removes a note from the NoteList
		/// </summary>
		public ICommand NoteDestroyCommand {
			get { return f_NoteDestroyCommand ?? throw new MissingCommandException(); }
			set { if (f_NoteDestroyCommand == null) f_NoteDestroyCommand = value; }
		}

		private ICommand? f_NoteDestroyCommand;

		/// <summary>
		/// adds or removes groups attached to a particular note
		/// </summary>
		public ICommand NoteChangeGroupsCommand {
			get { return f_NoteChangeGroupsCommand ?? throw new MissingCommandException(); }
			set { if (f_NoteChangeGroupsCommand == null) f_NoteChangeGroupsCommand = value; }
		}

		private ICommand? f_NoteChangeGroupsCommand;

		#endregion



		#region Commands: Group

		/// <summary>
		/// witholds/cancels dragdrop on dragleave operation
		/// </summary>
		public ICommand GroupNoteHoldCommand {
			get { return f_GroupNoteHoldCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupNoteHoldCommand == null) f_GroupNoteHoldCommand = value; }
		}

		private ICommand? f_GroupNoteHoldCommand;

		/// <summary>
		/// adds note to group on dragdrop release
		/// </summary>
		public ICommand GroupNoteDropCommand {
			get { return f_GroupNoteDropCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupNoteDropCommand == null) f_GroupNoteDropCommand = value; }
		}

		private ICommand? f_GroupNoteDropCommand;

		/// <summary>
		/// change the title
		/// </summary>
		public ICommand GroupChangeTitleCommand {
			get { return f_GroupChangeTitleCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupChangeTitleCommand == null) f_GroupChangeTitleCommand = value; }
		}

		private ICommand? f_GroupChangeTitleCommand;

		/// <summary>
		/// update the title
		/// </summary>
		public ICommand GroupUpdateTitleCommand {
			get { return f_GroupUpdateTitleCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupUpdateTitleCommand == null) f_GroupUpdateTitleCommand = value; }
		}

		private ICommand? f_GroupUpdateTitleCommand;

		/// <summary>
		/// update the color
		/// </summary>
		public ICommand GroupUpdateColorCommand {
			get { return f_GroupUpdateColorCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupUpdateColorCommand == null) f_GroupUpdateColorCommand = value; }
		}

		private ICommand? f_GroupUpdateColorCommand;

		#endregion



		#region ViewModel Contexts

		public StateViewModel StateViewModel { get; private set; }
		public StatusBarViewModel StatusBarViewModel { get; private set; }
		public NoteTextViewModel NoteTextViewModel { get; private set; }
		public GroupTabsViewModel GroupTabsViewModel { get; private set; }
		public NoteListViewModel NoteListViewModel { get; private set; }

		#endregion



		#region Constructor

		public PrimeViewModel (
			StateViewModel stateViewModel,
			StatusBarViewModel statusBarViewModel,
			NoteTextViewModel noteTextViewModel,
			GroupTabsViewModel groupTabsViewModel,
			NoteListViewModel noteListViewModel)
		{
			// set ViewModel context dependencies
			StateViewModel = stateViewModel;
			NoteListViewModel = noteListViewModel;
			GroupTabsViewModel = groupTabsViewModel;
			NoteTextViewModel = noteTextViewModel;
			StatusBarViewModel = statusBarViewModel;

			// default to no selected item
			f_SelectedNoteViewModel = null;

			// attach handlers
			SetSelectedChangedEventHandler();
			SetNoteCountChangedEventHandler();
			SetGroupCountChangedEventHandler();
			SetGroupNoteCountChangedEventHandler();
			SetNoteTextChangedEventHandler();
			SetGroupStringsChangedEventHandler();
		}

		private void SetSelectedChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "SelectedNoteViewModel") {
					StatusBarViewModel.NoteTextCursorLinePos = 0;
					StatusBarViewModel.NoteTextCursorColumnPos = 0;
				}
			};

			PropertyChanged += handler;
		}

		private void SetNoteCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "ItemCount") {
					StatusBarViewModel.NoteCount = ((NoteListViewModel)sender).ItemCount;
				}
			};

			NoteListViewModel.PropertyChanged += handler;
		}

		private void SetGroupCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "GroupCount") {
					StatusBarViewModel.GroupCount = ((GroupTabsViewModel)sender).GroupCount;
				}
			};

			GroupTabsViewModel.PropertyChanged += handler;
		}

		private void SetGroupNoteCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "GroupNoteCount") {
					StatusBarViewModel.GroupNoteCount = ((GroupTabsViewModel)sender).GroupNoteCount;
				}
			};

			GroupTabsViewModel.PropertyChanged += handler;
		}

		private void SetNoteTextChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "LineIndex") {
					StatusBarViewModel.NoteTextCursorLinePos = ((NoteTextViewModel)sender).LineIndex;
				}
				else if (e.PropertyName == "ColumnIndex") {
					StatusBarViewModel.NoteTextCursorColumnPos = ((NoteTextViewModel)sender).ColumnIndex;
				}
				else if (e.PropertyName == "IsNewGroupAllowed") {
					StateViewModel.IsNewGroupAllowed = ((NoteTextViewModel)sender).IsNewGroupAllowed;
				}
			};

			NoteTextViewModel.PropertyChanged += handler;
		}

		private void SetGroupStringsChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "Incoming") {
					NoteListObjectViewModel? incoming = ((GroupContentsViewModel)sender).Incoming;

					if (incoming != null &&
						SelectedNoteViewModel != null &&
						incoming.DataId == SelectedNoteViewModel.DataId) {
						NoteTextViewModel.GroupStrings = NoteTextViewModel.NoteGroupsToString(incoming.Model.Data);
					}
				}
			};

			GroupTabsViewModel.GroupContentsViewModel.PropertyChanged += handler;
		}

		#endregion



		#region Load

		/// <summary>
		/// post-view-load behavior and assignments for the viewmodel
		/// </summary>
		public void Load ()
		{
			//// if no notes exist, create one
			//if (NoteListViewModel.Items.Count() == 0) {
			//	AddNote(NoteListViewModel.Create());
			//}

			// select the most recently selected or first note
			IEnumerable<NoteListObjectViewModel> match =
				NoteListViewModel.Items.Where((item) => StateViewModel.SelectedNoteListItemId == item.ItemId);

			if (match.Any()) {
				NoteListViewModel.Highlighted = match.First();
			}
			//else {
			//	NoteListViewModel.Highlighted = NoteListViewModel.Items.First();
			//}
			NoteListObjectViewModel? highlighted = NoteListViewModel.Highlighted;

			if (highlighted != null) {
				if (NoteSelectCommand.CanExecute(highlighted)) {
					NoteSelectCommand.Execute(highlighted);
				}
			}
		}

		#endregion



		#region Events

		public void SetGroupsOnNote (NoteListObjectViewModel target)
		{
			// get data sources
			IEnumerable<string> groupTitleStrings = NoteTextViewModel.GroupStringList;
			IEnumerable<GroupListObjectViewModel> searchTarget = GroupTabsViewModel.GroupListViewModel.Items;

			// insert existing groups into the groups-to-associate-with-notes queue and remove them from missing list
			List<string> missingGroups = new List<string>(groupTitleStrings);
			List<GroupListObjectViewModel> groups = new List<GroupListObjectViewModel>();
			IEnumerable<GroupListObjectViewModel> foundGroups =
				NoteTextViewModel.FindExistingGroupsInStrings(groupTitleStrings, searchTarget);

			foreach (GroupListObjectViewModel groop in foundGroups) {
				groups.Add(groop);
				missingGroups.Remove(groop.Title);
			}

			if (NoteTextViewModel.IsNewGroupAllowed) {
				foreach (string groupTitle in missingGroups) {
					GroupListObjectViewModel newGroup =
						GroupTabsViewModel.GroupListViewModel.Create((obj) => { obj.Title = groupTitle; });
					GroupTabsViewModel.GroupListViewModel.Add(newGroup);
					groups.Add(newGroup);
				}
			}
			// NOTE: this case should be handled by validation and is almost identical in functionality
			//       to the above, so nothing more should need to be done here.
			else if (missingGroups.Any()) {
				return;
			}

			// find groups which are no longer associated with a note
			foreach (GroupListObjectViewModel groop in GroupTabsViewModel.GroupListViewModel.Items) {
				Group groopData = groop.Model.Data;
				Note noteData = target.Model.Data;

				// remove a GroupObject if no longer associated with a note
				IEnumerable<GroupListObjectViewModel> match =
					groups.Where((gn) => gn.DataId == groop.DataId);

				if (!match.Any()) {
					GroupTabsViewModel.GroupContentsViewModel.RemoveGroupObjectByGroup(noteData, groopData);
					continue;
				}

				// if GroupObjectViewModel already exists in GroupContentsViewModel, skip this iteration
				if (GroupTabsViewModel.GroupContentsViewModel.HasNoteInGroup(groopData, noteData)) continue;

				// associate a GroupObjectViewModel (aka 'GroupNote') newly created for a group with given GroupObject
				GroupObjectViewModel groupNote = GroupTabsViewModel.GroupContentsViewModel.Create(groopData, noteData);

				// add the GroupObject to the contents list
				GroupTabsViewModel.GroupContentsViewModel.Add(groupNote);
			}
		}



		/// <summary>
		/// adds external data to the NoteList, e.g. test data
		/// </summary>
		/// <param name="input"></param>
		public void AddNote (NoteListObjectViewModel input)
		{
			NoteListViewModel.Add(input);
		}



		/// <summary>
		/// inserts a new note into the note list; selects the newly created note
		/// </summary>
		/// <param name="target">the location at where the item will be inserted</param>
		public NoteListObjectViewModel CreateNoteAt (NoteListObjectViewModel? target)
		{
			NoteListObjectViewModel output = NoteListViewModel.Create();
			// if target is null, try to use the selected item
			if (target == null) {
				target = SelectedNoteViewModel;
			}

			// de-select the target
			if (target != null) {
				target.IsSelected = false;
			}

			NoteListViewModel.Insert(target, output);

			// set text viewer
			if (NoteSelectCommand.CanExecute(output)) {
				NoteSelectCommand.Execute(output);
			}

			return output;
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyNote (NoteListObjectViewModel input)
		{
			AutoSelectFailSafe(input);

			//// add a list item if none remain
			//if (NoteListViewModel.Items.Count() == 1) {
			//	NoteListObjectViewModel newNote = CreateNoteAt(null);
			//	if (NoteSelectCommand.CanExecute(newNote)) {
			//		NoteSelectCommand.Execute(newNote);
			//	}
			//	NoteListViewModel.Highlighted = newNote;
			//}
			if (NoteListViewModel.Items.Count() == 1) {
				NoteListViewModel.Highlighted = null;
				if (NoteSelectCommand.CanExecute(NoteListViewModel.Highlighted)) {
					NoteSelectCommand.Execute(NoteListViewModel.Highlighted);
				}
			}

			// remove any existing note objects matching the input in any of the groups
			GroupTabsViewModel.RemoveGroupObjectsByNote(input.Model.Data);

			// remove the note
			NoteListViewModel.Remove(input);
		}

		public void UpdateGroupTitle (GroupListObjectViewModel target)
		{
			GroupTabsViewModel.UpdateGroupTitle(target);

			if (SelectedNoteViewModel != null) {
				Group groop = target.Model.Data;
				Note note = SelectedNoteViewModel.Model.Data;

				bool hasSelectedNoteInGroup =
					GroupTabsViewModel.GroupContentsViewModel.HasNoteInGroup(groop, note);

				if (hasSelectedNoteInGroup) {
					NoteTextViewModel.GroupStrings = NoteTextViewModel.NoteGroupsToString(note);
				}
			}

		}

		public void UpdateGroupColor (GroupListObjectViewModel target)
		{
			GroupTabsViewModel.UpdateGroupColor(target);
		}

		/// <summary>
		/// take temporary GroupObject and make it a permanent addition to the group
		/// </summary>
		public void AddNoteToGroup ()
		{
			GroupTabsViewModel.AddNoteToGroup();
		}

		public void HoldGroupNote ()
		{
			GroupTabsViewModel.HoldGroupNote();
		}

		/// <summary>
		/// selects another object linked to the input, for if/when the input becomes unavailable
		/// </summary>
		/// <param name="input"></param>
		private void AutoSelectFailSafe (NoteListObjectViewModel input)
		{
			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (NoteListViewModel.Highlighted == null) {
				NoteListViewModel.Highlighted = SelectedNoteViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedNoteViewModel == input) {
				if (NoteListViewModel.Highlighted == input) {
					if (input.Next != null) {
						if (NoteSelectCommand.CanExecute((NoteListObjectViewModel)input.Next)) {
							NoteSelectCommand.Execute((NoteListObjectViewModel)input.Next);
						}
					}
					else if (input.Previous != null) {
						if (NoteSelectCommand.CanExecute((NoteListObjectViewModel)input.Previous)) {
							NoteSelectCommand.Execute((NoteListObjectViewModel)input.Previous);
						}
					}
					NoteListViewModel.Highlighted = SelectedNoteViewModel;
				}
				else if (NoteListViewModel.Highlighted != null) {
					if (NoteSelectCommand.CanExecute(NoteListViewModel.Highlighted)) {
						NoteSelectCommand.Execute(NoteListViewModel.Highlighted);
					}
				}
			}
			else {
				if (NoteListViewModel.Highlighted == input) {
					NoteListViewModel.Highlighted = SelectedNoteViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}
		}

		/// <summary>
		/// do housekeeping (save changes, clear resources, etc.)
		/// </summary>
		public void Shutdown ()
		{
			RemoveAllEventHandlers();

			GroupTabsViewModel.Shutdown();
			NoteListViewModel.Shutdown();
			StateViewModel.Shutdown();
		}

		#endregion
	}
}
