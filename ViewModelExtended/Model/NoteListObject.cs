using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.Model
{
	public class NoteListObject : IObject
	{
		private IObject Root { get; set; }



		#region IObject interface

		public INode Node {
			get { return Root.Node; }
		}

		public Timestamp Timestamp {
			get { return Root.Timestamp; }
		}

		#endregion



		#region Instance

		public NoteListItem Item { get; private set; }
		public Note Data { get; private set; }

		#endregion



		#region Constructor

		public NoteListObject (NoteListItem item, IObject root, Note data)
		{
			Root = root;
			Item = item;
			Data = data;
		}

		#endregion
	}
}
