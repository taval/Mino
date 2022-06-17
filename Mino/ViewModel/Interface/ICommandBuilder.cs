using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Mino.ViewModel
{
	public interface ICommandBuilder
	{
		public void MakeMainWindow (MainWindowViewModel target);

		public void MakePrime (PrimeViewModel target);
		public void MakeGroupTabs (GroupTabsViewModel target);

		public void MakeNoteText (NoteTextViewModel target);

		public void MakeGroup (GroupContentsViewModel target);
		public void MakeGroupList (GroupListViewModel target);
		public void MakeNoteList (NoteListViewModel target);
	}
}
