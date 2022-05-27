﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended.Command
{
	public class NoteDestroyCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteDestroyCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.DestroyNote((NoteListObjectViewModel)parameter);
		}
	}
}
