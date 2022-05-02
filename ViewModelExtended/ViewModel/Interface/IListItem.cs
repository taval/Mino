using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// the ViewModel representation of a list item
	/// </summary>
	public interface IListItem : IObject
	{
		public int ItemId { get; }

		//public INode Node { get; } // TODO: deprecate

		/// <summary>
		/// the IListItem's previous item
		/// </summary>
		public IListItem? Previous { get; set; }

		//public int? PreviousId { get; } // TODO: deprecate

		/// <summary>
		/// the IListItem's next item
		/// </summary>
		public IListItem? Next { get; set; }

		//public int? NextId { get; } // TODO: deprecate

		//public IListItem Value { get; } // TODO: deprecate
	}
}
