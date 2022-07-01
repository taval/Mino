using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupNoteDestroyCommand : CommandBase
	{
		private readonly GroupTabsViewModel f_GroupTabsViewModel;

		public GroupNoteDestroyCommand (GroupTabsViewModel groupTabsViewModel)
		{
			f_GroupTabsViewModel = groupTabsViewModel;
		}

		public override void Execute (object parameter)
		{
			f_GroupTabsViewModel.DestroyGroupNote((GroupObjectViewModel)parameter);
		}
	}
}
