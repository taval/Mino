using System;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mino.Model;
using Mino.ViewModel;

// NOTE: creation time is slow, possibly because the forcing of redundant lookups w/ linear over constant time when searching for associated models. This only ever comes into play when testing large data set inputs. If there is ever a reason for the user to generate a batch of populated notes at a time, then this might be a concern; otherwise leave it alone.

// TODO: make modules for view, vm, db, etc.

// TODO: most exceptions should trigger a rollback to the last known good state and shutdown/commit properly (exception safety)

/** TODO: RichTextBox data entry character rate feels sluggish.
 * Testing revealed bound commands had little visible impact.
 * Possibly related to https://github.com/dotnet/wpf/issues/3350
 * but adding configuration/post-InitializeComponent fix did not alleviate the problem
 */

// TODO: update or clear the cursor position when a note is selected

namespace Mino
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup (StartupEventArgs e)
		{
			#region Configuration

			// load configuration
			// NOTE: GetCurrentDirectory encompasses the build directory when we just want the project root for now
			string configFolder = Directory.GetCurrentDirectory().Split("\\bin")[0];

			var builder = new ConfigurationBuilder()
				.SetBasePath(configFolder)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			IConfiguration config = builder.Build();

			// set database options
			var dbFolder = Environment.SpecialFolder.LocalApplicationData;
			var dbPath = Environment.GetFolderPath(dbFolder);
			string dbFilePath = System.IO.Path.Join(dbPath, config.GetValue<string>("DbFileName"));

			DbContextOptionsBuilder<MinoDbContext> optionsBuilder = new DbContextOptionsBuilder<MinoDbContext>();

			optionsBuilder.UseSqlite($"Data Source={ dbFilePath }");

			// configure NoteListObjectViewModel: set Priority options
			string[] priorityTypes = config.GetSection("PriorityTypes").Get<string[]>();
			foreach (string priorityType in priorityTypes) NoteListObjectViewModel.PriorityTypes.Add(priorityType);

			#endregion



			#region Load View Resources

			AddResource("CloseButton");
			AddResource("Button");
			AddResource("WarnHighlight");
			AddResource("ListViewItem");
			AddResource("Status");
			//AddResource("LeftScrollViewer");

			#endregion



			#region ViewModel Context

			// load kit
			IViewModelKit viewModelKit = new ViewModelKit(optionsBuilder.Options);

			// create db if does not exist
			using (IDbContext dbContext = viewModelKit.CreateDbContext()) {
				dbContext.Migrate();
			}

			// load viewmodel
			IViewModelContext context = new ViewModelContext(viewModelKit.ViewModelCreator);

			#endregion



			#region Load Test Data

			//AppTestData.AddNoteListObjects(viewModelKit, context);
			//AppTestData.AddGroupListObjects(viewModelKit, context);
			//AppTestData.AddLotsOfNoteListObjects(viewModelKit, context);

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

		#endregion
	}
}
