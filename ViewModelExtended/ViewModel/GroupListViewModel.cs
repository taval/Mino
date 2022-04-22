using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying the master list of all Groups
	/// </summary>
	public class GroupListViewModel : ViewModelBase, IObservableList
	{
		/// <summary>
		/// the base ListViewModel
		/// </summary>
		private ListViewModel ListViewModel { get; set; }

		public IEnumerable<IListItem> Items {
			get { return ListViewModel.Items; }
		}

		private IViewModelResource Resource { get; set; }



		#region Cross-View Data

		private IListItem? m_Highlighted;
		public IListItem? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
				ListViewModel.Highlighted = m_Highlighted;
			}
		}

		#endregion



		#region Commands

		public ICommand ReorderCommand {
			get { return ListViewModel.ReorderCommand ?? throw new NullReferenceException("command not assigned"); }
			set { if (ListViewModel.ReorderCommand == null) ListViewModel.ReorderCommand = value; }
		}

		public ICommand PreselectCommand {
			get { return ListViewModel.PreselectCommand ?? throw new NullReferenceException("command not assigned"); }
			set { if (ListViewModel.PreselectCommand == null) ListViewModel.PreselectCommand = value; }
		}

		#endregion



		#region Constructor

		public GroupListViewModel (IViewModelResource resource)
		{
			Resource = resource;
			ListViewModel = new ListViewModel(Resource);
			Resource.CommandBuilder.MakeGroupList(this);
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<IListItem> unsortedObjects = Resource.DbQueryHelper.GetAllGroupListObjects(dbContext);
				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects, ListViewModel);
			}
		}

		#endregion



		#region Access

		public void Add (IListItem item)
		{
			ListViewModel.Add(item);
		}

		public void Insert (IListItem? target, IListItem input)
		{
			ListViewModel.Insert(target, input);
		}

		public void Reorder (IListItem source, IListItem target)
		{
			ListViewModel.Reorder(source, target);
		}

		public void Remove (IListItem obj)
		{
			//using (IDbContext dbContext = Resource.CreateDbContext()) {
			//	dbContext.DeleteGroupListItem(dbContext.GroupListItems.Find(obj.ItemId)); // TODO: this should refer to object and not item
			//}
			ListViewModel.Remove(obj);
			Resource.ViewModelCreator.DestroyGroupListObjectViewModel((GroupListObjectViewModel)obj);
		}

		public int Index (IListItem input)
		{
			return ListViewModel.Index(input);
		}

		public void Clear ()
		{
			ListViewModel.Clear();
		}

		#endregion



		#region Create

		public GroupListObjectViewModel Create ()
		{
			return Resource.ViewModelCreator.CreateGroupListObjectViewModel();
		}

		#endregion



		#region Refresh

		public void Refresh ()
		{
			ListViewModel.RefreshListView();
		}

		#endregion
	}
}
