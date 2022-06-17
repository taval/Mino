using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mino.ViewModel
{
	internal class NoteHandler
	{
		public NoteListObjectViewModel Subject { get; private set; }
		public PropertyChangedEventHandler Handler { get; private set; }

		public NoteHandler (NoteListObjectViewModel subject, PropertyChangedEventHandler handler)
		{
			Subject = subject;
			Handler = handler;
		}
	}
}
