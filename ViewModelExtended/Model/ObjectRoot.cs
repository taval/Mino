using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.Model
{
	public class ObjectRoot : IObject
	{
		public INode Node { get; private set; }
		public Timestamp Timestamp { get; private set; }

		public ObjectRoot (INode node, Timestamp timestamp)
		{
			Node = node;
			Timestamp = timestamp;
		}
	}
}
