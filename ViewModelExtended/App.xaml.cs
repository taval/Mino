using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModelExtended.Model;
using ViewModelExtended.ViewModel;

// TODO: Allow groups, selected tab, selected NoteList/GroupList/GroupContents object are configuration state which should be preserved in db
//  UPDATE: save implemented for all but NoteTextViewModel's state; load not yet implemented

// TODO: make modules for view, vm, db, etc.

// TODO: most exceptions should trigger a rollback to the last known good state and shutdown/commit properly (exception safety)

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

			AddResource("TextBoxError");
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

			//context.Load();

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

		private string FlowDocumentWrap (string text)
		{
			return
$@"
<FlowDocument
	PagePadding=""5,0,5,0""
	AllowDrop=""True""
	NumberSubstitution.CultureSource=""User""
	xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">

	<Paragraph>
		{ text }
	</Paragraph>

</FlowDocument>
";
		}

		/// <summary>
		/// adds NoteListObject test data
		/// </summary>
		/// <param name="viewModelKit"></param>
		private void AddNoteListObjectTestData (IViewModelKit viewModelKit, IViewModelContext context)
		{
			using (IDbContext dbContext = viewModelKit.CreateDbContext()) {
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "8:00 AM"; c.Text = FlowDocumentWrap("make video"); c.Priority = 2; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:00 AM"; c.Text = FlowDocumentWrap("walk the dog"); c.Priority = 2; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:30 AM"; c.Text = FlowDocumentWrap("eat dinner"); c.Priority = 2; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "10:15 AM"; c.Text = FlowDocumentWrap("watch tv"); c.Priority = 0; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "11:30 AM"; c.Text = FlowDocumentWrap("jog"); c.Priority = 2; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "12:00 PM"; c.Text = FlowDocumentWrap("mop"); c.Priority = 1; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "1:00 PM"; c.Text = FlowDocumentWrap("kick it"); c.Priority = 0; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "2:45 PM"; c.Text = FlowDocumentWrap("throw things"); c.Priority = 2; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "7:00 PM"; c.Text = FlowDocumentWrap("sweep"); c.Priority = 1; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "7:30 PM"; c.Text = FlowDocumentWrap("chill"); c.Priority = 0; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "8:00 PM"; c.Text = FlowDocumentWrap("run"); c.Priority = 0; }));
				context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
					dbContext, c => { c.Title = "9:00 PM"; c.Text = FlowDocumentWrap("also run"); c.Priority = 2; }));

				context.NoteListViewModel.SaveListOrder();
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

				context.GroupListViewModel.SaveListOrder();
			}
		}

		#endregion
	}
}







// NOTE: the test data for GroupViewModel can stay
