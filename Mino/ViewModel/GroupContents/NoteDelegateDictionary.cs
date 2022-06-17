using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;



namespace Mino.ViewModel
{
	internal class NoteDelegateDictionary
	{
		Dictionary<int, NoteHandler> f_Delegates;
		private Func<NoteListObjectViewModel, PropertyChangedEventHandler, NoteHandler> f_HandlerCreator;

		public NoteDelegateDictionary (
			Func<NoteListObjectViewModel, PropertyChangedEventHandler, NoteHandler> handlerCreator)
		{
			f_Delegates = new Dictionary<int, NoteHandler>();
			f_HandlerCreator = handlerCreator;
		}

		public bool ContainsKey (int observerId)
		{
			return f_Delegates.ContainsKey(observerId);
		}

		public void Add (int observerId, NoteHandler handler)
		{
			f_Delegates.Add(observerId, handler);
		}

		public NoteHandler GetHandlerByObserverId (int id)
		{
			return f_Delegates[id];
		}

		public void Remove (int observerId)
		{
			f_Delegates.Remove(observerId);
		}

		public NoteHandler Create (NoteListObjectViewModel subject, PropertyChangedEventHandler handler)
		{
			return f_HandlerCreator.Invoke(subject, handler);
		}
	}
}
