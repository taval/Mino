using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Mino.ViewModel;



namespace Mino
{
	public class PriorityRule : ValidationRule
	{
		public PriorityRule ()
		{
		}

		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			if (!IsValidPriority((int)value)) {
				return new ValidationResult(false, $"Enter a valid priority level (0 - 2)");
			}

			return ValidationResult.ValidResult;
		}

		public static bool IsValidPriority (int priority)
		{
			return priority < NoteListObjectViewModel.PriorityTypes.Count;
		}
	}
}
