using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended
{
	/// <summary>
	/// the Model representation of a list item
	/// </summary>
	public interface INode : IIdentifiable
	{
		public int? PreviousId { get; set; }
		public int? NextId { get; set; }
	}
}
