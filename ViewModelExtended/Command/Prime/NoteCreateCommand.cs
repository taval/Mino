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
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteCreateCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.CreateNote(
				f_PrimeViewModel.Resource.NoteListViewModel.Highlighted,
				f_PrimeViewModel.Resource.NoteListViewModel.Create()
			);
		}
	}
}
