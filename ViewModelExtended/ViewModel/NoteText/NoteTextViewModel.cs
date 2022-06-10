using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class NoteTextViewModel : ViewModelBase
	{
		#region Kit

		private IViewModelKit f_ViewModelKit;

		#endregion



		#region Content Data: displayed data from a list source

		public NoteListObjectViewModel? ContentData {
			get { return f_ContentData; }
			set {
				Set(ref f_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Text));
			}
		}

		private NoteListObjectViewModel? f_ContentData;

		#endregion



		#region Properties

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

		public int LineNumber {
			get { return f_LineNumber; }
			set { Set(ref f_LineNumber, value); }
		}

		private int f_LineNumber;

		public int ColumnNumber {
			get { return f_ColumnNumber; }
			set { Set(ref f_ColumnNumber, value); }
		}

		private int f_ColumnNumber;

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
			set { Set(ref f_IsNewGroupAllowed, value); }
		}

		private bool f_IsNewGroupAllowed;

		#endregion



		#region Commands

		public ICommand ChangeTitleCommand {
			get { return f_ChangeTitleCommand ?? throw new MissingCommandException(); }
			set { if (f_ChangeTitleCommand == null) f_ChangeTitleCommand = value; }
		}

		private ICommand? f_ChangeTitleCommand;

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

		public ICommand CalcCursorPosCommand {
			get { return f_CalcCursorPosCommand ?? throw new MissingCommandException(); }
			set { if (f_CalcCursorPosCommand == null) f_CalcCursorPosCommand = value; }
		}

		private ICommand? f_CalcCursorPosCommand;

		#endregion



		#region Constructor

		public NoteTextViewModel (IViewModelKit viewModelKit)
		{
			f_ViewModelKit = viewModelKit;
			f_ContentData = null;
		}

		#endregion



		#region Note

		public void UpdateTitle ()
		{
			if (f_ContentData == null) return;

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				dbContext.UpdateNote(f_ContentData.Model.Data, Title, null);
				dbContext.Save();
			}
		}

		public void UpdateText ()
		{
			if (f_ContentData == null) return;

			using (IDbContext dbContext = f_ViewModelKit.CreateDbContext()) {
				dbContext.UpdateNote(f_ContentData.Model.Data, null, Text);
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

		#endregion
	}
}
