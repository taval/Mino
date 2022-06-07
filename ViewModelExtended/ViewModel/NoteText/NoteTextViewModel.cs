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
			//f_ViewModelKit.CommandBuilder.MakeNoteText(this);
			f_ContentData = null;
		}

		#endregion



		#region Update

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
	}
}
