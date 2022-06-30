using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

// TODO: IDataErrorInfo/ValidatesOnDataErrors="True" for model validations; use ValidationRules in markup for view validation

namespace Mino
{
	public class NoteTitleRule : ValidationRule
	{
		public int MaxChars {
			get { return MaxCharacters; }
			set { MaxCharacters = value; }
		}

		public static int MaxCharacters { get; private set; }

		static NoteTitleRule ()
		{
			MaxCharacters = 255; // default max; overridden by MaxChars
		}

		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			string title = String.Empty;

			try {
				if (((string)value).Length > 0) title = (String)value;
			}
			catch (Exception e) {
				return new ValidationResult(false, $"Illegal characters or { e.Message }.");
			}

			if (!IsValidNoteTitle(title)) {
				return new ValidationResult(false, $"Enter a valid title - allowed characters: alphanumeric, space, ( ) _ - , .");
			}

			return ValidationResult.ValidResult;
		}

		public static bool IsValidNoteTitle (string? title)
		{
			return Utility.IsValidFileNameOrPath(title, MaxCharacters);
		}
	}
}
