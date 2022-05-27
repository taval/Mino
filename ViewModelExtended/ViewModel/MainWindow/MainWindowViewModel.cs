using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModelExtended.Command;
using ViewModelExtended.ViewModel;

namespace ViewModelExtended.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		#region Prime ViewModel

		public IViewModelResource Resource { get; private set; }

		#endregion

		public ICommand CloseCommand { get; set; }

		#region Constructor

		public MainWindowViewModel (IViewModelResource resource)
		{
			Resource = resource;
			Resource.CommandBuilder.MakeMainWindow(this);
		}

		#endregion
	}
}
