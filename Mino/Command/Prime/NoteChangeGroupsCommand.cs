using System;
using System.Collections.Generic;
using System.Text;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteChangeGroupsCommand : CommandBase
	{
		private readonly PrimeViewModel f_Context;

		public NoteChangeGroupsCommand (PrimeViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			f_Context.SetGroupsOnNote((NoteListObjectViewModel)parameter);
		}
	}
}
