using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupNoteDropCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public GroupNoteDropCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.AddNoteToGroup();
		}
	}
}
