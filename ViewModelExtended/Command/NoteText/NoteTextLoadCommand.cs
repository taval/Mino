using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ViewModelExtended.ViewModel;

namespace ViewModelExtended.Command
{
	public class NoteTextLoadCommand : CommandBase
	{
		private readonly NoteTextViewModel f_Context;

		public NoteTextLoadCommand (NoteTextViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			// set Priority ComboBox to list of strings defined in NoteListObjectViewModel class
			//NoteTextView noteTextView = UIHelper.GetChildOfType<NoteTextView>(Application.Current.MainWindow);
			//ComboBox comboBox = UIHelper.GetChildOfType<ComboBox>(noteTextView);

			NoteListObjectViewModel.PriorityTypes.Add("Low");
			NoteListObjectViewModel.PriorityTypes.Add("Medium");
			NoteListObjectViewModel.PriorityTypes.Add("High");

			//comboBox.ItemsSource = NoteListObjectViewModel.PriorityTypes;

			// do viewmodel load
			f_Context.Load();
		}
	}
}
