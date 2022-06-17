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
        public int MaxChars { get; set; }

        public NoteTitleRule ()
        {
            MaxChars = 255;
        }

        public override ValidationResult Validate (object value, CultureInfo cultureInfo)
        {
            string title = String.Empty;

            try {
                if (((string)value).Length > 0)
                    title = (String)value;
            }
            catch (Exception e) {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if (!IsValidFileNameOrPath(title)) {
                return new ValidationResult(false,
                  $"Please enter a valid filename.");
            }
            return ValidationResult.ValidResult;
        }

        bool IsValidFileNameOrPath (string? name)
        {
            if (name == null || name.Equals(String.Empty)) return false;

            // determine if invalid characters in filename
            foreach (char invalidChar in System.IO.Path.GetInvalidPathChars()) {
                if (name.Contains(invalidChar)) return false;
            }

            // if longer than non-system specific length, fail
            if (name.Length > MaxChars) return false;

            return true;
        }
    }
}







