using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteChangeTitleCommand : CommandBase
	{
		private readonly NoteTextViewModel m_NoteTextViewModel;

		public NoteChangeTitleCommand (NoteTextViewModel noteTextViewModel)
		{
			m_NoteTextViewModel = noteTextViewModel;
		}

		public override void Execute (object parameter)
		{
			m_NoteTextViewModel.UpdateTitle();
		}
	}
}
