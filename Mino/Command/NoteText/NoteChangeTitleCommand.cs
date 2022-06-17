using System;
using System.Collections.Generic;
using System.Text;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteChangeTitleCommand : CommandBase
	{
		private readonly NoteTextViewModel f_NoteTextViewModel;

		public NoteChangeTitleCommand (NoteTextViewModel noteTextViewModel)
		{
			f_NoteTextViewModel = noteTextViewModel;
		}

		public override void Execute (object parameter)
		{
			f_NoteTextViewModel.UpdateTitle();
		}
	}
}
