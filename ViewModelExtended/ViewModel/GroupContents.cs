using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// container of contents lists which presents a single list at a time
	/// </summary>
	internal class GroupContents
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
		//public IObservableList<GroupObjectViewModel> List {
		//	get {
		//		// provide a dummy list if none available
		//		if (f_List == null) return f_DefaultList;

		//		return f_List;
		//	}
		//	set {
		//		f_List = value;
		//		ItemCount = (f_List != null) ? f_List.Items.Count() : 0;
		//	}
		//}
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
		/// the number of items in the container
		/// </summary>
		public int ItemCount {
			get { return List.Items.Count(); }
		}

		#endregion



		#region Constructor

		public GroupContents (Func<IObservableList<GroupObjectViewModel>> listCreator)
		{
			f_ListCreator = listCreator;
			f_DefaultList = f_ListCreator.Invoke();
			Lists = new Dictionary<Group, IObservableList<GroupObjectViewModel>>();
			f_List = null;
		}

		#endregion




		#region List Access

		/// <summary>
		/// add an object to the end of the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="input"></param>
		public void Add (GroupObjectViewModel input)
		{
			if (f_List == null) return;

			List.Add(input);
		}

		/// <summary>
		/// insert an object into the CURRENTLY VISIBLE list at the target's position
		/// </summary>
		/// <param name="target"></param>
		/// <param name="input"></param>
		public void Insert (GroupObjectViewModel? target, GroupObjectViewModel input)
		{
			if (f_List == null) return;

			List.Insert(target, input);
		}

		/// <summary>
		/// rearrange two objects in the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public void Reorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			if (f_List == null) return;

			List.Reorder(source, target);
		}


		/// <summary>
		/// remove the object from the CURRENTLY VISIBLE list
		/// </summary>
		/// <param name="input"></param>
		public void Remove (GroupObjectViewModel input)
		{
			if (f_List == null) return;

			List.Remove(input);
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
				//f_List = null;
				//return List;
				return f_DefaultList;
			}

			IEnumerable<KeyValuePair<Group, IObservableList<GroupObjectViewModel>>> selectedList =
				Lists.Where((kv) => kv.Key.Id == groop.Id);

			if (selectedList.Any()) {
				//List = selectedList.Single().Value;
				return selectedList.Single().Value;
			}
			else {
				IObservableList<GroupObjectViewModel> list = f_ListCreator.Invoke();

				Lists.Add(groop, list);
				//List = list;
				return list;
			}

			//return List;
		}

		#endregion
	}
}
