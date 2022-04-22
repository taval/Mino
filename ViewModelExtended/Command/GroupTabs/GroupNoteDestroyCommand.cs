using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupNoteDestroyCommand : CommandBase
	{
		private readonly GroupTabsViewModel m_GroupTabsViewModel;

		public GroupNoteDestroyCommand (GroupTabsViewModel groupTabsViewModel)
		{
			m_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			//m_GroupTabsViewModel.DestroyNote(m_GroupTabsViewModel.NoteListViewModel.Removed);
			m_GroupTabsViewModel.DestroyGroupNote((GroupObjectViewModel)parameter);
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
