using Mino.Command;
using Mino.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Mino
{
	
	public class CommandBuilder : ICommandBuilder
	{
		public void MakeGroup (GroupContentsViewModel target)
		{
			target.NoteReceiveCommand = new NoteReceiveCommand(target);

			target.ReorderCommand = new GroupNoteReorderCommand(target);
			target.PickupCommand = new GroupNotePickupCommand(target);
		}

		public void MakeGroupList (GroupListViewModel target)
		{
			target.ReorderCommand = new GroupReorderCommand(target);
			target.PickupCommand = new GroupPickupCommand(target);

			target.HighlightCommand = new GroupHighlightCommand(target);
		}

		public void MakeGroupTabs (GroupTabsViewModel target)
		{
			target.GroupSelectCommand = new GroupSelectCommand(target);
			target.GroupCreateAtCommand = new GroupCreateAtCommand(target);
			target.GroupDestroyCommand = new GroupDestroyCommand(target);

			target.GroupNoteSelectCommand = new GroupNoteSelectCommand(target);
			target.GroupNoteDestroyCommand = new GroupNoteDestroyCommand(target);
		}

		public void MakeNoteList (NoteListViewModel target)
		{
			target.SetDefaultTextCommand = new NoteSetDefaultTextCommand(target);

			target.PickupCommand = new NotePickupCommand(target);
			target.ReorderCommand = new NoteReorderCommand(target);
		}

		public void MakePrime (PrimeViewModel target)
		{
			target.NoteSelectCommand = new NoteSelectCommand(target);
			target.NoteCreateAtCommand = new NoteCreateAtCommand(target);
			target.NoteDestroyCommand = new NoteDestroyCommand(target);

			target.GroupUpdateTitleCommand = new GroupUpdateTitleCommand(target);
			target.GroupUpdateColorCommand = new GroupUpdateColorCommand(target);

			target.GroupNoteHoldCommand = new GroupNoteHoldCommand(target);
			target.GroupNoteDropCommand = new GroupNoteDropCommand(target);

			target.NoteChangeGroupsCommand = new NoteChangeGroupsCommand(target);
		}

		public void MakeMainWindow (MainWindowViewModel target)
		{
			target.LoadCommand = new MainWindowLoadCommand(target);
			target.CloseCommand = new MainWindowCloseCommand(target);
		}

		public void MakeNoteText (NoteTextViewModel target)
		{
			target.UpdateTitleCommand = new NoteUpdateTitleCommand(target);
			target.ChangeTextCommand = new NoteChangeTextCommand(target);

			target.UpdateTextCommand = new NoteUpdateTextCommand(target);

			target.UpdatePriorityCommand = new NoteUpdatePriorityCommand(target);

			target.CalcCursorPosCommand = new CalcCursorPosCommand(target);

			target.LoadCommand = new NoteTextLoadCommand(target);
		}
	}
}
