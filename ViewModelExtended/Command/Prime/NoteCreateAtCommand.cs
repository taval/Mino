using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteCreateAtCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteCreateAtCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.CreateNoteAt((NoteListObjectViewModel)parameter);
		}
	}
}
