using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying the master list of all list items
	/// </summary>
	public class ListViewModel : IObservableList
	{
		/// <summary>
		/// the IListItem container
		/// </summary>
		public IEnumerable<IListItem> Items {
			get { return List.Items; }
		}

		private IObservableList List { get; set; }

		/// <summary>
		/// the ViewModel service resource
		/// </summary>
		private IViewModelResource Resource { get; set; }



		#region Cross-View Data

		/// <summary>
		/// the most recently highlighted list item
		/// </summary>
		public IListItem? Highlighted { get; set; }

		#endregion



		#region Commands

		public ICommand? ReorderCommand { get; set; }
		public ICommand? PreselectCommand { get; set; }

		#endregion



		#region Constructor

		public ListViewModel (IViewModelResource resource)
		{
			List = new ObservableList();
			Resource = resource;
			Highlighted = null;
			ReorderCommand = null;
			PreselectCommand = null;
		}

		#endregion



		// TODO: this is to trigger a NotifyPropertyChanged event for each obj in order for the index display to be updated. Displaying a massive amount of indices could be a drag. A mitigation would be to use a range (all adjacent nodes, for instance). In mean time, remove calls to this method and displaying of indices if it becomes a problem
		public void RefreshListView ()
		{
			foreach (IListItem obj in Items) {
				int? _ = obj.PreviousId;
				_ = obj.NextId;
			}
		}



		#region Access

		public void Insert (IListItem? target, IListItem input)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// insert into list
				List.Insert(target, input);

				// update node data
				Resource.DbHelper.UpdateNodes(dbContext, target);
				Resource.DbHelper.UpdateNodes(dbContext, input);
				dbContext.Save();
				
				// update all of list view items
				//RefreshListView();
			}
		}

		public void Reorder (IListItem source, IListItem target)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// reorder in list
				List.Reorder(source, target);

				// update node data
				Resource.DbHelper.UpdateNodes(dbContext, source);
				Resource.DbHelper.UpdateNodes(dbContext, target);
				dbContext.Save();

				// update all of list view items
				//RefreshListView();
			}
		}

		public void Remove (IListItem obj)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// remove data from database
				IListItem? previous = obj.Previous;
				IListItem? next = obj.Next;

				// remove ViewModel from list
				List.Remove(obj);

				// update node data
				Resource.DbHelper.UpdateNodes(dbContext, previous);
				Resource.DbHelper.UpdateNodes(dbContext, next);
				dbContext.Save();

				// update all of list view items
				//RefreshListView();
			}
		}

		public void Add (IListItem item)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// add to list
				List.Add(item);

				// update node data
				Resource.DbHelper.UpdateNodes(dbContext, item);
				Resource.DbHelper.UpdateNodes(dbContext, item.Previous);
				Resource.DbHelper.UpdateNodes(dbContext, item.Next);
				dbContext.Save();

				// update all of list view items
				//RefreshListView();
			}
		}

		public int Index (IListItem input)
		{
			return List.Index(input);
		}

		public void Clear ()
		{
			List.Clear();
			//RefreshListView();
		}

		#endregion
	}
}
