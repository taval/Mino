using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Mino.Model;

// TODO: methods here in NoteTextViewModel associated with anything other than NoteListObjectViewModel belong in PrimeViewModel. This entails setting GroupStrings externally when ContentData is changed
namespace Mino.ViewModel
{
	public class NoteTextViewModel : ViewModelBase
	{
		#region Kit

		private IViewModelKit f_ViewModelKit;
		private StateViewModel f_StateViewModel;

		#endregion



		#region Content Data: displayed data from a list source

		public NoteListObjectViewModel? ContentData {
			get { return f_ContentData; }
			set {
				Set(ref f_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Text));
				NotifyPropertyChanged(nameof(Priority));
				NotifyPropertyChanged(nameof(IsTitleValid));
				NotifyPropertyChanged(nameof(TagFormBorderColor));
				NotifyPropertyChanged(nameof(GroupStrings));
				NotifyPropertyChanged(nameof(GroupStringList));
				if (f_ContentData != null) {
					Note note = f_ContentData.Model.Data;
					GroupStrings = NoteGroupsToString(note);
				}
				else {
					GroupStrings = String.Empty;
				}
			}
		}

		private NoteListObjectViewModel? f_ContentData;

		#endregion



		#region Properties

		public IList<string> PriorityTypes {
			get { return NoteListObjectViewModel.PriorityTypes; }
		}

		public string Title {
			get {
				if (f_ContentData != null) return f_ContentData.Title;
				return String.Empty;
			}
			set {
				if (f_ContentData != null) {
					if (Equals(f_ContentData.Title, value)) return;
					f_ContentData.Title = value;
					NotifyPropertyChanged(nameof(Title));
					NotifyPropertyChanged(nameof(IsTitleValid));
					NotifyPropertyChanged(nameof(TagFormBorderColor));
				}
			}
		}

		public string Text {
			get {
				if (f_ContentData != null) return f_ContentData.Text;
				return "Nada";
			}
			set {
				if (f_ContentData != null) {
					if (Equals(f_ContentData.Text, value)) return;
					f_ContentData.Text = value;
					NotifyPropertyChanged(nameof(Text));
				}
			}
		}

		public int Priority {
			get {
				if (f_ContentData != null) return f_ContentData.Priority;
				return 0;
			}
			set {
				if (f_ContentData != null) {
					if (Equals(f_ContentData.Priority, value)) return;
					f_ContentData.Priority = value;
					NotifyPropertyChanged(nameof(Priority));
				}
			}
		}

		public int LineIndex {
			get { return f_LineIndex; }
			set { Set(ref f_LineIndex, value); }
		}

		private int f_LineIndex;

		public int ColumnIndex {
			get { return f_ColumnIndex; }
			set { Set(ref f_ColumnIndex, value); }
		}

		private int f_ColumnIndex;

		/// <summary>
		/// user-inputted space-separated string list of Groups to which the Note will be attached
		/// </summary>
		public string GroupStrings {
			get { return f_GroupStrings ?? String.Empty; }
			set {
				Set(ref f_GroupStrings, value);
				NotifyPropertyChanged(nameof(GroupStringList));
			}
		}

		private string? f_GroupStrings;

		/// <summary>
		/// list of groups to store
		/// </summary>
		public IEnumerable<string> GroupStringList {
			get {
				if (f_GroupStrings != null) {
					return GroupStringListFromString(f_GroupStrings);
				}
				else {
					return Enumerable.Empty<string>();
				}
			}
		}

		/// <summary>
		/// if user selects true, ad-hoc group names are converted into new groups
		/// else, validation will fail and no changes to the groups attached to the note are made
		/// </summary>
		public bool IsNewGroupAllowed {
			get { return f_IsNewGroupAllowed; }
			set {
				Set(ref f_IsNewGroupAllowed, value);
				NotifyPropertyChanged(nameof(GroupStrings));
				NotifyPropertyChanged(nameof(GroupStringList));
			}
		}

		private bool f_IsNewGroupAllowed;

		/// <summary>
		/// if title is valid, return true, else return false
		/// </summary>
		public bool IsTitleValid {
			get { return NoteTitleRule.IsValidNoteTitle(Title); }
		}

		public string TagFormBorderColor {
			get { return (IsTitleValid) ? "#ccc" : "Transparent"; }
		}

		#endregion



		#region Commands

		public ICommand UpdateTitleCommand {
			get { return f_UpdateTitleCommand ?? throw new MissingCommandException(); }
			set { if (f_UpdateTitleCommand == null) f_UpdateTitleCommand = value; }
		}

		private ICommand? f_UpdateTitleCommand;

		public ICommand ChangeTextCommand {
			get { return f_ChangeTextCommand ?? throw new MissingCommandException(); }
			set { if (f_ChangeTextCommand == null) f_ChangeTextCommand = value; }
		}

		private ICommand? f_ChangeTextCommand;

		public ICommand UpdateTextCommand {
			get { return f_UpdateTextCommand ?? throw new MissingCommandException(); }
			set { if (f_UpdateTextCommand == null) f_UpdateTextCommand = value; }
		}

		private ICommand? f_UpdateTextCommand;

		public ICommand UpdatePriorityCommand {
			get { return f_UpdatePriorityCommand ?? throw new MissingCommandException(); }
			set { if (f_UpdatePriorityCommand == null) f_UpdatePriorityCommand = value; }
		}

		private ICommand? f_UpdatePriorityCommand;

		public ICommand CalcCursorPosCommand {
			get { return f_CalcCursorPosCommand ?? throw new MissingCommandException(); }
			set { if (f_CalcCursorPosCommand == null) f_CalcCursorPosCommand = value; }
		}

		private ICommand? f_CalcCursorPosCommand;

		#endregion



		#region Constructor

		public NoteTextViewModel (IViewModelKit viewModelKit, StateViewModel stateViewModel)
		{
			f_ViewModelKit = viewModelKit;
			f_StateViewModel = stateViewModel;
			ContentData = null;
		}

		#endregion



		#region Load

		public void Load ()
		{
			// TODO: datamodel load stuff here
			IsNewGroupAllowed = f_StateViewModel.IsNewGroupAllowed;

			NotifyPropertyChanged(nameof(PriorityTypes));
			NotifyPropertyChanged(nameof(Title));
			NotifyPropertyChanged(nameof(Text));
			NotifyPropertyChanged(nameof(Priority));
			NotifyPropertyChanged(nameof(GroupStrings));
			NotifyPropertyChanged(nameof(GroupStringList));
			NotifyPropertyChanged(nameof(IsTitleValid));
			NotifyPropertyChanged(nameof(TagFormBorderColor));
		}

		#endregion



		#region Note

		public void UpdateTitle ()
		{
			if (f_ContentData == null) return;

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				dbContext.UpdateNote(f_ContentData.Model.Data, Title, null, null);
				dbContext.Save();
			}
		}

		public void UpdateText ()
		{
			if (f_ContentData == null) return;

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				dbContext.UpdateNote(f_ContentData.Model.Data, null, Text, null);
				dbContext.Save();
			}
		}

		public void UpdatePriority ()
		{
			if (f_ContentData == null) return;

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				dbContext.UpdateNote(f_ContentData.Model.Data, null, null, Priority);
				dbContext.Save();
			}
		}

		#endregion



		#region Group

		/// <summary>
		/// convert a string of group names into a list of group names
		/// </summary>
		/// <param name="listString"></param>
		/// <returns></returns>
		public IEnumerable<string> GroupStringListFromString (string listString)
		{
			if ((listString == null) || listString.Equals(String.Empty)) return Enumerable.Empty<string>();
			return listString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>
		/// get a list of existing groups given a list of title strings
		/// </summary>
		/// <param name="groupTitleStrings"></param>
		/// <param name="searchTarget"></param>
		/// <returns></returns>
		public IEnumerable<GroupListObjectViewModel> FindExistingGroupsInStrings (
			IEnumerable<string> groupTitleStrings, IEnumerable<GroupListObjectViewModel> searchTarget)
		{
			foreach (string groupTitle in groupTitleStrings) {
				IEnumerable<GroupListObjectViewModel> hasTitleGroup =
					searchTarget.Where((item) => item.Title.Equals(groupTitle));

				if (hasTitleGroup.Any()) {
					yield return hasTitleGroup.Single();
				}
			}
		}

		public string NoteGroupsToString (Note note)
		{
			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				// get the unbound data objects
				IQueryable<GroupObjectViewModel> notesInGroup =
					f_ViewModelKit.DbQueryHelper.GetGroupObjectsByNote(dbContext, note);

				// create a string by joining group titles separated by space
				return String.Join(" ", notesInGroup.Select((groupNote) => groupNote.Group.Title));
			}
		}

		#endregion
	}
}
