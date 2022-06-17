using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Mino;
using Mino.Model;



namespace Mino.ViewModel
{
	/// <summary>
	/// the ViewModel representation of a list item
	/// </summary>
	public interface IListItem : IObject, INotifyPropertyChanged
	{
		public int ItemId { get; }

		public int DataId { get; }

		/// <summary>
		/// the IListItem's previous item
		/// </summary>
		public IListItem? Previous { get; set; }

		/// <summary>
		/// the IListItem's next item
		/// </summary>
		public IListItem? Next { get; set; }
	}

	public class ListDataEqualityComparer : IEqualityComparer<IListItem>
	{
		public bool Equals (IListItem? lhs, IListItem? rhs)
		{
			return lhs?.DataId == rhs?.DataId;
		}

		public int GetHashCode ([DisallowNull] IListItem obj)
		{
			//return obj.GetHashCode();
			return obj.DataId.GetHashCode() * 17;
		}
	}

	public class ListItemEqualityComparer : IEqualityComparer<IListItem>
	{
		public bool Equals (IListItem? lhs, IListItem? rhs)
		{
			return lhs?.ItemId == rhs?.ItemId;
		}

		public int GetHashCode ([DisallowNull] IListItem obj)
		{
			//return obj.GetHashCode();
			return obj.ItemId.GetHashCode() * 17;
		}
	}
}
