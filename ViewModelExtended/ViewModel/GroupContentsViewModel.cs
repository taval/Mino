﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ViewModelExtended.Model;

// TODO: Use IObject for interfaces - testing Node without INode interface because some ghosty things are happening when trying to remove a dropped GroupNote

namespace ViewModelExtended.ViewModel
{
	/// <summary>
	/// ViewModel data for displaying any notes in a selected group
	/// </summary>
	public class GroupContentsViewModel : ViewModelBase
	{
		private IViewModelResource Resource { get; set; }

		private Dictionary<Group, IObservableList> Lists { get; set; }

		/// <summary>
		/// the base ListViewModel
		/// </summary>
		private IObservableList List {
			get {
				//if (m_List == null) return Resource.ViewModelCreator.CreateList();
				if (m_List == null) throw new NullReferenceException("GroupObject list not set");

				return m_List;
			}
			set {
				m_List = value;
				NotifyPropertyChanged(nameof(Items));
			}
		}

		private IObservableList? m_List;

		public IEnumerable<IListItem> Items {
			get { return List.Items; }
		}

		/// <summary>
		/// Group that is handed off from NoteTabsViewModel.SelectedGroup - represents an interface to SelectedGroup
		/// </summary>
		public GroupListObjectViewModel? ContentData {
			get { return m_ContentData; }
			set {
				Set(ref m_ContentData, value);
				NotifyPropertyChanged(nameof(Title));
				NotifyPropertyChanged(nameof(Color));
			}
		}

		private GroupListObjectViewModel? m_ContentData = null;

		public string Title {
			get {
				if (m_ContentData != null) return m_ContentData.Title;
				return "";
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Title, value)) return;
					m_ContentData.Title = value;
					NotifyPropertyChanged(nameof(Title));
				}
			}
		}

		public string Color {
			get {
				if (m_ContentData != null) return m_ContentData.Color;
				return "";
			}
			set {
				if (m_ContentData != null) {
					if (Equals(m_ContentData.Color, value)) return;
					m_ContentData.Color = value;
					NotifyPropertyChanged(nameof(Color));
				}
			}
		}

		#region Cross-View Data

		public GroupObjectViewModel? Highlighted {
			get { return m_Highlighted; }
			set {
				Set(ref m_Highlighted, value);
			}
		}

		private GroupObjectViewModel? m_Highlighted;

		/// <summary>
		/// the NoteListObjectViewModel received from an external list, e.g. from NoteListViewModel
		/// </summary>
		private GroupObjectViewModel? IncomingNoteViewModel { get; set; }

		#endregion



		#region Commands

		public ICommand ReorderCommand {
			get { return m_ReorderCommand ?? throw new MissingCommandException(); }
			set { if (m_ReorderCommand == null) m_ReorderCommand = value; }
		}

		private ICommand? m_ReorderCommand;

		//public ICommand PreselectCommand {
		//	get { return m_PreselectCommand ?? throw new MissingCommandException(); }
		//	set { if (m_PreselectCommand == null) m_PreselectCommand = value; }
		//}

		//private ICommand? m_PreselectCommand;

		public ICommand PickupCommand {
			get { return m_PickupCommand ?? throw new MissingCommandException(); }
			set { if (m_PickupCommand == null) m_PickupCommand = value; }
		}

		private ICommand? m_PickupCommand;

		/// <summary>
		/// sends a Note to a Group
		/// </summary>
		public ICommand NoteReceiveCommand {
			get { return m_NoteReceiveCommand ?? throw new MissingCommandException(); }
			set { if (m_NoteReceiveCommand == null) m_NoteReceiveCommand = value; }
		}

		private ICommand? m_NoteReceiveCommand;

		#endregion



		#region Constructor

		public GroupContentsViewModel (IViewModelResource resource)
		{
			Resource = resource;
			Resource.CommandBuilder.MakeGroup(this);
			m_ContentData = null;
			m_Highlighted = null;
			m_List = null;
			Lists = new Dictionary<Group, IObservableList>();
		}

		#endregion



		#region Access

		public void Add (GroupObjectViewModel input)
		{
			List.Add(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterAdd(dbContext, input);
				dbContext.Save();
			}
		}

		public void Insert (GroupObjectViewModel? target, GroupObjectViewModel input)
		{
			List.Insert(target, input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterInsert(dbContext, target, input);
				dbContext.Save();
			}
		}

		public void Reorder (GroupObjectViewModel source, GroupObjectViewModel target)
		{
			List.Reorder(source, target);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterReorder(dbContext, source, target);
				dbContext.Save();
			}
		}

		public void Remove (GroupObjectViewModel input)
		{
			List.Remove(input);

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				Resource.DbListHelper.UpdateAfterRemove(dbContext, input);
				dbContext.Save();
				Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, input);
			}
		}

		public int Index (GroupObjectViewModel input)
		{
			return List.Index(input);
		}

		#endregion



		#region Create

		public GroupObjectViewModel Create (Group groop, Note data)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				return Resource.ViewModelCreator.CreateGroupObjectViewModel(dbContext, groop, data);
			}
		}

		#endregion



		#region Refresh

		public void Refresh ()
		{
			Utility.RefreshListView(List.Items);
		}

		#endregion



		#region Clear

		public void Clear ()
		{
			List.Clear();
		}

		#endregion



		#region Set Group

		/// <summary>
		/// destroy a Group's contents and the associated list
		/// </summary>
		/// <param name="groop"></param>
		public void DestroyGroup (Group groop)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				if (!Lists.ContainsKey(groop)) return;

				IObservableList groupObjs = Lists[groop];

				foreach (GroupObjectViewModel obj in groupObjs.Items) {
					Remove(obj);
				}

				Lists.Remove(groop);
			}
		}

		/// <summary>
		/// removes all dependent GroupObjects (notes) associated with a given NoteListObject
		/// </summary>
		/// <param name="note"></param>
		/// <exception cref="NullReferenceException"></exception>
		public void RemoveGroupObjectsByNote (Note note)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// get the unbound data objects
				IQueryable<IListItem> groupObjectsByNote = Resource.DbQueryHelper.GetGroupObjectsByNote(dbContext, note);

				// for each group represented in original query (can just iterate over the query since one group exists per group object)
				foreach (GroupObjectViewModel obj in groupObjectsByNote) {
					IObservableList? list = null;

					// check if temp list is same as displayed list to prevent populating same list twice
					// select the list of the particular group or the display group
					bool isActiveGroup = obj.Model.Group.Id == ContentData?.Model.Data.Id;

					if (isActiveGroup) {
						list = List;
					}
					else {
						list = GetListByGroupKey(obj.Model.Group);
					}

					if (list == null) {
						throw new NullReferenceException("temporary Group contents list could not be set");
					}

					if (!isActiveGroup) {
						// populate the viewmodel list of that group
						PopulateGroup(dbContext, list, obj.Model.Group);
					}

					// select bound group object matching the unbound group object in the original query
					IEnumerable<IListItem> match =
						list.Items.Where((n) => ((GroupObjectViewModel)n).Model.Data.Id == note.Id);

					// remove the bound group object from the output list
					if (match.Count() > 0) {
						GroupObjectViewModel item = (GroupObjectViewModel)match.First();

						list.Remove(item);
						Resource.DbListHelper.UpdateAfterRemove(dbContext, item);
						dbContext.Save();
						Resource.ViewModelCreator.DestroyGroupObjectViewModel(dbContext, item);
					}
				}
				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		public void ReceiveGroupNote (NoteListObjectViewModel input)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// if no group is selected, bail out
				if (ContentData == null) {
					return;
				}

				// if Note already exists in Group, bail out
				if (Items.Contains(input, new GroupNoteObjectEqualityComparer())) {
					return;
				}

				// create a temporary GroupObject with the given NoteListObject
				GroupObjectViewModel groupNote =
					Resource.ViewModelCreator.CreateTempGroupObjectViewModel(
						dbContext, ContentData.Model.Data, input.Model.Data);

				IncomingNoteViewModel = groupNote;
				List.Add(groupNote);
			}
		}

		public void HoldGroupNote ()
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				if (IncomingNoteViewModel != null) {
					List.Remove(IncomingNoteViewModel);
					IncomingNoteViewModel = null;
				}
			}
		}

		/// <summary>
		/// adds a dependent GroupObject converted from given owner NoteListObject
		/// </summary>
		/// <param name="input"></param>
		//public void AddNoteToGroup (NoteListObjectViewModel input)
		//{
		//	// if no group is selected, bail out
		//	if (ContentData == null) {
		//		return;
		//	}

		//	// if Note already exists in Group, bail out
		//	if (Items.Contains(input, new GroupObjectEqualityComparer())) {
		//		return;
		//	}

		//	using (IDbContext dbContext = Resource.CreateDbContext()) {
		//		// associate a newly created GroupObject with the given NoteListObject
		//		GroupObjectViewModel groupNote =
		//			Resource.ViewModelCreator.CreateGroupObjectViewModel(
		//				dbContext, ContentData.Model.Data, input.Model.Data);

		//		// add the GroupObject to the contents list
		//		Add(groupNote);
		//	}
		//}

		public void AddNoteToGroup ()
		{
			if (ContentData == null || IncomingNoteViewModel == null) return;

			using (IDbContext dbContext = Resource.CreateDbContext()) {
				// remove the temp from list
				List.Remove(IncomingNoteViewModel);

				Group groop = ContentData.Model.Data;
				Note note = IncomingNoteViewModel.Model.Data;

				IncomingNoteViewModel = null;

				// associate a newly created GroupObject with the given temporary GroupObject
				GroupObjectViewModel groupNote =
					Resource.ViewModelCreator.CreateGroupObjectViewModel(dbContext, groop, note);

				// add the GroupObject to the contents list
				Add(groupNote);
			}
		}

		/// <summary>
		/// set the displayed Group contents
		/// </summary>
		/// <param name="groop"></param>
		public void SetGroup (Group groop)
		{
			using (IDbContext dbContext = Resource.CreateDbContext()) {
				IObservableList list = GetListByGroupKey(groop);

				PopulateGroup(dbContext, list, groop);
				List = list;
			}
		}

		/// <summary>
		/// set the given list with all sorted GroupObjects (notes) within a Group
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="list"></param>
		/// <param name="groop"></param>
		private void PopulateGroup (IDbContext dbContext, IObservableList list, Group groop)
		{
			IQueryable<IListItem> unsortedObjects = Resource.DbQueryHelper.GetGroupObjectsInGroup(dbContext, groop);

			Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects, list);
		}

		/// <summary>
		/// gets the list by using Group as key
		/// </summary>
		/// <param name="groop"></param>
		/// <returns>the GroupObject list associated with the given Group key</returns>
		private IObservableList GetListByGroupKey (Group groop)
		{
			IEnumerable<KeyValuePair<Group, IObservableList>> selectedList = Lists.Where((kv) => kv.Key == groop);

			if (selectedList.Count() > 0) {
				return selectedList.First().Value;
			}
			else {
				IObservableList list = Resource.ViewModelCreator.CreateList();
				Lists.Add(groop, list);
				return list;
			}
		}

		#endregion



		//// old SetGroup (prior to Dictionary upgrade)
		//public void SetGroup (GroupListObjectViewModel groop)
		//{
		//	using (IDbContext dbContext = Resource.CreateDbContext()) {
		//		IQueryable<IListItem> unsortedObjects =
		//			Resource.DbQueryHelper.GetGroupObjectsInGroup(dbContext, groop.Model.Data);

		//		Resource.DbQueryHelper.GetSortedListObjects(unsortedObjects, List);
		//	}
		//}



		//#region Add Note to Group

		//public void AddNoteToGroup (IListItem input)
		//{
		//	// if no group is selected, bail out
		//	if (ContentData == null) {
		//		return;
		//	}

		//	// associate a newly created GroupObject with the given NoteListObject
		//	NoteListObjectViewModel originalNote = (NoteListObjectViewModel)input;
		//	GroupObjectViewModel groupNote =
		//		Resource.ViewModelCreator.CreateGroupObjectViewModel(ContentData.Data.Data, originalNote.Data.Data);

		//	// add the GroupObject to the contents list
		//	Add(groupNote);
		//}

		//#endregion
	}
}
