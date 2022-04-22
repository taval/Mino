using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteDestroyCommand : CommandBase
	{
		private readonly PrimeViewModel m_PrimeViewModel;

		public NoteDestroyCommand (PrimeViewModel primeViewModel)
		{
			m_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			//m_PrimeViewModel.DestroyNote(m_PrimeViewModel.NoteListViewModel.Removed);
			m_PrimeViewModel.DestroyNote((NoteListObjectViewModel)parameter);
		}
	}

	//public class NoteRemoveCommand : CommandBase
	//{
	//	private readonly NoteListViewModel m_ListViewModel;

	//	public NoteRemoveCommand (NoteListViewModel listViewModel)
	//	{
	//		m_ListViewModel = listViewModel;
	//	}

	//	public override void Execute (object parameter)
	//	{
	//		m_ListViewModel.Inserted = new NoteViewModel(m_ListViewModel);
	//		if (m_ListViewModel.Removed == null) {
	//			return;
	//		}
	//		m_ListViewModel.Remove(m_ListViewModel.Removed);
	//	}
	//}
}
