using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteReceiveCommand : CommandBase
	{
		private readonly GroupContentsViewModel f_Context;

		public NoteReceiveCommand (GroupContentsViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			DragEventArgs e = (DragEventArgs)parameter;

			// prevent close button from performing operation
			Button? closeButton =
				(Button?)UIHelper.FindChildOrNull<Button>(((FrameworkElement)e.Source).Parent, "RemoveItemButton");

			if (e == null || e.Handled || closeButton?.IsMouseOver == true) return;

			e.Handled = true;

			// get the event source element
			ListView? listView = e.Source as ListView;

			if (listView == null || !listView.AllowDrop) return;

			// get the data from DragDrop operation
			Tuple<string, object> data =
				(Tuple<string, object>)e.Data.GetData(DataFormats.Serializable);

			// if the item comes from the same ListView, bail out
			string itemListName = data.Item1;

			if (itemListName.Equals(listView.Name)) return;

			// get the actual data to be sent
			NoteListObjectViewModel? dataContext = data.Item2 as NoteListObjectViewModel;

			if (dataContext == null) return;

			f_Context.Incoming = dataContext;
		}
	}
}
