using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: there should be allowed zero groups - zero groups would gray out the Contents tab

namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying the master list of all Groups
	/// </summary>
	public class GroupListViewModel : ViewModelBase
	{
		/// <summary>
		/// the base ListViewModel
		/// </summary>
		private IObservableList List { get; set; }

		public IEnumerable<IListItem> Items {
			get { return List.Items; }
		}

		private IViewModelResource Resource { get; set; }



		#region Cross-View Data

		public GroupListObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
			}
		}

		private GroupListObjectViewModel? m_Highlighted;

		#endregion



		#region Commands

		public ICommand ReorderCommand {
			get { return m_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (m_ReorderCommand == null) m_ReorderCommand = value; }
		}

		private ICommand? m_ReorderCommand;

		public ICommand PreselectCommand {
			get { return m_PreselectCommand ?? throw new MissingCommandException(); }
			set { if (m_PreselectCommand == null) m_PreselectCommand = value; }
		}

		private ICommand? m_PreselectCommand;

		#endregion



		#region Constructor

		public GroupListViewModel (IViewModelResource resource)
		{
			Resource = resource;
			Resource.CommandBuilder.MakeGroupList(this);
			m_Highlighted = null;
			List = Resource.ViewModelCreator.CreateList();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<IListItem> unsortedObjects = Resource.DbQueryHelper.GetAllGroupListObjects(dbContext);
				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects, List);
			}
		}

		#endregion



		#region Access

		public void Add (GroupListObjectViewModel input)
		{
			List.Add(input);

			// make changes to database
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterAdd(dbContext, input);
				dbContext.Save();
			}
		}

		public void Insert (GroupListObjectViewModel? target, GroupListObjectViewModel input)
		{
			List.Insert(target, input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterInsert(dbContext, target, input);
				dbContext.Save();
			}
		}

		public void Reorder (GroupListObjectViewModel source, GroupListObjectViewModel target)
		{
			List.Reorder(source, target);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterReorder(dbContext, source, target);
				dbContext.Save();
			}
		}

		public void Remove (GroupListObjectViewModel input)
		{
			List.Remove(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterRemove(dbContext, input);
				dbContext.Save();
				Resource.ViewModelCreator.DestroyGroupListObjectViewModel(dbContext, input);
			}
		}

		public int Index (GroupListObjectViewModel input)
		{
			return List.Index(input);
		}

		public void Clear ()
		{
			List.Clear();
		}

		#endregion



		#region Create

		public GroupListObjectViewModel Create ()
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				return Resource.ViewModelCreator.CreateGroupListObjectViewModel(dbContext);
			}
		}

		#endregion



		#region Refresh

		public void Refresh ()
		{
			Utility.RefreshListView(List.Items);
		}

		#endregion
	}
}
