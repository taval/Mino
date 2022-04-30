using ViewModelExtended.Command;
using ViewModelExtended.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ViewModelExtended
{
	
	public class CommandBuilder : ICommandBuilder
	{
		private IViewModelResource Resource { get; set; }

		public CommandBuilder (IViewModelResource resource)
		{
			Resource = resource;
		}

		public void MakeGroup (GroupContentsViewModel target)
		{
			target.NoteReceiveCommand = new NoteReceiveCommand(target);

			target.ReorderCommand = new GroupNoteReorderCommand(target);
			//target.RemoveCommand = new GroupNoteRemoveCommand(target); // TODO: don't think this is used anymore, also, does this one need RefreshCommand?
			//target.PreselectCommand = new GroupNotePreselectCommand(target);
			target.PickupCommand = new GroupNotePickupCommand(target);
		}

		public void MakeGroupList (GroupListViewModel target)
		{
			target.ReorderCommand = new GroupReorderCommand(target);
			//target.RefreshCommand = new GroupRefreshCommand(target);
			//target.PreselectCommand = new GroupPreselectCommand(target);
			target.PickupCommand = new GroupPickupCommand(target);
		}

		public void MakeGroupTabs (GroupTabsViewModel target)
		{
			//target.GroupTabsLoadCommand = new GroupTabsLoadCommand(target);

			target.GroupSelectCommand = new GroupSelectCommand(target);
			target.GroupCreateCommand = new GroupCreateCommand(target);
			target.GroupDestroyCommand = new GroupDestroyCommand(target);

			target.GroupNoteSelectCommand = new GroupNoteSelectCommand(target);
			target.GroupNoteDestroyCommand = new GroupNoteDestroyCommand(target);
		}

		public void MakeNoteList (NoteListViewModel target)
		{
			target.PickupCommand = new NotePickupCommand(target);

			target.ReorderCommand = new NoteReorderCommand(target);
			//target.RefreshCommand = new NoteRefreshCommand(target);
			//target.PreselectCommand = new NotePreselectCommand(target);
		}

		public void MakePrime (PrimeViewModel target)
		{
			//target.PrimeLoadCommand = new PrimeLoadCommand(target);

			target.NoteSelectCommand = new NoteSelectCommand(target);
			target.NoteCreateCommand = new NoteCreateCommand(target);
			target.NoteDestroyCommand = new NoteDestroyCommand(target);

			
			target.GroupNoteHoldCommand = new GroupNoteHoldCommand(target);
			target.GroupNoteDropCommand = new GroupNoteDropCommand(target);
		}

		public void MakeMainWindow (MainWindowViewModel target)
		{
			target.CloseCommand = new MainWindowCloseCommand(target);
		}
	}
}
