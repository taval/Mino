using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.Model
{
	/// <summary>
	/// wrapper of instantiated instances of an item and its supporting data
	/// </summary>
	public interface IObject
	{
		public Node Node { get; }
		public Timestamp Timestamp { get; }
	}
}
