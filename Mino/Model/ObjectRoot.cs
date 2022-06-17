using System;
using System.Collections.Generic;
using System.Text;



namespace Mino.Model
{
	public class ObjectRoot : IObject
	{
		public Node Node { get; private set; }
		public Timestamp Timestamp { get; private set; }

		public ObjectRoot (Node node, Timestamp timestamp)
		{
			Node = node;
			Timestamp = timestamp;
		}
	}
}
