using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteSendCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public NoteSendCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			//if (m_ListViewModel.Highlighted != null) {
			//	m_ListViewModel.Send(m_ListViewModel.Highlighted);
			//}
			m_PrimeViewModel.SetOutgoing((NoteListObjectViewModel)parameter);
		}
	}
}
