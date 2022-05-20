using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	internal class ChangeQueue<T> : IChangeQueue<T> where T : IListItem
	{
		#region Container

		/// <summary>
		/// the interface to the selected dirty list
		/// </summary>
		public Dictionary<IListItem, int> List { get; private set; }

		#endregion



		#region Constructor

		public ChangeQueue (Dictionary<IListItem, int> list)
		{
			List = list;
		}

		#endregion




		#region List Access

		/// <summary>
		/// add an object to the end of the list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnAdd (T input)
		{
			List.Add(input, List.Count());
		}

		/// <summary>
		/// insert an object into the list at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void QueueOnInsert (IListItem? target, T input)
		{
			List.Add(input, List.Count());
			if (target != null && !List.ContainsKey(target))
				List.Add(target, List.Count());
		}

		/// <summary>
		/// rearrange two objects in the list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void QueueOnReorder (IListItem source, IListItem target)
		{
			if (!List.ContainsKey(source)) List.Add(source, List.Count());
			if (!List.ContainsKey(target)) List.Add(target, List.Count());
		}


		/// <summary>
		/// remove the object from the list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnRemove (IListItem input)
		{
			if (input.Previous != null && !List.ContainsKey(input.Previous))
				List.Add(input.Previous, List.Count());

			if (input.Next != null && !List.ContainsKey(input.Next))
				List.Add(input.Next, List.Count());

			if (List.ContainsKey(input)) List.Remove(input);
		}

		public bool IsDirty { get { return List.Count > 0; } }

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ()
		{
			List.Clear();
		}

		#endregion
	}
}
