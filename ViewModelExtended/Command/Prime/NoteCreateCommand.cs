using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;

// TODO: refactor: unwind this away from prime viewmodel - prime view should call this via attached property

namespace ViewModelExtended.Command
{
	public class NoteCreateCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public NoteCreateCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			m_PrimeViewModel.CreateNote(
				m_PrimeViewModel.Resource.NoteListViewModel.Highlighted,
				m_PrimeViewModel.Resource.NoteListViewModel.Create()
			);
			//m_PrimeViewModel.Resource.NoteListViewModel.Refresh();
		}
	}
}
