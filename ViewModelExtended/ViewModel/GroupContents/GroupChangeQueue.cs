using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	internal class GroupChangeQueue : IEnumerable<KeyValuePair<IListItem, int>>
	{
		#region Lists Container

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		public Dictionary<Group, IListItemDictionary> Dictionaries { get; private set; }

		#endregion



		#region Default Dictionary

		/// <summary>
		/// the default dictionary to return
		/// </summary>
		private IListItemDictionary f_DefaultDictionary;

		#endregion



		#region Kit

		/// <summary>
		/// the dictionary creation mechanism
		/// </summary>
		private readonly Func<IListItemDictionary> f_DictionaryCreator;

		#endregion



		#region Current List

		public IListItemDictionary Items {
			get {
				if (f_Items == null) return f_DefaultDictionary;

				return f_Items;
			}
			set {
				f_Items = value;
			}
		}

		/// <summary>
		/// the stored reference to the selected dirty dictionary
		/// </summary>
		private IListItemDictionary? f_Items;

		#endregion



		#region Dirty

		public bool IsDirty {
			get { return Any(); }
		}

		#endregion



		#region Constructor

		public GroupChangeQueue (Func<IListItemDictionary> dictionaryCreator)
		{
			f_DictionaryCreator = dictionaryCreator;
			f_DefaultDictionary = dictionaryCreator.Invoke();
			Dictionaries = new Dictionary<Group, IListItemDictionary>(new GroupEqualityComparer());
			f_Items = null;
		}

		#endregion




		#region List Access

		/// <summary>
		/// add an object to the end of the dictionary
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnAdd (GroupObjectViewModel input)
		{
			Group groop = input.Group;

			if (Dictionaries.ContainsKey(groop)) {
				IListItemDictionary dictionary = Dictionaries[groop];

				dictionary.Add(input);
			}
		}

		/// <summary>
		/// insert an object into the dictionary at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void QueueOnInsert (GroupObjectViewModel? target, GroupObjectViewModel input)
		{
			Group groop = input.Group;

			if (Dictionaries.ContainsKey(groop)) {
				IListItemDictionary dictionary = Dictionaries[groop];

				dictionary.Add(input);
				if (target != null && !dictionary.ContainsKey(target))
					dictionary.Add(target);
			}
		}

		/// <summary>
		/// rearrange two objects in the dictionary selected by group
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void QueueOnReorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			Group groop = source.Group;

			if (source.Group != target.Group) return;

			if (Dictionaries.ContainsKey(groop)) {
				IListItemDictionary dictionary = Dictionaries[groop];

				if (!dictionary.ContainsKey(source)) dictionary.Add(source);
				if (!dictionary.ContainsKey(target)) dictionary.Add(target);
			}
		}

		/// <summary>
		/// remove the object from the dictionary
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnRemove (GroupObjectViewModel input)
		{
			Group groop = input.Group;

			if (Dictionaries.ContainsKey(groop)) {
				IListItemDictionary dictionary = Dictionaries[groop];

				if (input.Previous != null && !dictionary.ContainsKey(input.Previous))
					dictionary.Add(input.Previous);

				if (input.Next != null && !dictionary.ContainsKey(input.Next))
					dictionary.Add(input.Next);

				if (dictionary.ContainsKey(input)) dictionary.Remove(input);
			}
		}

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ()
		{
			foreach (KeyValuePair<Group, IListItemDictionary> dictionary in Dictionaries) {
				dictionary.Value.Clear();
			}
		}

		/// <summary>
		/// gets the dictionary by using Group as key
		/// </summary>
		/// <param name="groop"></param>
		/// <returns>the GroupObject dictionary associated with the given Group key</returns>
		public IListItemDictionary GetListByGroupKey (Group? groop)
		{
			if (groop == null) {
				return f_DefaultDictionary;
			}

			if (Dictionaries.ContainsKey(groop)) {
				return Dictionaries[groop];
			}
			else {
				IListItemDictionary output = f_DictionaryCreator.Invoke();
				Dictionaries.Add(groop, output);

				return output;
			}
		}

		/// <summary>
		/// if any items exist, return true, else return false
		/// </summary>
		/// <returns></returns>
		public bool Any ()
		{
			foreach (KeyValuePair<Group, IListItemDictionary> item in Dictionaries) {
				if (item.Value.Any()) return true;
			}

			return false;
		}

		/// <summary>
		/// iterate over the sorted selected dictionary
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<IListItem, int>> GetEnumerator ()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
