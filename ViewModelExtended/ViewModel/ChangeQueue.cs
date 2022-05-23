﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	internal class ChangeQueue<T> : IChangeQueue<T> where T : IListItem
	{
		#region Container

		private ListItemDictionary f_Items;

		#endregion



		#region Dirty

		public bool IsDirty {
			get {
				return Any();
			}
		}

		#endregion



		#region Constructor

		public ChangeQueue (IViewModelResource resource)
		{
			f_Items = new ListItemDictionary(resource);
		}

		public ChangeQueue (IViewModelResource resource, Dictionary<IListItem, int> list)
		{
			f_Items = new ListItemDictionary(resource, list);
		}

		#endregion




		#region IChangeQueue - List Access

		public void QueueOnAdd (T input)
		{
			f_Items.Add(input);
		}

		public void QueueOnInsert (IListItem? target, T input)
		{
			f_Items.Add(input);
			if (target != null && !f_Items.ContainsKey(target))
				f_Items.Add(target);
		}

		public void QueueOnReorder (IListItem source, IListItem target)
		{
			if (!f_Items.ContainsKey(source)) f_Items.Add(source);
			if (!f_Items.ContainsKey(target)) f_Items.Add(target);
		}

		public void QueueOnRemove (IListItem input)
		{
			if (input.Previous != null && !f_Items.ContainsKey(input.Previous))
				f_Items.Add(input.Previous);

			if (input.Next != null && !f_Items.ContainsKey(input.Next))
				f_Items.Add(input.Next);

			if (f_Items.ContainsKey(input)) f_Items.Remove(input);
		}

		public void Remove (IListItem input)
		{
			f_Items.Remove(input);
		}

		public bool Any ()
		{
			return f_Items.Any();
		}

		public void Clear ()
		{
			f_Items.Clear();
		}

		#endregion



		#region IEnumerable

		public IEnumerator<KeyValuePair<IListItem, int>> GetEnumerator ()
		{
			return f_Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
