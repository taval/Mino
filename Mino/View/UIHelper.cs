using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;



namespace Mino
{
	public static class UIHelper
	{
		#region FindParent

		/// <summary>
		/// Finds a parent of a given item on the visual tree
		/// </summary>
		/// <typeparam name="T">the type of queried item</typeparam>
		/// <param name="child">a child of the queried item</param>
		/// <returns>The first parent item that matches the given type parameter</returns>
		public static T FindParent<T> (DependencyObject child)
			where T : DependencyObject
		{
			DependencyObject? output = FindParentOrNull<T>(child);

			if (output == null) {
				throw new NullReferenceException(
					$"No parent of type { typeof(T).FullName } of given child could be found");
			}

			return (T)output;
		}

		public static DependencyObject? FindParentOrNull<T> (DependencyObject child)
			where T : DependencyObject
		{
			// get parent item
			DependencyObject? parentObject = VisualTreeHelper.GetParent(child);

			// end of tree
			if (parentObject == null) return null;

			// check if parent matches search type
			if (!(parentObject is T)) {
				return FindParentOrNull<T>(parentObject);
			}
			return parentObject;
		}

		#endregion



		#region FindChild

		/// <summary>
		/// finds a child of a given item in the visual tree
		/// </summary>
		/// <param name="parent">a parent of the queried item</param>
		/// <typeparam name="T">the type of the queried item</typeparam>
		/// <param name="childName">x:Name or Name of child</param>
		/// <returns>the first parent item that matches the given type parameter or null if no results found</returns>
		public static T FindChild<T> (DependencyObject? parent, string childName)
			where T : DependencyObject
		{
			DependencyObject? output = FindChildOrNull<T>(parent, childName);

			if (output == null) {
				throw new NullReferenceException(
					$"No child of type { typeof(T).FullName } with name { childName } of given parent could be found");
			}

			return (T)output;
		}

		public static DependencyObject? FindChildOrNull<T> (DependencyObject? parent, string childName)
			where T : DependencyObject
		{
			if (parent == null) return null;

			DependencyObject? foundChild = null;
			int childCount = VisualTreeHelper.GetChildrenCount(parent);

			for (int i = 0; i < childCount; i++) {
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);

				if (!(child is T)) {
					foundChild = FindChildOrNull<T>(child, childName);

					// if child found, break so it is not overwritten
					if (foundChild != null) break;
				}
				else if (!string.IsNullOrEmpty(childName)) {
					FrameworkElement? element = child as FrameworkElement;

					if (element != null && element.Name == childName) {
						foundChild = child;

						break;
					}
				}
				else {
					foundChild = child;

					break;
				}
			}

			return foundChild;
		}

		#endregion



		#region GetChildOfType

		/// <summary>
		/// given a parent DependencyObject, find a child of a given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="depObj"></param>
		/// <returns>the first resulting match for the given type</returns>
		/// <exception cref="NullReferenceException"></exception>
		public static T FindChildOfType<T> (this DependencyObject parent)
			where T : DependencyObject
		{
			DependencyObject? output = FindChildOfTypeOrNull<T>(parent);

			if (output == null) {
				throw new NullReferenceException(
					$"No child of type {typeof(T).FullName} of given parent could be found");
			}

			return (T)output;
		}

		private static DependencyObject? FindChildOfTypeOrNull<T> (this DependencyObject parent)
			where T : DependencyObject
		{
			if (parent == null) return null;

			DependencyObject? result = null;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
				var child = VisualTreeHelper.GetChild(parent, i);

				result = (child as T) ?? FindChildOfTypeOrNull<T>(child);

				if (result != null) return result;
			}

			return result;
		}

		#endregion



		#region CreateEmptyFlowDocument

		/// <summary>
		/// return a serialized empty FlowDocument
		/// </summary>
		/// <returns></returns>
		public static string CreateEmptyFlowDocument ()
		{
			FlowDocument doc = new FlowDocument();

			// serialize XAML
			MemoryStream stream = new MemoryStream();

			XamlWriter.Save(doc, stream);

			return Encoding.UTF8.GetString(stream.ToArray());
		}

		#endregion



		#region ScrollListView

		/// <summary>
		/// given a ListView, create motion based on the vertical position when a DragEvent occurs
		/// </summary>
		/// <param name="e"></param>
		/// <param name="listView"></param>
		public static void ScrollListView (DragEventArgs e, ListView listView)
		{
			ScrollViewer? scrollViewer = FindChildOfTypeOrNull<ScrollViewer>(listView) as ScrollViewer;

			if (scrollViewer == null) return;

			double tolerance = 60;
			double verticalPos = e.GetPosition(listView).Y;
			double offset = 1;

			// if top of visible list, scroll up
			// if bottom of visible list, scroll down
			if (verticalPos < tolerance) {
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
			}
			else if (verticalPos > listView.ActualHeight - tolerance) {
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
			}
		}

		#endregion
	}
}
