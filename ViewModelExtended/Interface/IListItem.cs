using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended;



namespace ViewModelExtended
{
	/// <summary>
	/// the ViewModel representation of a list item
	/// </summary>
	public interface IListItem
	{
		public int ItemId { get; }

		public INode Node { get; }

		/// <summary>
		/// the IListItem's previous item
		/// </summary>
		public IListItem? Previous { get; set; }

		public int? PreviousId { get; }

		/// <summary>
		/// the IListItem's next item
		/// </summary>
		public IListItem? Next { get; set; }

		public int? NextId { get; }

		public IListItem Value { get; }
	}
}
