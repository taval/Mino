using System;
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

		private IListItemDictionary f_Items;

		#endregion

		public IReadOnlyDictionary<IListItem, int> Items {
			get {
				return f_Items.Items;
			}
		}

		#region Dirty

		public bool IsDirty {
			get { return Any(); }
		}

		#endregion



		#region Constructor

		public ChangeQueue (Func<IListItemDictionary> dictionaryCreator)
		{
			f_Items = dictionaryCreator.Invoke();
		}

		public ChangeQueue (
			Func<Dictionary<IListItem, int>, IListItemDictionary> dictionaryCreator,
			Dictionary<IListItem, int> dictionary)
		{
			f_Items = dictionaryCreator.Invoke(dictionary);
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
