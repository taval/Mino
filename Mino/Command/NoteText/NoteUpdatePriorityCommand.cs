using System;
using System.Text;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteUpdatePriorityCommand : CommandBase
	{
		private readonly NoteTextViewModel f_Context;

		public NoteUpdatePriorityCommand (NoteTextViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			f_Context.UpdatePriority();
		}
	}
}
