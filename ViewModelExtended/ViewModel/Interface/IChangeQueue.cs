using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	internal interface IChangeQueue<T> where T : IListItem
	{
		#region Container

		/// <summary>
		/// the interface to the selected dirty list
		/// </summary>
		public Dictionary<IListItem, int> List { get; }

		#endregion



		#region List Access

		/// <summary>
		/// add an object to the end of the list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnAdd (T input);

		/// <summary>
		/// insert an object into the list at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void QueueOnInsert (IListItem? target, T input);

		/// <summary>
		/// rearrange two objects in the list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void QueueOnReorder (IListItem source, IListItem target);


		/// <summary>
		/// remove the object from the list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnRemove (IListItem input);

		public bool IsDirty { get; }

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ();

		#endregion
	}
}
