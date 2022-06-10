using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: incomplete/invalid Notes should be disallowed from GroupContentsViewModel addition/insertion

namespace ViewModelExtended.ViewModel
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
			set { Set(ref f_SelectedNoteViewModel, value); }
		}

		private NoteListObjectViewModel? f_SelectedNoteViewModel;

		#endregion



		#region Commands

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



		#region Kit

		//private IViewModelKit f_ViewModelKit;

		#endregion



		#region ViewModel Contexts

		public StatusBarViewModel StatusBarViewModel { get; private set; }
		public NoteTextViewModel NoteTextViewModel { get; private set; }
		public GroupTabsViewModel GroupTabsViewModel { get; private set; }
		public NoteListViewModel NoteListViewModel { get; private set; }

		#endregion



		#region Constructor

		//public PrimeViewModel (
		//	IViewModelKit viewModelKit,
		//	StatusBarViewModel statusBarViewModel,
		//	NoteTextViewModel noteTextViewModel,
		//	GroupTabsViewModel groupTabsViewModel,
		//	NoteListViewModel noteListViewModel)
		//{
		public PrimeViewModel (
			StatusBarViewModel statusBarViewModel,
			NoteTextViewModel noteTextViewModel,
			GroupTabsViewModel groupTabsViewModel,
			NoteListViewModel noteListViewModel)
		{
			// set ViewModel kit
			//f_ViewModelKit = viewModelKit;

			// set ViewModel context dependencies
			NoteListViewModel = noteListViewModel;
			GroupTabsViewModel = groupTabsViewModel;
			NoteTextViewModel = noteTextViewModel;
			StatusBarViewModel = statusBarViewModel;

			// default to no selected item
			f_SelectedNoteViewModel = null;

			// attach commands
			//f_ViewModelKit.CommandBuilder.MakePrime(this);

			// attach handlers
			SetSelectedChangedEventHandler();
			SetNoteCountChangedEventHandler();
			SetGroupCountChangedEventHandler();
			SetGroupNoteCountChangedEventHandler();
			SetCursorPosChangedEventHandler();
		}

		public void SetSelectedChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "SelectedNoteViewModel") {
					if (SelectedNoteViewModel != null) {
						StatusBarViewModel.SelectedItemId = SelectedNoteViewModel.ItemId;
						StatusBarViewModel.SelectedDateCreated = SelectedNoteViewModel.DateCreated;
					}
					else {
						StatusBarViewModel.SelectedItemId = -1;
						StatusBarViewModel.SelectedDateCreated = DateTime.MinValue;
					}
				}
			};

			PropertyChanged += handler;
		}

		private void SetNoteCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "ItemCount") {
					StatusBarViewModel.NoteCount = NoteListViewModel.ItemCount;
				}
			};

			NoteListViewModel.PropertyChanged += handler;
		}

		private void SetGroupCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "GroupCount") {
					StatusBarViewModel.GroupCount = GroupTabsViewModel.GroupCount;
				}
			};

			GroupTabsViewModel.PropertyChanged += handler;
		}

		private void SetGroupNoteCountChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "GroupNoteCount") {
					StatusBarViewModel.GroupNoteCount = GroupTabsViewModel.GroupNoteCount;
				}
			};

			GroupTabsViewModel.PropertyChanged += handler;
		}

		private void SetCursorPosChangedEventHandler ()
		{
			PropertyChangedEventHandler handler = (sender, e) =>
			{
				if (e.PropertyName == "LineNumber") {
					StatusBarViewModel.NoteTextCursorLinePos = NoteTextViewModel.LineNumber;
				}
				else if (e.PropertyName == "ColumnNumber") {
					StatusBarViewModel.NoteTextCursorColumnPos = NoteTextViewModel.ColumnNumber;
				}
			};

			NoteTextViewModel.PropertyChanged += handler;
		}

		#endregion



		#region Load

		/// <summary>
		/// post-view-load behavior and assignments for the viewmodel
		/// </summary>
		public void Load ()
		{
			// if no notes exist, create one
			if (NoteListViewModel.Items.Count() == 0) {
				AddNote(NoteListViewModel.Create());
			}

			// select the first note
			NoteListViewModel.Highlighted = NoteListViewModel.Items.First();
			NoteListObjectViewModel highlighted = NoteListViewModel.Highlighted;

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

			foreach (GroupListObjectViewModel groop in groups) {
				// create a GroupObjectViewModel (a 'GroupNote') for each group, if one does not already exist
				Group groopData = groop.Model.Data;
				Note noteData = target.Model.Data;

				if (GroupTabsViewModel.GroupContentsViewModel.HasNoteInGroup(groopData, noteData)) continue;

				// associate a newly created GroupObject with the given temporary GroupObject
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
		/// <param name="input">the item to insert</param>
		//public void CreateNote (NoteListObjectViewModel? target, NoteListObjectViewModel input)
		//{
		//	// if target is null, try to use the selected item
		//	if (target == null) {
		//		target = SelectedNoteViewModel;
		//	}

		//	// de-select the target
		//	if (target != null) {
		//		target.IsSelected = false;
		//	}

		//	f_NoteListViewModel.Insert(target, input);

		//	// set text viewer
		//	SelectNote(input);
		//}

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
			//SelectNote(output);
			if (NoteSelectCommand.CanExecute(output)) {
				NoteSelectCommand.Execute(output);
			}

			return output;
		}

		/// <summary>
		/// // set the Text viewer to the selected note (this is generally the SelectedNote passed in from NoteList to Prime)
		/// </summary>
		/// <param name="note"></param>
		public void SelectNote (NoteListObjectViewModel note)
		{
			SelectedNoteViewModel = note;
			NoteTextViewModel.ContentData = note;
			NoteTextViewModel.ContentData.IsSelected = true;
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyNote (NoteListObjectViewModel input)
		{
			AutoSelectFailSafe(input);

			// add a list item if none remain
			if (NoteListViewModel.Items.Count() == 1) {
				//NoteListObjectViewModel newNote = f_NoteListViewModel.Create();
				//CreateNote(null, newNote);
				NoteListObjectViewModel newNote = CreateNoteAt(null);
				//SelectNote(newNote);
				if (NoteSelectCommand.CanExecute(newNote)) {
					NoteSelectCommand.Execute(newNote);
				}
				NoteListViewModel.Highlighted = newNote;
			}

			// remove any existing note objects matching the input in any of the groups
			GroupTabsViewModel.RemoveGroupObjectsByNote(input.Model.Data);

			// remove the note
			NoteListViewModel.Remove(input);
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
						//SelectNote((NoteListObjectViewModel)input.Next);
						if (NoteSelectCommand.CanExecute((NoteListObjectViewModel)input.Next)) {
							NoteSelectCommand.Execute((NoteListObjectViewModel)input.Next);
						}
					}
					else if (input.Previous != null) {
						//SelectNote((NoteListObjectViewModel)input.Previous);
						if (NoteSelectCommand.CanExecute((NoteListObjectViewModel)input.Previous)) {
							NoteSelectCommand.Execute((NoteListObjectViewModel)input.Previous);
						}
					}
					NoteListViewModel.Highlighted = SelectedNoteViewModel;
				}
				else if (NoteListViewModel.Highlighted != null) {
					//SelectNote(NoteListViewModel.Highlighted);
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

			//f_GroupContentsViewModel.Shutdown();
			//f_GroupListViewModel.Shutdown();
			GroupTabsViewModel.Shutdown();
			NoteListViewModel.Shutdown();
		}

		#endregion
	}
}
