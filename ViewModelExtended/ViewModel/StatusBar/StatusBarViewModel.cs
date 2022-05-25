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

				return m_SelectedItemId;
			}
			private set { Set(ref m_SelectedItemId, value); }
		}

		private int m_SelectedItemId;

		public DateTime SelectedDateCreated {
			get {
				if (Resource.PrimeViewModel.SelectedNoteViewModel != null) {
					SelectedDateCreated = Resource.PrimeViewModel.SelectedNoteViewModel.DateCreated;
				}
				else {
					SelectedDateCreated = new DateTime(1966, 9, 8);
				}

				return m_SelectedDateCreated;
			}
			private set { Set(ref m_SelectedDateCreated, value); }
		}

		private DateTime m_SelectedDateCreated;

		public int ItemCount {
			get {
				if (Resource.NoteListViewModel != null) {
					ItemCount = Resource.NoteListViewModel.ItemCount;
				}
				else {
					ItemCount = 0;
				}

				return m_ItemCount;
			}
			private set { Set(ref m_ItemCount, value); }
		}

		private int m_ItemCount;
	}
}
