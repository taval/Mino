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

		public int SelectedTabIndex {
			get { return f_SelectedTabIndex; }
			set { Set(ref f_SelectedTabIndex, value); }
		}

		private int f_SelectedTabIndex;

		/// <summary>
		/// the GroupViewModel (data displayed by GroupView)
		/// NOTE: this is bound for purpose of GroupView population operation
		/// </summary>
		public GroupListObjectViewModel? SelectedGroupViewModel {
			get { return f_SelectedGroupViewModel; }
			set {
				Set(ref f_SelectedGroupViewModel, value);
				NotifyPropertyChanged("GroupTitle");
				if (f_SelectedGroupViewModel == null) return;
				f_SelectedGroupViewModel.PropertyChanged += (sender, e) =>
				{
					if (e.PropertyName == "Title") {
						NotifyPropertyChanged("GroupTitle");
					}
				};
			}
		}

		/// <summary>
		/// the selected group's title
		/// </summary>
		public string GroupTitle {
			get { return (SelectedGroupViewModel != null) ? SelectedGroupViewModel.Title : string.Empty; }
		}

		private GroupListObjectViewModel? f_SelectedGroupViewModel;

		/// <summary>
		/// the NoteViewModel selected in the Group Contents tab
		/// </summary>
		public GroupObjectViewModel? SelectedGroupNoteViewModel {
			get { return f_SelectedGroupNoteViewModel; }
			set { Set(ref f_SelectedGroupNoteViewModel, value); }
		}

		private GroupObjectViewModel? f_SelectedGroupNoteViewModel;

		#endregion



		#region Resource Component

		public IViewModelResource Resource { get; private set; }

		#endregion



		#region GroupList Commands

		public ICommand SwitchTabsCommand {
			get { return f_SwitchTabsCommand ?? throw new MissingCommandException(); }
			set { if (f_SwitchTabsCommand == null) f_SwitchTabsCommand = value; }
		}

		private ICommand? f_SwitchTabsCommand;

		public ICommand GroupSelectCommand {
			get { return f_GroupSelectCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupSelectCommand == null) f_GroupSelectCommand = value; }
		}

		private ICommand? f_GroupSelectCommand;

		public ICommand GroupCreateCommand {
			get { return f_GroupCreateCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupCreateCommand == null) f_GroupCreateCommand = value; }
		}

		private ICommand? f_GroupCreateCommand;

		public ICommand GroupDestroyCommand {
			get { return f_GroupDestroyCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupDestroyCommand == null) f_GroupDestroyCommand = value; }
		}

		private ICommand? f_GroupDestroyCommand;

		#endregion



		#region Group Commands

		public ICommand GroupNoteSelectCommand {
			get { return f_GroupNoteSelectCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupNoteSelectCommand == null) f_GroupNoteSelectCommand = value; }
		}

		private ICommand? f_GroupNoteSelectCommand;

		public ICommand GroupNoteDestroyCommand {
			get { return f_GroupNoteDestroyCommand ?? throw new MissingCommandException(); }
			set { if (f_GroupNoteDestroyCommand == null) f_GroupNoteDestroyCommand = value; }
		}

		private ICommand? f_GroupNoteDestroyCommand;

		#endregion



		#region Constructor

		public GroupTabsViewModel (IViewModelResource resource)
		{
			Resource = resource;
			f_SelectedGroupViewModel = null;
			f_SelectedGroupNoteViewModel = null;
			SelectedTabIndex = 0;
			Resource.CommandBuilder.MakeGroupTabs(this);
		}

		#endregion



		#region Load

		public void Load ()
		{
			// NOTE: NOT creating a new group if none exist - must be done manually by design

			if (Resource.GroupListViewModel.Items.Count() == 0) return;

			// select the first group
			SelectedGroupViewModel = Resource.GroupListViewModel.Items.First();

			if (SelectedGroupViewModel != null) {
				SelectGroup(SelectedGroupViewModel);
			}

			// highlight the first group
			Resource.GroupListViewModel.Highlighted = SelectedGroupViewModel;

			// select the first note in the selected group
			if (Resource.GroupContentsViewModel.Items.Count() == 0) return;

			SelectedGroupNoteViewModel = Resource.GroupContentsViewModel.Items.First();

			if (SelectedGroupNoteViewModel != null) {
				SelectGroupNote(SelectedGroupNoteViewModel);
			}

			// highlight the first note in the selected group
			Resource.GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
		}

		#endregion



		#region Events: GroupList

		/// <summary>
		/// adds a Group at the end of the group list
		/// </summary>
		/// <param name="input"></param>
		public void AddGroup (GroupListObjectViewModel input)
		{
			Resource.GroupListViewModel.Add(input);
		}

		/// <summary>
		/// inserts a new group into the group list; selects the newly created group
		/// </summary>
		/// <param name="target">the location at where the item will be inserted</param>
		/// <param name="input">the item to insert</param>
		public void CreateGroup (GroupListObjectViewModel? target, GroupListObjectViewModel input)
		{
			// if target is null, try to use the selected item
			if (target == null) {
				target = SelectedGroupViewModel;
			}

			// de-select the target
			if (target != null) {
				target.IsSelected = false;
			}

			Resource.GroupListViewModel.Insert(target, input);

			// set selected group to display
			SelectGroup(input);
		}

		/// <summary>
		/// set the Contents viewer to the selected group (this is generally the Highlighted item passed from GroupList to GroupTabs)
		/// </summary>
		/// <param name="group"></param>
		public void SelectGroup (GroupListObjectViewModel? groop)
		{
			SelectedGroupViewModel = groop;
			if (SelectedGroupViewModel != null) SelectedGroupViewModel.IsSelected = true;

			Resource.GroupContentsViewModel.ContentData = groop;
			//Group? data = groop?.Model.Data;
			//Resource.GroupContentsViewModel.SetGroup(data);
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyGroup (GroupListObjectViewModel input)
		{
			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (Resource.GroupListViewModel.Highlighted == null) {
				Resource.GroupListViewModel.Highlighted = SelectedGroupViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedGroupViewModel == input) {
				if (Resource.GroupListViewModel.Highlighted == input) {
					if (input.Next != null) {
						SelectGroup((GroupListObjectViewModel)input.Next);
					}
					else if (input.Previous != null) {
						SelectGroup((GroupListObjectViewModel)input.Previous);
					}
					Resource.GroupListViewModel.Highlighted = SelectedGroupViewModel;
				}
				else if (Resource.GroupListViewModel.Highlighted != null) {
					SelectGroup(Resource.GroupListViewModel.Highlighted);
				}
			}
			else {
				if (Resource.GroupListViewModel.Highlighted == input) {
					Resource.GroupListViewModel.Highlighted = SelectedGroupViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}

			// add a list item if none remain
			//if (Resource.GroupListViewModel.Items.Count() == 1) {
			//	GroupListObjectViewModel newGroup = Resource.GroupListViewModel.Create();
			//	CreateGroup(null, newGroup);
			//	SelectGroup(newGroup);
			//	Resource.GroupListViewModel.Highlighted = newGroup;
			//}
			if (Resource.GroupListViewModel.Items.Count() == 1) {
				SelectGroup(null);
				Resource.GroupListViewModel.Highlighted = null;
			}

			// destroy the GroupObjects (dependent Notes) in the group
			Resource.GroupContentsViewModel.DestroyList(input.Model.Data);

			// destroy the Group
			Resource.GroupListViewModel.Remove(input);

			//Resource.GroupListViewModel.RefreshListView();
		}

		#endregion



		#region Events: Group

		public void HoldGroupNote ()
		{
			Resource.GroupContentsViewModel.HoldGroupNote();
		}

		/// <summary>
		/// adds a dependent note to a group as a GroupObject
		/// </summary>
		/// <param name="input"></param>
		//public void AddNoteToGroup (NoteListObjectViewModel input)
		//{
		//	Resource.GroupContentsViewModel.AddNoteToGroup(input);
		//}
		public void AddNoteToGroup ()
		{
			Resource.GroupContentsViewModel.AddNoteToGroup();
		}

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
		/// removes all dependent GroupObjects (notes) associated with a given NoteListObject
		/// </summary>
		/// <param name="note"></param>
		/// <exception cref="NullReferenceException"></exception>
		public void RemoveGroupObjectsByNote (Note note)
		{
			GroupObjectViewModel? match = null;
			IEnumerable<GroupObjectViewModel> visibleGroupObj = Resource.GroupContentsViewModel.Items.Where(
				(item) => item.DataId == note.Id);

			if (visibleGroupObj.Any()) match = visibleGroupObj?.Single();

			if (match != null) AutoSelectFailSafe(match);

			Resource.GroupContentsViewModel.RemoveGroupObjectsByNote(note);
		}

		/// <summary>
		/// removes target object from list
		/// chooses a new selection when removing a list item (or doesn't)
		/// </summary>
		/// <param name="obj"></param>
		public void DestroyGroupNote (GroupObjectViewModel input)
		{
			if (SelectedGroupViewModel == null) {
				throw new NullReferenceException("no Group is selected from which to remove a Note");
			}

			AutoSelectFailSafe(input);

			Resource.GroupContentsViewModel.Remove(input);
		}

		/// <summary>
		/// selects another object linked to the input, for if/when the input becomes unavailable
		/// </summary>
		/// <param name="input"></param>
		private void AutoSelectFailSafe (GroupObjectViewModel input)
		{
			// don't let lack of highlighted item cause deleted item to not be de-selected
			if (Resource.GroupContentsViewModel.Highlighted == null) {
				Resource.GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
			}

			// compare what is selected to what is highlighted and what is to be deleted
			if (SelectedGroupNoteViewModel == input) {
				if (Resource.GroupContentsViewModel.Highlighted == input) {
					if (input.Next != null) {
						SelectGroupNote((GroupObjectViewModel)input.Next);
					}
					else if (input.Previous != null) {
						SelectGroupNote((GroupObjectViewModel)input.Previous);
					}
					Resource.GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;
				}
				else if (Resource.GroupContentsViewModel.Highlighted != null) {
					SelectGroupNote(Resource.GroupContentsViewModel.Highlighted);
				}
			}
			else {
				if (Resource.GroupContentsViewModel.Highlighted == input) {
					Resource.GroupContentsViewModel.Highlighted = SelectedGroupNoteViewModel;

				}
				else {
					// do nothing
					//Target = null;
				}
			}
		}

		#endregion
	}
}
