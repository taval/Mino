using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	public class StatusBarViewModel : ViewModelBase
	{
		public int SelectedItemId {
			get { return f_SelectedItemId; }
			set { Set(ref f_SelectedItemId, value); }
		}

		private int f_SelectedItemId;

		public DateTime SelectedDateCreated {
			get { return f_SelectedDateCreated; }
			set { Set(ref f_SelectedDateCreated, value); }
		}

		private DateTime f_SelectedDateCreated;

		public int NoteCount {
			get { return f_NoteCount; }
			set { Set(ref f_NoteCount, value); }
		}

		private int f_NoteCount;

		public int GroupCount {
			get { return f_GroupCount; }
			set { Set(ref f_GroupCount, value); }
		}

		private int f_GroupCount;

		public int GroupNoteCount {
			get { return f_GroupNoteCount; }
			set { Set(ref f_GroupNoteCount, value); }
		}

		private int f_GroupNoteCount;

		public int NoteTextCursorLinePos {
			get { return f_NoteTextCursorLinePos; }
			set {
				Set(ref f_NoteTextCursorLinePos, value);
				NotifyPropertyChanged(nameof(CursorPos));
			}
		}

		private int f_NoteTextCursorLinePos;

		public int NoteTextCursorColumnPos {
			get { return f_NoteTextCursorColumnPos; }
			set {
				Set(ref f_NoteTextCursorColumnPos, value);
				NotifyPropertyChanged(nameof(CursorPos));
			}
		}

		private int f_NoteTextCursorColumnPos;

		public string CursorPos {
			get {
				return
					$"Line: { f_NoteTextCursorLinePos }," +
					$"Column: { f_NoteTextCursorColumnPos }";
			}
		}
	}
}
