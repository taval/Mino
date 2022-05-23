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
		public Dictionary<Group, ListItemDictionary> Dictionaries { get; private set; }

		#endregion



		#region Current List

		public ListItemDictionary Items {
			get {
				if (f_Items == null) return f_DefaultDictionary;

				return f_Items;
			}
			set {
				f_Items = value;
			}
		}

		

		/// <summary>
		/// the default dictionary to return
		/// </summary>
		private ListItemDictionary f_DefaultDictionary;

		/// <summary>
		/// the dictionary creation mechanism
		/// </summary>
		//private readonly Func<Dictionary<IListItem, int>> f_DictionaryCreator;
		private IViewModelResource f_Resource;

		/// <summary>
		/// the stored reference to the selected dirty dictionary
		/// </summary>
		//private Dictionary<IListItem, int>? f_Items;
		private ListItemDictionary? f_Items;

		#endregion



		#region Constructor

		//public GroupChangeQueue (Func<Dictionary<IListItem, int>> dictionaryCreator)
		public GroupChangeQueue (IViewModelResource resource)
		{
			//f_DictionaryCreator = dictionaryCreator;
			f_Resource = resource;
			//f_DefaultDictionary = new ListItemDictionary(f_DictionaryCreator.Invoke());
			//f_DefaultDictionary = new ListItemDictionary(resource.ViewModelCreator.CreateDictionary());
			f_DefaultDictionary = new ListItemDictionary(resource);
			Dictionaries = new Dictionary<Group, ListItemDictionary>(new GroupEqualityComparer());
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
			Group groop = input.Model.Group;

			if (Dictionaries.ContainsKey(groop)) {
				ListItemDictionary dictionary = Dictionaries[groop];

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
			Group groop = input.Model.Group;

			if (Dictionaries.ContainsKey(groop)) {
				ListItemDictionary dictionary = Dictionaries[groop];

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
		/// <param name="groop"></param>
		public void QueueOnReorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			Group groop = source.Model.Group;

			if (source.Model.Group != target.Model.Group) return;

			if (Dictionaries.ContainsKey(groop)) {
				ListItemDictionary dictionary = Dictionaries[groop];

				if (!dictionary.ContainsKey(source)) dictionary.Add(source);
				if (!dictionary.ContainsKey(target)) dictionary.Add(target);
			}
		}

		/// <summary>
		/// remove the object from the dictionary
		/// </summary>
		/// <param name="input"></param>
		/// <param name="groop"></param>
		public void QueueOnRemove (GroupObjectViewModel input)
		{
			Group groop = input.Model.Group;

			if (Dictionaries.ContainsKey(groop)) {
				ListItemDictionary dictionary = Dictionaries[groop];

				if (input.Previous != null && !dictionary.ContainsKey(input.Previous))
					dictionary.Add(input.Previous);

				if (input.Next != null && !dictionary.ContainsKey(input.Next))
					dictionary.Add(input.Next);

				if (dictionary.ContainsKey(input)) dictionary.Remove(input);
			}
		}

		public bool IsDirty {
			get {
				foreach (KeyValuePair<Group, ListItemDictionary> dictionary in Dictionaries) {
					if (dictionary.Value.Any()) return true;
				}
				return false;
			}
		}

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ()
		{
			foreach (KeyValuePair<Group, ListItemDictionary> dictionary in Dictionaries) {
				dictionary.Value.Clear();
			}
		}

		/// <summary>
		/// gets the dictionary by using Group as key
		/// </summary>
		/// <param name="groop"></param>
		/// <returns>the GroupObject dictionary associated with the given Group key</returns>
		//public Dictionary<IListItem, int> GetListByGroupKey (Group? groop)
		public ListItemDictionary GetListByGroupKey (Group? groop)
		{
			if (groop == null) {
				return f_DefaultDictionary;
			}

			if (Dictionaries.ContainsKey(groop)) {
				return Dictionaries[groop];
			}
			else {
				//ListItemDictionary output = new ListItemDictionary(f_DictionaryCreator.Invoke());
				//ListItemDictionary output = new ListItemDictionary(f_Resource.ViewModelCreator.CreateDictionary());
				ListItemDictionary output = new ListItemDictionary(f_Resource);
				Dictionaries.Add(groop, output);

				return output;
			}
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

// TODO: make nearly identical to GroupContents class

