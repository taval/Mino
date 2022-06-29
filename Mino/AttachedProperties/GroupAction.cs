﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;



namespace Mino
{
	public class GroupAction : DependencyObject
	{
		public static readonly DependencyProperty UpdateTitleCommand = DependencyProperty.RegisterAttached(
			nameof(UpdateTitleCommand), typeof(ICommand), typeof(GroupAction), new PropertyMetadata(null));
		public static ICommand GetUpdateTitleCommand (DependencyObject o) => (ICommand)o.GetValue(UpdateTitleCommand);
		public static void SetUpdateTitleCommand (DependencyObject o, ICommand val) => o.SetValue(UpdateTitleCommand, val);

		public static readonly DependencyProperty UpdateColorCommand = DependencyProperty.RegisterAttached(
			nameof(UpdateColorCommand), typeof(ICommand), typeof(GroupAction), new PropertyMetadata(null));
		public static ICommand GetUpdateColorCommand (DependencyObject o) => (ICommand)o.GetValue(UpdateColorCommand);
		public static void SetUpdateColorCommand (DependencyObject o, ICommand val) => o.SetValue(UpdateColorCommand, val);
	}
}
