using Mino.Model;
using Mino.ViewModel;
using System;

namespace Mino
{
	/// <summary>
	/// helper class for test input generation methods
	/// </summary>
	internal static class AppTestData
	{
		private static string FlowDocumentWrap (string text)
		{
			return
$@"
<FlowDocument
	PagePadding=""5,0,5,0""
	AllowDrop=""True""
	NumberSubstitution.CultureSource=""User""
	xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">

	<Paragraph>
		{text}
	</Paragraph>

</FlowDocument>
";
		}

		public static void AddLotsOfNoteListObjects (IViewModelKit viewModelKit, IViewModelContext context)
		{
			using (IDbContext dbContext = viewModelKit.CreateDbContext()) {
				Random rnd = new Random();

				string lorem =
						@"    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

				string flowDoc = FlowDocumentWrap(lorem);

				for (int i = 0; i < 1000; i++) {
					context.PrimeViewModel.AddNote(viewModelKit.ViewModelCreator.CreateNoteListObjectViewModel(
						dbContext, c => {
							c.Title = $"Title { i }";
							c.Text = flowDoc;
							c.Priority = rnd.Next(0, 3);
						}
					));
				}

				context.NoteListViewModel.SaveListOrder();
			}
		}

		/// <summary>
		/// adds NoteListObject test data
		/// </summary>
		/// <param name="viewModelKit"></param>
		public static void AddNoteListObjects (IViewModelKit viewModelKit, IViewModelContext context)
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
		public static void AddGroupListObjects (IViewModelKit viewModelKit, IViewModelContext context)
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
	}
}
