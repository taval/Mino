using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteSetDefaultTextCommand : CommandBase
	{
		private readonly NoteListViewModel f_Context;

		public NoteSetDefaultTextCommand (NoteListViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			f_Context.DefaultText = UIHelper.CreateEmptyFlowDocument();
		}
	}
}
