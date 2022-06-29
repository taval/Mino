using System;
using System.Collections.Generic;
using System.Text;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupUpdateColorCommand : CommandBase
	{
		private readonly PrimeViewModel f_Context;

		public GroupUpdateColorCommand (PrimeViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			// TODO: GroupUpdateColorCommand's usage is unknown and is just a placeholder.
			// whether the group is passed directly vs via event is just assumed
			if (!(parameter is GroupListObjectViewModel)) return;
			f_Context.UpdateGroupColor((GroupListObjectViewModel)parameter);
		}
	}
}
