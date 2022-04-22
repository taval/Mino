using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



namespace ViewModelExtended.ViewModel
{
	public class NoteTextViewModel : ViewModelBase
	{
		private IViewModelResource Resource { get; set; }



		#region Content Data: displayed data from a list source

		public NoteListObjectViewModel? ContentData {
			get { return m_ContentData; }
			set {
				Set(ref m_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Text));
			}
		}

		private NoteListObjectViewModel? m_ContentData = null;

		#endregion



		#region Properties

		public string Title {
			get {
				if (m_ContentData != null) return m_ContentData.Title;
				return String.Empty;
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Title, value)) return;
					m_ContentData.Title = value;
					NotifyPropertyChanged(nameof(Title));
				}
			}
		}

		public string Text {
			get {
				if (m_ContentData != null) return m_ContentData.Text;
				return "Nada";
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Text, value)) return;
					m_ContentData.Text = value;
					NotifyPropertyChanged(nameof(Text));
				}
			}
		}

		#endregion



		#region Constructor

		public NoteTextViewModel (IViewModelResource resource)
		{
			Resource = resource;
		}

		#endregion
	}
}
