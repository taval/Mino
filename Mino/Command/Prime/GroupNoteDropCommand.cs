using Mino.ViewModel;



namespace Mino.Command
{
	public class GroupNoteDropCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public GroupNoteDropCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.AddNoteToGroup();
		}
	}
}
