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
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteSelectCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.SelectNote((NoteListObjectViewModel)parameter);
		}
	}
}
