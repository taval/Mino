using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;



namespace Mino
{
	public abstract class CommandBase : ICommand
	{
		public event EventHandler CanExecuteChanged;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public CommandBase () {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public virtual bool CanExecute (object parameter)
		{
			return true;
		}

		public abstract void Execute (object parameter);

		protected void OnCanExecuteChanged ()
		{
			this.CanExecuteChanged?.Invoke(this, new EventArgs());
		}
	}
}
