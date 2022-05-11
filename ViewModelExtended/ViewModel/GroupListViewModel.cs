using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: zero groups should gray out the Contents tab. An existing group should colorize it.

// TODO: double-clicking the group's title should only select an item upon the first double-click. Subsequent double-clicks should cause it to edit the text.

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
		private IObservableList<GroupListObjectViewModel> List { get; set; }

		public IEnumerable<GroupListObjectViewModel> Items {
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

		public ICommand PickupCommand {
			get { return m_PickupCommand ?? throw new MissingCommandException(); }
			set { if (m_PickupCommand == null) m_PickupCommand = value; }
		}

		private ICommand? m_PickupCommand;

		public ICommand ChangeTitleCommand {
			get { return m_ChangeTitleCommand ?? throw new MissingCommandException(); }
			set { if (m_ChangeTitleCommand == null) m_ChangeTitleCommand = value; }
		}

		private ICommand? m_ChangeTitleCommand;

		public ICommand ChangeColorCommand {
			get { return m_ChangeColorCommand ?? throw new MissingCommandException(); }
			set { if (m_ChangeColorCommand == null) m_ChangeColorCommand = value; }
		}

		private ICommand? m_ChangeColorCommand;

		public ICommand HighlightCommand {
			get { return m_HighlightCommand ?? throw new MissingCommandException(); }
			set { if (m_HighlightCommand == null) m_HighlightCommand = value; }
		}

		private ICommand? m_HighlightCommand;

		#endregion



		#region Constructor

		public GroupListViewModel (IViewModelResource resource)
		{
			Resource = resource;
			Resource.CommandBuilder.MakeGroupList(this);
			m_Highlighted = null;
			List = Resource.ViewModelCreator.CreateList<GroupListObjectViewModel>();

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IQueryable<GroupListObjectViewModel> unsortedObjects = Resource.DbQueryHelper.GetAllGroupListObjects(dbContext);
				Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects.ToList(), List);
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
			RemoveAllEventHandlers();
			List.Clear();
		}

		#endregion



		#region Query

		public GroupListObjectViewModel Find (Func<GroupListObjectViewModel, bool> predicate)
		{
			return List.Find(predicate);
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



		#region Update

		public void UpdateTitle ()
		{
			if (m_Highlighted == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.UpdateGroup(m_Highlighted.Model.Data, m_Highlighted.Title, null);
				dbContext.Save();
			}
		}

		public void UpdateColor ()
		{
			if (m_Highlighted == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				dbContext.UpdateGroup(m_Highlighted.Model.Data, m_Highlighted.Color, null);
				dbContext.Save();
			}
		}

		#endregion
	}
}
