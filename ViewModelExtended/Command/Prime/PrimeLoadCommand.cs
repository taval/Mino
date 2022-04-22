using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class PrimeLoadCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public PrimeLoadCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			m_PrimeViewModel.Load();
		}
	}
}
