using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	public class StatusBarViewModel : ViewModelBase
	{
		public IViewModelResource Resource { get; private set; }

		public StatusBarViewModel (IViewModelResource resource)
		{
			Resource = resource;
		}

		public int SelectedItemId {
			get {
				if (Resource.PrimeViewModel.SelectedNoteViewModel != null) {
					SelectedItemId = Resource.PrimeViewModel.SelectedNoteViewModel.ItemId;
				}
				else {
					SelectedItemId = -1;
				}

				return f_SelectedItemId;
			}
			private set { Set(ref f_SelectedItemId, value); }
		}

		private int f_SelectedItemId;

		public DateTime SelectedDateCreated {
			get {
				if (Resource.PrimeViewModel.SelectedNoteViewModel != null) {
					SelectedDateCreated = Resource.PrimeViewModel.SelectedNoteViewModel.DateCreated;
				}
				else {
					SelectedDateCreated = new DateTime(1966, 9, 8);
				}

				return f_SelectedDateCreated;
			}
			private set { Set(ref f_SelectedDateCreated, value); }
		}

		private DateTime f_SelectedDateCreated;

		public int NoteCount {
			get {
				if (Resource.NoteListViewModel != null) {
					f_NoteCount = Resource.NoteListViewModel.ItemCount;
				}
				else {
					f_NoteCount = 0;
				}

				return f_NoteCount;
			}
			private set { Set(ref f_NoteCount, value); }
		}

		private int f_NoteCount;

		public int GroupCount {
			get {
				if (Resource.GroupListViewModel != null) {
					f_GroupCount = Resource.GroupListViewModel.ItemCount;
				}
				else {
					f_GroupCount = 0;
				}

				return f_GroupCount;
			}
			private set { Set(ref f_GroupCount, value); }
		}

		private int f_GroupCount;

		public string CursorPos {
			get {
				f_CursorPos =
					$"Line: { Resource.NoteTextViewModel.LineNumber }," +
					$"Column: { Resource.NoteTextViewModel.ColumnNumber }";
				return f_CursorPos;
			}
			private set { Set(ref f_CursorPos, value); }
		}

		private string f_CursorPos;
	}
}
