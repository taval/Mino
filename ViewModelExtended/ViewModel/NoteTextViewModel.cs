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
		public IViewModelResource Resource { get; private set; }



		#region Content Data: displayed data from a list source

		public NoteListObjectViewModel? ContentData {
			get { return m_ContentData; }
			set {
				Set(ref m_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Text));
			}
		}

		private NoteListObjectViewModel? m_ContentData;

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



		#region Commands

		public ICommand ChangeTitleCommand {
			get { return m_ChangeTitleCommand ?? throw new MissingCommandException(); }
			set { if (m_ChangeTitleCommand == null) m_ChangeTitleCommand = value; }
		}

		private ICommand? m_ChangeTitleCommand;

		public ICommand ChangeTextCommand {
			get { return m_ChangeTextCommand ?? throw new MissingCommandException(); }
			set { if (m_ChangeTextCommand == null) m_ChangeTextCommand = value; }
		}

		private ICommand? m_ChangeTextCommand;

		#endregion



		#region Constructor

		public NoteTextViewModel (IViewModelResource resource)
		{
			Resource = resource;
			Resource.CommandBuilder.MakeNoteText(this);
			m_ContentData = null;
		}

		#endregion



		#region Update

		public void UpdateTitle ()
		{
			if (m_ContentData == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.UpdateNote(m_ContentData.Model.Data, Title, null);
				dbContext.Save();
			}
		}

		public void UpdateText ()
		{
			if (m_ContentData == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.UpdateNote(m_ContentData.Model.Data, null, Text);
				dbContext.Save();
			}
		}

		#endregion
	}
}
