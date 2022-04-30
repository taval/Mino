using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupNoteDropCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public GroupNoteDropCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			m_PrimeViewModel.AddNoteToGroup();

		}
	}
}
