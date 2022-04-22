using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	public interface IObservableList
	{
		/// <summary>
		/// the enumerable list of items
		/// </summary>
		public IEnumerable<IListItem> Items { get; }

		/// <summary>
		/// add a new input to the end of the collection
		/// </summary>
		/// <param name="item"></param>
		public void Add (IListItem item);

		/// <summary>
		/// insert a new input at the target's position or default position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (IListItem? target, IListItem input);

		/// <summary>
		/// place the existing source at the target's position
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (IListItem source, IListItem target);

		/// <summary>
		/// removes target object from list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (IListItem input);

		/// <summary>
		/// the position of the given item, if exists within the container
		/// </summary>
		/// <param name="input"></param>
		/// <returns>the integer index starting with position zero</returns>
		public int Index (IListItem input);

		/// <summary>
		/// empty the list
		/// </summary>
		public void Clear ();
	}
}
