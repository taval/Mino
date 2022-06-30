using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
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

		public override bool CanExecute (object parameter)
		{
			return ((ViewModelContext)Application.Current.MainWindow.DataContext).IsLoaded;
		}

		public override void Execute (object parameter)
		{
			f_Context.UpdateGroupTitle((GroupListObjectViewModel)parameter);
		}
	}
}
