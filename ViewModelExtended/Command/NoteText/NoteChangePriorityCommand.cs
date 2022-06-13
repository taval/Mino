using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteChangePriorityCommand : CommandBase
	{
		private readonly NoteTextViewModel f_Context;

		public NoteChangePriorityCommand (NoteTextViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			//RoutedEventArgs e = (RoutedEventArgs)parameter;

			//if (!(e.Source is ComboBox)) return;

			//ComboBox source = (ComboBox)e.Source;

			//int index = source.SelectedIndex;

			f_Context.UpdatePriority();
		}
	}
}
