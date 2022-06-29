using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Mino.ViewModel;



namespace Mino
{
	public class GroupTitleRule : ValidationRule
	{
		public GroupTitleRule ()
		{
		}

		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			if (!IsValidNewGroupTitle((string)value)) {
				return new ValidationResult(false, $"A tag may only consist of a single word with no spaces.");
			}

			return ValidationResult.ValidResult;
		}

		// NOTE: an existing valid group will give always positive response to HasGroup so don't check for it here
		public static bool IsValidGroupTitle (string? title)
		{
			if (title == null || title.Equals(String.Empty)) return false;

			return !ContainsSpaces(title);
		}

		// NOTE: this is the implementation used by the view for new Groups
		private static bool IsValidNewGroupTitle (string? title)
		{
			if (title == null || title.Equals(String.Empty)) return false;

			return !(HasGroup(title) || ContainsSpaces(title));
		}

		private static bool HasGroup (string title)
		{
			ViewModelContext context = (ViewModelContext)Application.Current.MainWindow.DataContext;

			return context.GroupListViewModel.Items.Where((groop) => groop.Title == title).Any();
		}

		private static bool ContainsSpaces (string title)
		{
			return ((string)title).Contains(' ');
		}
	}
}
