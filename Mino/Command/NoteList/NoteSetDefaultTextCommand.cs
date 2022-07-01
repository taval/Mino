using Mino.ViewModel;



namespace Mino.Command
{
	public class NoteSetDefaultTextCommand : CommandBase
	{
		private readonly NoteListViewModel f_Context;

		public NoteSetDefaultTextCommand (NoteListViewModel context)
		{
			f_Context = context;
		}

		public override void Execute (object parameter)
		{
			f_Context.DefaultText = UIHelper.CreateEmptyFlowDocument();
		}
	}
}
