using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;



namespace Mino
{
	public class TextAction : DependencyObject
	{
		/// <summary>
		/// add or remove groups attached to a particular note
		/// </summary>
		public static readonly DependencyProperty ChangeGroupsCommand = DependencyProperty.RegisterAttached(
			nameof(ChangeGroupsCommand), typeof(ICommand), typeof(TextAction), new PropertyMetadata(null));
		public static ICommand GetChangeGroupsCommand (DependencyObject o) => (ICommand)o.GetValue(ChangeGroupsCommand);
		public static void SetChangeGroupsCommand (DependencyObject o, ICommand val) => o.SetValue(ChangeGroupsCommand, val);
	}
}
