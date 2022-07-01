using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mino.ViewModel;



namespace Mino
{
	public class GroupStringsRule : ValidationRule
	{
		public GroupStringsRule ()
		{
		}

		public override ValidationResult Validate (object value, CultureInfo cultureInfo)
		{
			string groupStrings = (string)value;

			if (!IsValidGroupStrings(groupStrings)) {
				return new ValidationResult(false, $"Enter a valid space-separated list of tags");
			}
			return ValidationResult.ValidResult;
		}

		private bool IsValidGroupStrings (string? groupStrings)
		{
			// invalid on null, empty string is ok
			if (groupStrings == null) return false;
			if (groupStrings.Equals(String.Empty)) return true;

			// if IsNewGroupAllowed == false, fail if any groups in the user string don't match with existing group
			ViewModelContext context = (ViewModelContext)Application.Current.MainWindow.DataContext;

			// get data sources
			IEnumerable<string> groupTitleStrings = context.NoteTextViewModel.GroupStringListFromString(groupStrings);
			IEnumerable<GroupListObjectViewModel> searchTarget = context.GroupTabsViewModel.GroupListViewModel.Items;

			// remove existing groups from missing list
			List<string> missingGroups = new List<string>(groupTitleStrings);
			IEnumerable<GroupListObjectViewModel> foundGroups =
				context.NoteTextViewModel.FindExistingGroupsInStrings(groupTitleStrings, searchTarget);

			foreach (GroupListObjectViewModel groop in foundGroups) {
				IEnumerable<string> titleToRemove = missingGroups.Where((g) => g.Equals(groop.Title));
				if (titleToRemove.Any()) {
					missingGroups.Remove(titleToRemove.Single());
				}
			}

			if (!context.NoteTextViewModel.IsNewGroupAllowed && missingGroups.Any()) {
				return false;
			}

			return true;
		}
	}
}
