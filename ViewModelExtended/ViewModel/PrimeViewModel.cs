using System;
using System.Collections.Generic;
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
			get { return m_SelectedNoteViewModel; }
			set { Set(ref m_SelectedNoteViewModel, value); }
		}

		private NoteListObjectViewModel? m_SelectedNoteViewModel;

		#endregion



		#region Commands

		public ICommand GroupNoteHoldCommand {
			get { return m_GroupNoteHoldCommand ?? throw new MissingCommandException(); }
			set { if (m_GroupNoteHoldCommand == null) m_GroupNoteHoldCommand = value; }
		}

		private ICommand? m_GroupNoteHoldCommand;

		public ICommand GroupNoteDropCommand {
			get { return m_GroupNoteDropCommand ?? throw new MissingCommandException(); }
			set { if (m_GroupNoteDropCommand == null) m_GroupNoteDropCommand = value; }
		}

		private ICommand? m_GroupNoteDropCommand;

		/// <summary>
		/// selects a Note in the NoteList
		/// </summary>
		public ICommand NoteSelectCommand {
			get { return m_NoteSelectCommand ?? throw new MissingCommandException(); }
			set { if (m_NoteSelectCommand == null) m_NoteSelectCommand = value; }
		}

		private ICommand? m_NoteSelectCommand;

		/// <summary>
		/// inserts an empty note into NoteList
		/// </summary>
		public ICommand NoteCreateCommand {
			get { return m_NoteCreateCommand ?? throw new MissingCommandException(); }
			set { if (m_NoteCreateCommand == null) m_NoteCreateCommand = value; }
		}

		private ICommand? m_NoteCreateCommand;

		/// <summary>
		/// removes a note from the NoteList
		/// </summary>
		public ICommand NoteDestroyCommand {
			get { return m_NoteDestroyCommand ?? throw new MissingCommandException(); }
			set { if (m_NoteDestroyCommand == null) m_NoteDestroyCommand = value; }
		}

		private ICommand? m_NoteDestroyCommand;

		/// <summary>
		/// loads ViewModel
		/// </summary>
		//public ICommand PrimeLoadCommand {
		//	get { return m_PrimeLoadCommand ?? throw new MissingCommandException(); }
		//	set { if (m_PrimeLoadCommand == null) m_PrimeLoadCommand = value; }
		//}

		//private ICommand? m_PrimeLoadCommand;

		#endregion



		#region Resource

		public IViewModelResource Resource { get; private set; }

		#endregion



		#region Constructor

		public PrimeViewModel (IViewModelResource resource)
		{
			Resource = resource;
			m_SelectedNoteViewModel = null;
			Resource.CommandBuilder.MakePrime(this);
		}

		#endregion



		#region Load

		/// <summary>
		/// upon view init, display the first Note
		/// </summary>
		public void Load ()
		{
			// if no notes exist, create one
			if (Resource.NoteListViewModel.Items.Count() == 0) {
				AddNote(Resource.NoteListViewModel.Create());
			}

			// select the first note
			SelectedNoteViewModel = Resource.NoteListViewModel.Items.First();
			if (SelectedNoteViewModel != null) {
				SelectNote(SelectedNoteViewModel);
			}

			// highlight the first note
			Resource.NoteListViewModel.Highlighted = SelectedNoteViewModel;

			// perform GroupTabs setup
			Resource.GroupTabsViewModel.Load();
		}

		#endregion



		#region Events

		/// <summary>
		/// adds external data to the NoteList, e.g. test data
		/// </summary>
		/// <param name="input"></param>
		public void AddNote (NoteListObjectViewModel input)
		{
			Resource.NoteListViewModel.Add(input);
		}

		/// <summary>
		/// inserts a new note into the note list; selects the newly created note
		/// </summary>
		/// <param name="target">the location at where the item will be inserted</param>
		/// <param name="input">the item to insert</param>
		public void CreateNote (NoteListObjectViewModel? target, NoteListObjectViewModel input)
		{
			// the insert target is the most recently highlighted Item
			//NoteListViewModel.Target = target;

			// using default NoteViewModel for creating a blank Note
			//NoteListViewModel.Inserted = input;

			// if target is null, try to use the selected item
			if (target == null) {
				target = SelectedNoteViewModel;
			}

			// de-select the target
			if (target != null) {
				target.IsSelected = false;
			}

			Resource.NoteListViewModel.Insert(target, input);

			// selected should now be set to the newly inserted item
			//SelectedNoteViewModel = input;

			// set text viewer
			SelectNote(input);
		}

		/// <summary>
		/// // set the Text viewer to the selected note (this is generally the SelectedNote passed in from NoteList to Prime)
		/// </summary>
		/// <param name="note"></param>
		public void SelectNote (NoteListObjectViewModel note)
		{
			SelectedNoteViewModel = note;
			Resource.NoteTextViewModel.ContentData = note;
			Resource.NoteTextViewModel.ContentData.IsSelected = true;
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
			if (Resource.NoteListViewModel.Items.Count() == 1) {
				NoteListObjectViewModel newNote = Resource.NoteListViewModel.Create();
				CreateNote(null, newNote);
				SelectNote(newNote);
				Resource.NoteListViewModel.Highlighted = newNote;
			}

			// remove any existing note objects matching the input in any of the groups
			Resource.GroupTabsViewModel.RemoveGroupObjectsByNote(input.Model.Data);

			// remove the note
			Resource.NoteListViewModel.Remove(input);
		}


		//// Receive
		//public void AddNoteToGroup (NoteListObjectViewModel input)
		//{
		//	Resource.GroupTabsViewModel.AddNoteToGroup(input);
		//}

		// Receive
		public void AddNoteToGroup ()
		{
			Resource.GroupTabsViewModel.AddNoteToGroup();
		}

		public void HoldGroupNote ()
		{
			Resource.GroupTabsViewModel.HoldGroupNote();
		}

		/// <summary>
		/// selects another object linked to the input, for if/when the input becomes unavailable
		/// </summary>
		/// <param name="input"></param>
		private void AutoSelectFailSafe (NoteListObjectViewModel input)
		{
			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (Resource.NoteListViewModel.Highlighted == null) {
				Resource.NoteListViewModel.Highlighted = SelectedNoteViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedNoteViewModel == input) {
				if (Resource.NoteListViewModel.Highlighted == input) {
					if (input.Next != null) {
						SelectNote((NoteListObjectViewModel)input.Next);
					}
					else if (input.Previous != null) {
						SelectNote((NoteListObjectViewModel)input.Previous);
					}
					Resource.NoteListViewModel.Highlighted = SelectedNoteViewModel;
				}
				else if (Resource.NoteListViewModel.Highlighted != null) {
					SelectNote(Resource.NoteListViewModel.Highlighted);
				}
			}
			else {
				if (Resource.NoteListViewModel.Highlighted == input) {
					Resource.NoteListViewModel.Highlighted = SelectedNoteViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}
		}

		#endregion
	}
}
