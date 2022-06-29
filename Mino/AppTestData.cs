using Mino.Model;
using Mino.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;



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
