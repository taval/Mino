using System;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mino.Model;
using Mino.ViewModel;

// TODO: make modules for view, vm, db, etc.

// TODO: most exceptions should trigger a rollback to the last known good state and shutdown/commit properly (exception safety)

/** TODO: RichTextBox data entry character rate feels sluggish.
 * Testing revealed bound commands had little visible impact.
 * Possibly related to https://github.com/dotnet/wpf/issues/3350
 * but adding configuration/post-InitializeComponent fix did not alleviate the problem
 */

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

			#endregion



			#region Load Resources

			AddResource("CloseButton");
			AddResource("Button");
			AddResource("WarnHighlight");
			//AddResource("LeftScrollViewer");
			//AddResource("ListView");
			AddResource("ListViewItem");

			AddResource("TextBoxError");
			IViewModelKit viewModelKit = new ViewModelKit(optionsBuilder.Options);

			#endregion



			#region ViewModel Context

			IViewModelContext context = new ViewModelContext(viewModelKit.ViewModelCreator);

			#endregion



			#region Load Test Data

			AppTestData.AddNoteListObjects(viewModelKit, context);
			AppTestData.AddGroupListObjects(viewModelKit, context);

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
