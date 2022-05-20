using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;

namespace ViewModelExtended.ViewModel
{
	internal class GroupChangeQueue : IChangeQueue<GroupObjectViewModel>
	{
		#region Lists Container

		/// <summary>
		/// save the dirty state for storing at shutdown, autosave intervals, etc.
		/// </summary>
		public Dictionary<Group, Dictionary<IListItem, int>> Lists { get; private set; }

		#endregion



		#region Current List

		/// <summary>
		/// the interface to the selected dirty list
		/// </summary>
		//public Dictionary<IListItem, int> List {
		//	get {
		//		if (f_List == null) return f_DefaultList;

		//		return f_List;
		//	}
		//	set {
		//		f_List = value;
		//	}
		//}
		public Dictionary<IListItem, int> List {
			get {
				// provide a dummy list if none available
				if (f_List == null) return f_DefaultList;

				return f_List;
			}
			set {
				f_List = value;
			}
		}

		/// <summary>
		/// the default list to return
		/// </summary>
		private Dictionary<IListItem, int> f_DefaultList;

		/// <summary>
		/// the list creation mechanism
		/// </summary>
		private readonly Func<Dictionary<IListItem, int>> f_ListCreator;

		/// <summary>
		/// the stored reference to the selected dirty list
		/// </summary>
		private Dictionary<IListItem, int>? f_List;

		#endregion



		#region Constructor

		public GroupChangeQueue (Func<Dictionary<IListItem, int>> listCreator)
		{
			f_ListCreator = listCreator;
			f_DefaultList = f_ListCreator.Invoke();
			Lists = new Dictionary<Group, Dictionary<IListItem, int>>();
			f_List = null;
		}

		#endregion




		#region List Access

		/// <summary>
		/// add an object to the end of the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnAdd (GroupObjectViewModel input)
		{
			if (f_List == null) return;

			List.Add(input, List.Count());
		}

		/// <summary>
		/// insert an object into the CURRENTLY VISIBLE list at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void QueueOnInsert (IListItem? target, GroupObjectViewModel input)
		{
			if (f_List == null) return;

			List.Add(input, List.Count());
			if (target != null && !List.ContainsKey(target))
				List.Add(target, List.Count());
		}

		/// <summary>
		/// rearrange two objects in the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void QueueOnReorder (IListItem source, IListItem target)
		{
			if (f_List == null) return;

			if (!List.ContainsKey(source)) List.Add(source, List.Count());
			if (!List.ContainsKey(target)) List.Add(target, List.Count());
		}


		/// <summary>
		/// remove the object from the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="input"></param>
		public void QueueOnRemove (IListItem input)
		{
			if (f_List == null) return;

			if (input.Previous != null && !List.ContainsKey(input.Previous))
				List.Add(input.Previous, List.Count());

			if (input.Next != null && !List.ContainsKey(input.Next))
				List.Add(input.Next, List.Count());

			if (List.ContainsKey(input)) List.Remove(input);
		}

		public bool IsDirty {
			get {
				foreach (KeyValuePair<Group, Dictionary<IListItem, int>> list in Lists) {
					if (list.Value.Count > 0) return false;
				}
				return true;
			}
		}

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ()
		{
			foreach (KeyValuePair<Group, Dictionary<IListItem, int>> list in Lists) {
				list.Value.Clear();
			}
		}

		/// <summary>
		/// gets the list by using Group as key
		/// </summary>
		/// <param name="groop"></param>
		/// <returns>the GroupObject list associated with the given Group key</returns>
		public Dictionary<IListItem, int> GetListByGroupKey (Group? groop)
		{
			if (groop == null) {
				f_List = null;
				return List;
			}

			IEnumerable<KeyValuePair<Group, Dictionary<IListItem, int>>> selectedList =
				Lists.Where((kv) => kv.Key.Id == groop.Id);

			if (selectedList.Any()) {
				//List = selectedList.Single().Value;
				return selectedList.Single().Value;
			}
			else {
				Dictionary<IListItem, int> list = f_ListCreator.Invoke();

				Lists.Add(groop, list);
				//List = list;
				return list;
			}

			//return List;
		}

		#endregion
	}
}

// TODO: make nearly identical to GroupContents class