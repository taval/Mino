using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupUpdateTitleCommand : CommandBase
	{
		private readonly PrimeViewModel f_Context;

		public GroupUpdateTitleCommand (PrimeViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			if (!(parameter is KeyEventArgs)) return;

			KeyEventArgs e = (KeyEventArgs)parameter;

			// check key type events for Enter or Tab key
			if (e == null || e.Handled || !(e.Source is TextBox) || !(e.Key == Key.Enter || e.Key == Key.Tab)) return;

			e.Handled = true;

			// unfocus the text box
			TextBox textBox = (TextBox)e.Source;

			textBox.Focus();

			// return the focus from the text box to the selected item
			ListViewItem? item = UIHelper.FindParent<ListViewItem>(textBox);

			if (item == null || !(item.DataContext is GroupListObjectViewModel)) return;
			item.Focus();

			GroupListObjectViewModel target = (GroupListObjectViewModel)item.DataContext;

			f_Context.UpdateGroupTitle(target);
		}
	}
}
