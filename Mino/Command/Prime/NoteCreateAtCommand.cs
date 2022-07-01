using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteCreateAtCommand : CommandBase
	{
		private readonly PrimeViewModel f_PrimeViewModel;

		public NoteCreateAtCommand (PrimeViewModel primeViewModel)
		{
			f_PrimeViewModel = primeViewModel;
		}

		public override void Execute (object parameter)
		{
			f_PrimeViewModel.CreateNoteAt((NoteListObjectViewModel)parameter);
		}
	}
}
