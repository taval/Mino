using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModelExtended.Model;
using ViewModelExtended.ViewModel;

// TODO: the viewmodel is initialized before the view and so does not detect changes to the object prior to start. A separate OnLoad command could populate the data viewmodels. Or, the StatusBar could have a constructor that takes these values

// TODO: make modules for view, vm, db, etc.

// TODO: most exceptions should trigger a rollback to the last known good state and shutdown/commit properly

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
			IViewModelKit viewModelKit = new ViewModelKit();

			#endregion



			#region ViewModel Context

			IViewModelContext context = new ViewModelContext(viewModelKit.ViewModelCreator);

			#endregion



			#region Load Test Data

			//AddNoteListObjectTestData(viewModelKit, context);
			//AddGroupListObjectTestData(viewModelKit, context);

			#endregion



			#region Setup ViewModel

			context.Load();

			#endregion



			#region Load Main Window

			MainWindow = new MainWindow() { DataContext = context };
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
		/// <param name="viewModelKit"></param>
		private void AddNoteListObjectTestData (IViewModelKit viewModelKit, IViewModelContext context)
		{
			using (IDbContext dbContext = viewModelKit.CreateDbContext()) {
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "8:00 AM"; c.Text = "make video"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:00 AM"; c.Text = "walk the dog"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:30 AM"; c.Text = "eat dinner"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "10:15 AM"; c.Text = "watch tv"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "11:30 AM"; c.Text = "jog"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "12:00 PM"; c.Text = "mop"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "1:00 PM"; c.Text = "kick it"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "2:45 PM"; c.Text = "throw things"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "7:00 PM"; c.Text = "sweep"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "7:30 PM"; c.Text = "chill"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "8:00 PM"; c.Text = "run"; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:00 PM"; c.Text = "also run"; }));
			}
		}

		/// <summary>
		/// adds GroupListObject test data
		/// </summary>
		/// <param name="viewModelKit"></param>
		private void AddGroupListObjectTestData (IViewModelKit viewModelKit, IViewModelContext context)
		{
			using (IDbContext dbContext = viewModelKit.CreateDbContext()) {
				context.GroupTabsViewModel.AddGroup(viewModelKit.ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext, c => { c.Title = "Chores"; }));
				context.GroupTabsViewModel.AddGroup(viewModelKit.ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext, c => { c.Title = "Exercises"; }));
				context.GroupTabsViewModel.AddGroup(viewModelKit.ViewModelCreator.CreateGroupListObjectViewModel(
					dbContext, c => { c.Title = "Leisure"; }));
			}
		}

		#endregion
	}
}







// NOTE: the test data for GroupViewModel can stay
