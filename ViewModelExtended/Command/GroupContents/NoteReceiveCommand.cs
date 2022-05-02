using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteReceiveCommand : CommandBase
	{
		private readonly GroupContentsViewModel m_GroupContentsViewModel;

		public NoteReceiveCommand (GroupContentsViewModel groupContentsViewModel)
		{
			m_GroupContentsViewModel = groupContentsViewModel;
		}

		public override void Execute (object parameter)
		{
			DragEventArgs e = (DragEventArgs)parameter;

			// prevent close button from performing operation
			Button? closeButton = UIHelper.FindChild<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");

			if (!(e.Source is FrameworkElement) || closeButton?.IsMouseOver == true) return;

			// get the data from DragDrop operation
			Tuple<string, object> data =
				(Tuple<string, object>)e.Data.GetData(DataFormats.Serializable);

			// if the item comes from the same ListView, bail out
			string itemName = data.Item1;
			

			if (itemName.Equals("ListView_GroupContentsView")) return;

			// get the actual data to be sent
			if (data.Item2 == null || !(data.Item2 is NoteListObjectViewModel)) return;


			NoteListObjectViewModel dataContext = (NoteListObjectViewModel)data.Item2;

			//m_GroupContentsViewModel.ReceiveGroupNote(dataContext);
			m_GroupContentsViewModel.Incoming = dataContext;
		}
	}
}
