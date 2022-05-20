using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModelExtended.Model;
using ViewModelExtended.ViewModel;

// TODO: most exceptions should trigger a rollback to the last known good state and shutdown/commit properly
// TODO: use p_ for private properties, f_ for private fields (maybe m_ for private methods but prob best to keep camel case for that one)

namespace ViewModelExtended
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup (StartupEventArgs e)
		{
			#region Load Resources

			AddResource("CloseButton");
			AddResource("Button");
			AddResource("WarnHighlight");
			//AddResource("LeftScrollViewer");
			//AddResource("ListView");
			AddResource("ListViewItem");

			//AddResource("TextBoxError");
			IViewModelResource resource = new ViewModelResource();

			#endregion



			#region Load Test Data

			AddNoteListObjectTestData(resource);
			AddGroupListObjectTestData(resource);

			#endregion



			#region Setup ViewModel

			resource.PrimeViewModel.Load();

			#endregion



			#region Load Main Window

			MainWindow = new MainWindow() { DataContext = resource.MainWindowViewModel };
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

		/// <summary>
		/// adds NoteListObject test data
		/// </summary>
		/// <param name="resource"></param>
		private void AddNoteListObjectTestData (IViewModelResource resource)
		{
			using (IDbContext dbContext = resource.CreateDbContext()) {
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "8:00 AM"; c.Text = "make video"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:00 AM"; c.Text = "walk the dog"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:30 AM"; c.Text = "eat dinner"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "10:15 AM"; c.Text = "watch tv"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "11:30 AM"; c.Text = "jog"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "12:00 PM"; c.Text = "mop"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "1:00 PM"; c.Text = "kick it"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "2:45 PM"; c.Text = "throw things"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "7:00 PM"; c.Text = "sweep"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "7:30 PM"; c.Text = "chill"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "8:00 PM"; c.Text = "run"; }));
				resource.PrimeViewModel.AddNote(resource.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:00 PM"; c.Text = "also run"; }));
			}
		}

		/// <summary>
		/// adds GroupListObject test data
		/// </summary>
		/// <param name="resource"></param>
		private void AddGroupListObjectTestData (IViewModelResource resource)
		{
			using (IDbContext dbContext = resource.CreateDbContext()) {
				resource.GroupTabsViewModel.AddGroup(resource.ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext, c => { c.Title = "Chores"; }));
				resource.GroupTabsViewModel.AddGroup(resource.ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext, c => { c.Title = "Exercises"; }));
				resource.GroupTabsViewModel.AddGroup(resource.ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext, c => { c.Title = "Leisure"; }));
			}
		}

		#endregion
	}
}







// NOTE: the test data for GroupViewModel can stay
