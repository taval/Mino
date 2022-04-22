using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class GroupReorderCommand : CommandBase
	{
		private readonly GroupListViewModel m_ListViewModel;

		public GroupReorderCommand (GroupListViewModel listViewModel)
		{
			m_ListViewModel = listViewModel;
		}

		public override void Execute (object parameter)
		{
			DragEventArgs? e = parameter as DragEventArgs;

			if (e == null) {
				return;
			}

			FrameworkElement? element = e.Source as FrameworkElement;

			if (element == null) {
				return;
			}

			IListItem? target = element.DataContext as IListItem;
			IListItem? source = e.Data.GetData(DataFormats.Serializable) as IListItem;

			if (source != null && target != null) {
				m_ListViewModel.Reorder(source, target);
			}

			// refresh list data
			//m_ListViewModel.RefreshListView();
		}
	}
}
