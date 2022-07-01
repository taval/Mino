using System;
using System.ComponentModel;



namespace Mino.ViewModel
{
	internal class GroupContentsComponentCreator : IGroupContentsComponentCreator
	{
		public GroupChangeQueue CreateGroupChangeQueue (Func<IListItemDictionary> dictionaryCreator)
		{
			return new GroupChangeQueue(dictionaryCreator);
		}

		public GroupContents CreateGroupContents (Func<IObservableList<GroupObjectViewModel>> listCreator)
		{
			return new GroupContents(listCreator);
		}

		public NoteDelegateDictionary CreateNoteDelegateDictionary ()
		{
			return new NoteDelegateDictionary((s, h) => CreateNoteHandler(s, h));
		}

		public NoteHandler CreateNoteHandler (NoteListObjectViewModel subject, PropertyChangedEventHandler handler)
		{
			return new NoteHandler(subject, handler);
		}
	}
}
