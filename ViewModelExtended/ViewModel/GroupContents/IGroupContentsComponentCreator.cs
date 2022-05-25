using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	internal interface IGroupContentsComponentCreator
	{
		public GroupContents CreateGroupContents (Func<IObservableList<GroupObjectViewModel>> listCreator);

		public GroupChangeQueue CreateGroupChangeQueue (Func<IListItemDictionary> dictionaryCreator);

		public NoteDelegateDictionary CreateNoteDelegateDictionary ();

		public NoteHandler CreateNoteHandler (NoteListObjectViewModel subject, PropertyChangedEventHandler handler);
	}
}
