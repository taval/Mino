using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.Model
{
	public class GroupListObject : IObject
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

		public GroupListItem Item { get; private set; }
		public Group Data { get; private set; }

		#endregion



		#region Constructor

		public GroupListObject (GroupListItem item, IObject root, Group data)
		{
			Root = root;
			Item = item;
			Data = data;
		}

		#endregion
	}
}
