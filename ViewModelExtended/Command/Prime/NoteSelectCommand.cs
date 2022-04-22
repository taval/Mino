using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteSelectCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public NoteSelectCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			//if (m_PrimeViewModel.SelectedNoteViewModel != null) {
			//	m_PrimeViewModel.SelectNote(m_PrimeViewModel.SelectedNoteViewModel);
			//}
			m_PrimeViewModel.SelectNote((NoteListObjectViewModel)parameter);
		}
	}
}
