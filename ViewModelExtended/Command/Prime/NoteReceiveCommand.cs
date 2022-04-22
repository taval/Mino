using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteReceiveCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public NoteReceiveCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			m_PrimeViewModel.AddNoteToGroup((NoteListObjectViewModel)parameter);
		}
	}
}
