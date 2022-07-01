using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteDestroyCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteDestroyCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.DestroyNote((NoteListObjectViewModel)parameter);
		}
	}
}
