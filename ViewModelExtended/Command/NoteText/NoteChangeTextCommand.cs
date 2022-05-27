using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteChangeTextCommand : CommandBase
	{
		private readonly NoteTextViewModel f_NoteTextViewModel;

		public NoteChangeTextCommand (NoteTextViewModel noteTextViewModel)
		{
			f_NoteTextViewModel = noteTextViewModel;
		}

		public override void Execute (object parameter)
		{
			f_NoteTextViewModel.UpdateText();
		}
	}
}
