using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Mino.Model;

// TODO: replace the redundant setters in the getters in an attempt to trigger propertychanged handlers with a NotifyNoteChanged function to be called by NoteListObjectViewModel's propertychanged event handler. Getters here in GroupObjectViewModel should look just like the ones for Node and Timestamp

namespace Mino.ViewModel
{
	/// <summary>
	/// A Note visible under a GroupContentsViewModel list
	/// this object uses NotifyXChanged in contrast with setters because the data 
	/// </summary>
	public class GroupObjectViewModel : ViewModelBase, IListItem, ISelectable, IEquatable<IListItem>
	{
		#region Model

		/// <summary>
		/// the wrapper for all instantiated data associated with a particular model object
		/// </summary>
		public GroupObject Model {
			get { return f_Model; }
		}

		private readonly GroupObject f_Model;

		#endregion



		#region IObject

		/// <summary>
		/// list location data
		/// </summary>
		public Node Node {
			get { return Model.Node; }
		}

		/// <summary>
		/// temporal data related to the model
		/// </summary>
		public Timestamp Timestamp {
			get { return Model.Timestamp; }
		}

		#endregion



		#region IListItem

		/// <summary>
		/// identifier for the data wrapper containing identifiers for the model and all associated data
		/// </summary>
		public int ItemId {
			get { return Model.Item.Id; }
		}

		/// <summary>
		/// identifier for the data model the item is based around
		/// </summary>
		public int DataId {
			get { return Model.Data.Id; }
		}

		/// <summary>
		/// get the previous node in the list containing this one
		/// </summary>
		public IListItem? Previous { get; set; }

		/// <summary>
		/// get the next node in the list containing this one
		/// </summary>
		public IListItem? Next { get; set; }

		#endregion



		#region ISelectable

		/// <summary>
		/// this object is the one currently under edit
		/// </summary>
		public bool IsSelected {
			get { return f_IsSelected; }
			set { Set(ref f_IsSelected, value); }
		}

		private bool f_IsSelected;

		#endregion



		#region Data

		/// <summary>
		/// represents the title, for organizing, filename, etc
		/// </summary>
		public string Title {
			get { return Model.Data.Title; }
		}

		/// <summary>
		/// the serialized text contents
		/// </summary>
		public string Text {
			get { return Model.Data.Text; }
		}

		/// <summary>
		/// an integer representing the key to PriorityTypes dictionary
		/// </summary>
		public int Priority {
			get { return Model.Data.Priority; }
		}

		public Group Group {
			get { return Model.Group; }
		}

		#endregion



		#region Constructor

		public GroupObjectViewModel (GroupObject model)
		{
			f_Model = model;
			Previous = null;
			Next = null;
			IsSelected = false;
			NotifyTitleChanged();
			NotifyTextChanged();
			NotifyPriorityChanged();
		}

		#endregion



		#region Equality

		public bool Equals ([AllowNull] IListItem other)
		{
			return DataId == other?.DataId;
		}

		public void NotifyTitleChanged ()
		{
			NotifyPropertyChanged(nameof(Title));
		}

		public void NotifyTextChanged ()
		{
			NotifyPropertyChanged(nameof(Text));
		}

		public void NotifyPriorityChanged ()
		{
			NotifyPropertyChanged(nameof(Priority));
		}

		#endregion
	}
}
