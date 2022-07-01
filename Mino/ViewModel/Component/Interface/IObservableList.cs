using System;
using System.Collections.Generic;

// TODO/NOTE: implementing IObservable as an IEnumerable fails miserably, breaking everything. A problem with ObservableCollection, perhaps? since custom implementation of GetEnumerator has the same problem, would seem unlikely.

namespace Mino.ViewModel
{
	internal interface IObservableList<T> where T : IListItem
	{
		/// <summary>
		/// the enumerable list of items
		/// </summary>
		public IEnumerable<T> Items { get; }

		/// <summary>
		/// joins a range of list items with node data already set
		/// incoming node data is left as-is
		/// existing tail is joined with incoming node data
		/// </summary>
		/// <param name="source"></param>
		public void AddSortedRange (IEnumerable<T> source);

		/// <summary>
		/// joins a set of list items at end of existing list with unset node data
		/// ignores and overwrites incoming node data
		/// existing tail is joined with incoming node data
		/// </summary>
		/// <param name="source"></param>
		public void AddRange (IEnumerable<T> source);

		/// <summary>
		/// add a new input to the end of the collection
		/// </summary>
		/// <param name="item"></param>
		public void Add (T item);

		/// <summary>
		/// insert a new input at the target's position or default position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (IListItem? target, T input);

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

		public T Find (Func<T, bool> predicate);

		public bool Any ();

		public IEnumerable<T> Where (Func<T, bool> predicate);
	}
}
