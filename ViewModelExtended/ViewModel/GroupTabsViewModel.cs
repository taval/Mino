using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupTabsViewModel : ViewModelBase
	{
		#region Cross-View Data

		/// <summary>
		/// the GroupViewModel (data displayed by GroupView)
		/// NOTE: this is bound for purpose of GroupView population operation
		/// </summary>
		private GroupListObjectViewModel? m_SelectedGroupViewModel = null;
		public GroupListObjectViewModel? SelectedGroupViewModel {
			get { return m_SelectedGroupViewModel; }
			set { Set(ref m_SelectedGroupViewModel, value); }
		}

		/// <summary>
		/// the NoteViewModel selected in the Group Contents tab
		/// </summary>
		private GroupObjectViewModel? m_SelectedGroupNoteViewModel = null;
		public GroupObjectViewModel? SelectedGroupNoteViewModel {
			get { return m_SelectedGroupNoteViewModel; }
			set { Set(ref m_SelectedGroupNoteViewModel, value); }
		}

		#endregion

		#region Resource Component

		public IViewModelResource Resource { get; private set; }

		#endregion

		#region GroupList Commands

		public ICommand GroupSelectCommand { get; set; }
		public ICommand GroupCreateCommand { get; set; }
		public ICommand GroupDestroyCommand { get; set; }

		#endregion

		#region Group Commands

		public ICommand NoteReceiveCommand { get; set; }

		public ICommand GroupNoteSelectCommand { get; set; }
		public ICommand GroupNoteDestroyCommand { get; set; }

		#endregion



		public GroupTabsViewModel (IViewModelResource resource)
		{
			GroupListObjectViewModel? firstItem = null;

			Resource = resource;

			if (Resource.GroupListViewModel.Items.Count() > 0) {
				firstItem = Resource.GroupListViewModel.Items.First() as GroupListObjectViewModel;
			}

			if (firstItem != null) {
				SelectGroup(firstItem);
			}

			Resource.CommandBuilder.MakeGroupTabs(this);
		}

		#region Events: GroupList

		/// <summary>
		/// set the Contents viewer to the selected group (this is generally the SelectedGroup passed in from GroupList to GroupTabs)
		/// </summary>
		/// <param name="group"></param>
		public void SelectGroup (GroupListObjectViewModel groop)
		{
			SelectedGroupViewModel = groop;
			SelectedGroupViewModel.IsSelected = true;

			Resource.GroupContentsViewModel.ContentData = null;
			Resource.GroupContentsViewModel.ContentData = groop;

			//using (IDbContext dbContext = Resource.CreateDbContext()) {
				// TODO: this can be done cleaner with a LINQ query
				//GroupListItem item = dbContext.GroupListItems.Find(groop.ItemId);
				//Resource.GroupContentsViewModel.SetGroup(dbContext.Groups.Find(item.ObjectId));
			//}
			Resource.GroupContentsViewModel.SetGroup(groop);
		}

		/// <summary>
		/// inserts a new group into the group list; selects the newly created group
		/// </summary>
		/// <param name="target">the location at where the item will be inserted</param>
		/// <param name="input">the item to insert</param>
		public void CreateGroup (IListItem? target, GroupListObjectViewModel input)
		{
			// if target is null, try to use the selected item
			if (target == null) {
				target = SelectedGroupViewModel;
			}

			// de-select the target
			if (target != null) {
				((GroupListObjectViewModel)target).IsSelected = false; // TODO: can probably just pass in a GroupListObjectViewModel for the target
			}

			Resource.GroupListViewModel.Insert(target, input);

			// set selected group to display
			SelectGroup(input);
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyGroup (IListItem obj)
		{
			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (Resource.GroupListViewModel.Highlighted == null) {
				Resource.GroupListViewModel.Highlighted = SelectedGroupViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedGroupViewModel == obj) {
				if (Resource.GroupListViewModel.Highlighted == obj) {
					if (obj.Next != null) {
						SelectGroup((GroupListObjectViewModel)obj.Next);
					}
					else if (obj.Previous != null) {
						SelectGroup((GroupListObjectViewModel)obj.Previous);
					}
					Resource.GroupListViewModel.Highlighted = SelectedGroupViewModel;
				}
				else if (Resource.GroupListViewModel.Highlighted != null) {
					SelectGroup((GroupListObjectViewModel)Resource.GroupListViewModel.Highlighted);
				}
			}
			else {
				if (Resource.GroupListViewModel.Highlighted == obj) {
					Resource.GroupListViewModel.Highlighted = SelectedGroupViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}

			// add a list item if none remain
			if (Resource.GroupListViewModel.Items.Count() == 1) {
				GroupListObjectViewModel newGroup = Resource.GroupListViewModel.Create();
				CreateGroup(null, newGroup);
				SelectGroup(newGroup);
				Resource.GroupListViewModel.Highlighted = newGroup;
			}

			Resource.GroupListViewModel.Remove(obj);

			//Resource.GroupListViewModel.RefreshListView();
		}

		#endregion

		#region Events: Group

		/// <summary>
		/// // assign Group's selected note
		/// </summary>
		/// <param name="note"></param>
		public void SelectGroupNote (GroupObjectViewModel note)
		{
			SelectedGroupNoteViewModel = note;
			SelectedGroupNoteViewModel.IsSelected = true;
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyGroupNote (IListItem obj)
		{
			if (SelectedGroupViewModel == null) {
				throw new NullReferenceException("no Group is selected from which to remove a Note");
			}

			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (Resource.GroupContentsViewModel.Highlighted == null) {
				Resource.GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedGroupNoteViewModel == obj) {
				if (Resource.GroupContentsViewModel.Highlighted == obj) {
					if (obj.Next != null) {
						SelectGroupNote((GroupObjectViewModel)obj.Next);
					}
					else if (obj.Previous != null) {
						SelectGroupNote((GroupObjectViewModel)obj.Previous);
					}
					Resource.GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
				}
				else if (Resource.GroupContentsViewModel.Highlighted != null) {
					SelectGroupNote((GroupObjectViewModel)Resource.GroupContentsViewModel.Highlighted);
				}
			}
			else {
				if (Resource.GroupContentsViewModel.Highlighted == obj) {
					Resource.GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}

			Resource.GroupContentsViewModel.Remove(obj);

			//Resource.GroupContentsViewModel.RefreshListView();
		}

		#endregion
	}
}








