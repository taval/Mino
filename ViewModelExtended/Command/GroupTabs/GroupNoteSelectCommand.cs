using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupNoteSelectCommand : CommandBase
	{
		private readonly GroupTabsViewModel m_GroupTabsViewModel;

		public GroupNoteSelectCommand (GroupTabsViewModel groupTabsViewModel)
		{
			m_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			//if (m_GroupTabsViewModel.SelectedNoteViewModel != null) {
			//	m_GroupTabsViewModel.SelectNote(m_GroupTabsViewModel.SelectedNoteViewModel);
			//}
			m_GroupTabsViewModel.SelectGroupNote((GroupObjectViewModel)parameter);
		}
	}
}
