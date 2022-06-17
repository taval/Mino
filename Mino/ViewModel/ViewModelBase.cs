using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace Mino.ViewModel
{
	/// <summary>
	/// contains the necessary requirements for notifying the view of changes
	/// </summary>

	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		#region Constructor

		#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public ViewModelBase ()

		{
			
		}
		#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		#endregion

		#region Public Properties

		public virtual bool Set<T> (ref T field, T value, [CallerMemberName] string propertyName = "")
		{
			if (Equals(field, value))
				return false;
			field = value;
			NotifyPropertyChanged(propertyName);
			return true;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// to subscribe to this event for properties of a ViewModel:
		/// _vm.PropertyChanged += OnViewModelPropertyChanged;
		/// where OnViewModelPropertyChanged is the name of the particular handler for that event
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RemoveAllEventHandlers ()
		{
			foreach (Delegate d in PropertyChanged.GetInvocationList()) {
				PropertyChanged -= (PropertyChangedEventHandler)d;
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// This method is called by the Set accessor of each property.
		/// The CallerMemberName attribute that is applied to the optional propertyName
		/// parameter causes the property name of the caller to be substituted as an argument.
		/// </summary>
		/// <param name="propertyName"></param>
		protected void NotifyPropertyChanged ([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}



}
