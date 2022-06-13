using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended
{
	public class PriorityRule : ValidationRule
	{
		public PriorityRule ()
		{
		}

		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
            if ((int)value >= NoteListObjectViewModel.PriorityTypes.Count) {
                return new ValidationResult(false, $"Please enter a valid priority level.");
            }

            return ValidationResult.ValidResult;
        }
	}
}
