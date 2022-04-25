using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModelExtended.Model;
using ViewModelExtended.ViewModel;



namespace ViewModelExtended
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected void LinkAll (IEnumerable<IListItem> items)
		{
			// create the relationship between Notes in a linked list
			// assumes note begin/end nodes will correctly contain null upon immediate construction

			IListItem? prev = null;
			IListItem? current = null;
			IListItem? next = null;

			foreach (IListItem note in items) {
				if (prev == null) {
					prev = note;
					//prev.Text = "First Node";
					continue;
				}
				if (prev != null && current == null) {
					current = note;
					continue;
				}
				if (prev != null && current != null) {
					next = note;
					prev.Next = current;
					current.Previous = prev;
					current.Next = next;
					next.Previous = current;
					prev = current;
					current = next;
				}
			}
		}

		protected override void OnStartup (StartupEventArgs e)
		{
			#region Resources

			AddResource("CloseButton");
			AddResource("Button");
			AddResource("WarnHighlight");
			//AddResource("LeftScrollViewer");
			//AddResource("ListView");
			AddResource("ListViewItem");

			//AddResource("TextBoxError");
			IViewModelResource resource = new ViewModelResource();

			#endregion



			#region Note List ViewModel

			// load test data
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "8:00 AM"; c.Text = "make video"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "9:00 AM"; c.Text = "walk the dog"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "9:30 AM"; c.Text = "eat dinner"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "10:15 AM"; c.Text = "watch tv"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "11:30 AM"; c.Text = "jog"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "12:00 PM"; c.Text = "mop"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "1:00 PM"; c.Text = "kick it"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "2:45 PM"; c.Text = "throw things"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "7:00 PM"; c.Text = "sweep"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "7:30 PM"; c.Text = "chill"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "8:00 PM"; c.Text = "run"; }));
			//resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
			//	c => { c.Title = "9:00 PM"; c.Text = "also run"; }));

			#endregion



			#region Group Tabs ViewModel

			//resource.GroupTabsViewModel.AddGroup(resource.ViewModelCreator.CreateGroupListObjectViewModel(
			//	c => { c.Title = "Chores"; }));
			//resource.GroupTabsViewModel.AddGroup(resource.ViewModelCreator.CreateGroupListObjectViewModel(
			//	c => { c.Title = "Exercises"; }));
			//resource.GroupTabsViewModel.AddGroup(resource.ViewModelCreator.CreateGroupListObjectViewModel(
			//	c => { c.Title = "Leisure"; }));

			#endregion



			#region Main Window

			MainWindowViewModel mainWindowViewModel = resource.ViewModelCreator.CreateMainWindowViewModel();

			MainWindow = new MainWindow() { DataContext = mainWindowViewModel };
			MainWindow.Show();

			#endregion



			#region Startup

			base.OnStartup(e);

			#endregion
		}



		#region Helper

		/// <summary>
		/// adds the ResourceDictionaries to MergedDictionaries
		/// </summary>
		/// <param name="resourceName"></param>
		private void AddResource (string resourceName)
		{
			string resourceString = $"{resourceName}Resource";
			Uri uri = new Uri($"/Themes/{resourceString}.xaml", UriKind.Relative);
			Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(uri));
		}

		#endregion
	}
}







// NOTE: the test data for GroupViewModel can stay
