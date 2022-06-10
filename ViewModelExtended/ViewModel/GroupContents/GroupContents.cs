using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// container of contents lists which presents a single list at a time
	/// </summary>
	internal class GroupContents : IEnumerable<GroupObjectViewModel>
	{
		#region Lists Container

		/// <summary>
		/// A dictionary of GroupContents lists
		/// </summary>
		public Dictionary<Group, IObservableList<GroupObjectViewModel>> Lists { get; private set; }

		#endregion



		#region Current List

		/// <summary>
		/// the interface to the selected contents list
		/// </summary>
		public IObservableList<GroupObjectViewModel> List {
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
		/// the stored reference to the selected observable contents list
		/// </summary>
		private IObservableList<GroupObjectViewModel>? f_List;

		/// <summary>
		/// the default list to return
		/// </summary>
		private IObservableList<GroupObjectViewModel> f_DefaultList;

		/// <summary>
		/// the list creation mechanism
		/// </summary>
		private readonly Func<IObservableList<GroupObjectViewModel>> f_ListCreator;

		/// <summary>
		/// the public enumerable
		/// </summary>
		public IEnumerable<GroupObjectViewModel> Items {
			get { return List.Items; }
		}

		/// <summary>
		/// the number of items in the selected container
		/// </summary>
		public int ItemCount {
			get { return Items.Count(); }
		}

		#endregion



		#region Constructor

		public GroupContents (Func<IObservableList<GroupObjectViewModel>> listCreator)
		{
			f_ListCreator = listCreator;
			f_DefaultList = f_ListCreator.Invoke();
			Lists = new Dictionary<Group, IObservableList<GroupObjectViewModel>>(new GroupEqualityComparer());
			f_List = null;
		}

		#endregion



		#region List Access

		/// <summary>
		/// add an object to the end of the list
		/// </summary>
		/// <param name="input"></param>
		public void Add (GroupObjectViewModel input)
		{
			Group groop = input.Model.Group;

			if (Lists.ContainsKey(groop)) {
				Lists[groop].Add(input);
			}
		}

		/// <summary>
		/// insert an object into the list at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (GroupObjectViewModel? target, GroupObjectViewModel input)
		{
			Group groop = input.Model.Group;

			if (Lists.ContainsKey(groop)) {
				Lists[groop].Insert(target, input);
			}
		}

		/// <summary>
		/// rearrange two objects in the list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			Group groop = source.Model.Group;

			if (source.Model.Group != target.Model.Group) return;

			if (Lists.ContainsKey(groop)) {
				Lists[groop].Reorder(source, target);
			}
		}

		/// <summary>
		/// remove the object from the list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (GroupObjectViewModel input)
		{
			Group groop = input.Model.Group;

			if (Lists.ContainsKey(groop)) {
				Lists[groop].Remove(input);
			}
		}

		/// <summary>
		/// return the position of the given input
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public int Index (GroupObjectViewModel input)
		{
			if (f_List == null) return -1;

			return List.Index(input);
		}

		public GroupObjectViewModel Find (Func<GroupObjectViewModel, bool> predicate)
		{
			return List.Find(predicate);
		}

		/// <summary>
		/// empty all lists non-destructively
		/// </summary>
		public void Clear ()
		{
			foreach (KeyValuePair<Group, IObservableList<GroupObjectViewModel>> list in Lists) {
				list.Value.Clear();
			}
		}

		/// <summary>
		/// gets the list by using Group as key
		/// </summary>
		/// <param name="groop"></param>
		/// <returns>the GroupObject list associated with the given Group key</returns>
		public IObservableList<GroupObjectViewModel> GetListByGroupKey (Group? groop)
		{
			if (groop == null) {
				return f_DefaultList;
			}

			if (Lists.ContainsKey(groop)) {
				return Lists[groop];
			}
			else {
				IObservableList<GroupObjectViewModel> list = f_ListCreator.Invoke();

				Lists.Add(groop, list);

				return list;
			}
		}

		/// <summary>
		/// if any items exist, return true, else return false
		/// </summary>
		/// <returns></returns>
		public bool Any ()
		{
			foreach (KeyValuePair<Group, IObservableList<GroupObjectViewModel>> item in Lists) {
				if (item.Value.Any()) return true;
			}

			return false;
		}

		public bool HasNoteInGroup (Group groop, Note note)
		{
			IObservableList<GroupObjectViewModel> list = GetListByGroupKey(groop);
			return list.Where((groupNote) => groupNote.DataId == note.Id).Any();
		}

		#endregion



		#region IEnumerable

		public IEnumerator<GroupObjectViewModel> GetEnumerator ()
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
