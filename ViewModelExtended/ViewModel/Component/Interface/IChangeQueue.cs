using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	internal interface IChangeQueue<T> : IEnumerable<KeyValuePair<IListItem, int>> where T : IListItem
	{
		#region Dirty

		/// <summary>
		/// true if changes have been made
		/// </summary>
		public bool IsDirty { get; }

		#endregion

		#region List Access

		public IReadOnlyDictionary<IListItem, int> Items { get; }

		/// <summary>
		/// changes for when adding an object to the end of the list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnAdd (T input);

		/// <summary>
		/// changes for when inserting an object into the list at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void QueueOnInsert (IListItem? target, T input);

		/// <summary>
		/// changes for when rearranging two objects in the list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void QueueOnReorder (IListItem source, IListItem target);


		/// <summary>
		/// changes for when removing the object from the list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnRemove (IListItem input);

		/// <summary>
		/// remove change from queue
		/// </summary>
		/// <param name="target"></param>
		public void Remove (IListItem target);

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ();

		/// <summary>
		/// true if any objects exist in the dictionary
		/// </summary>
		/// <returns></returns>
		bool Any ();

		#endregion
	}
}
