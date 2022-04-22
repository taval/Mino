﻿using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.Model
{
	public class GroupObject : IObject
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

		public Group Group { get; private set; }
		public GroupItem Item { get; private set; }
		public Note Data { get; private set; }

		#endregion



		#region Constructor

		public GroupObject (GroupItem item, IObject root, Group groop, Note data)
		{
			Root = root;
			Item = item;
			Group = groop;
			Data = data;
		}

		#endregion
	}
}
