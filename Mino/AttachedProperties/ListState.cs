using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace Mino
{
	public class ListState : DependencyObject
	{
		/// <summary>
		/// data for the text display
		/// NOTE: SelectedProperty implementation detail (for reference only)
		/// ----- subscribers: PrimeViewModel & NoteListViewModel - don't delete one or the other on accident!
		/// </summary>
		public static readonly DependencyProperty Selected = DependencyProperty.RegisterAttached(
			nameof(Selected), typeof(object), typeof(ListState), new FrameworkPropertyMetadata(
				defaultValue: null, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static object GetSelected (DependencyObject o) => (object)o.GetValue(Selected);
		public static void SetSelected (DependencyObject o, object val) => o.SetValue(Selected, val);

		public static readonly DependencyProperty SelectedTitle = DependencyProperty.RegisterAttached(
		nameof(SelectedTitle), typeof(string), typeof(ListState), new FrameworkPropertyMetadata(
		defaultValue: String.Empty, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static string GetSelectedTitle (DependencyObject o) => (string)o.GetValue(SelectedTitle);
		public static void SetSelectedTitle (DependencyObject o, string val) => o.SetValue(SelectedTitle, val);

		/// <summary>
		/// a notification that a (highlighted) item is ready for DragDrop operation
		/// </summary>
		public static readonly DependencyProperty IsDropReady = DependencyProperty.RegisterAttached(
		nameof(IsDropReady), typeof(bool), typeof(ListState), new FrameworkPropertyMetadata(
		defaultValue: false));
		public static bool GetIsDropReady (DependencyObject o) => (bool)o.GetValue(IsDropReady);
		public static void SetIsDropReady (DependencyObject o, bool val) => o.SetValue(IsDropReady, val);

		/// <summary>
		/// a newly inserted item
		/// </summary>
		public static readonly DependencyProperty Inserted = DependencyProperty.RegisterAttached(
			nameof(Inserted), typeof(object), typeof(ListState), new FrameworkPropertyMetadata(
				defaultValue: null, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static object GetInserted (DependencyObject o) => (object)o.GetValue(Inserted);
		public static void SetInserted (DependencyObject o, object val) => o.SetValue(Inserted, val);

		/// <summary>
		/// the currently highlighted item
		/// </summary>
		public static readonly DependencyProperty Highlighted = DependencyProperty.RegisterAttached(
			nameof(Highlighted), typeof(object), typeof(ListState), new FrameworkPropertyMetadata(
				defaultValue: null, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static object GetHighlighted (DependencyObject o) => (object)o.GetValue(Highlighted);
		public static void SetHighlighted (DependencyObject o, object val) => o.SetValue(Highlighted, val);

		/// <summary>
		/// the data to be moved
		/// </summary>
		public static readonly DependencyProperty Source = DependencyProperty.RegisterAttached(
			nameof(Source), typeof(object), typeof(ListState), new FrameworkPropertyMetadata(
				defaultValue: null, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static object GetSource (DependencyObject o) => (object)o.GetValue(Source);
		public static void SetSource (DependencyObject o, object val) => o.SetValue(Source, val);

		/// <summary>
		/// the object at the position to be moved to
		/// <summary>
		public static readonly DependencyProperty Target = DependencyProperty.RegisterAttached(
			nameof(Target), typeof(object), typeof(ListState), new FrameworkPropertyMetadata(
				defaultValue: null, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static object GetTarget (DependencyObject o) => (object)o.GetValue(Target);
		public static void SetTarget (DependencyObject o, object val) => o.SetValue(Target, val);

		/// </summary>
		/// an item to be removed
		/// <summary>
		public static readonly DependencyProperty Removed = DependencyProperty.RegisterAttached(
			nameof(Removed), typeof(object), typeof(ListState), new FrameworkPropertyMetadata(
				defaultValue: null, flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static object GetRemoved (DependencyObject o) => (object)o.GetValue(Removed);
		public static void SetRemoved (DependencyObject o, object val) => o.SetValue(Removed, val);
	}
}
