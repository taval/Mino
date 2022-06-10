using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended
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
                return new ValidationResult(false,
                  $"Please enter a valid space-separated list of tags.");
            }
            return ValidationResult.ValidResult;
        }

        bool IsValidGroupStrings (string? groupStrings)
        {
            if (groupStrings == null || groupStrings.Equals(String.Empty)) return false;

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
