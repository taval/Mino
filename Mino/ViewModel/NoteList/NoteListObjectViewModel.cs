using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mino.Model;



namespace Mino.ViewModel
{
	public class NoteListObjectViewModel : ViewModelBase, IListItem, ISelectable, IEquatable<IListItem>
	{
		public static IList<string> PriorityTypes { get; private set; }

		static NoteListObjectViewModel ()
		{
			PriorityTypes = new List<string>();
		}

		#region Model

		/// <summary>
		/// the wrapper for all instantiated data associated with a particular model object
		/// </summary>
		public NoteListObject Model {
			get { return f_Model; }
		}

		private readonly NoteListObject f_Model;

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
			set {
				Model.Data.Title = value;
				NotifyPropertyChanged(nameof(Title));
			}
		}

		/// <summary>
		/// the serialized text contents
		/// </summary>
		public string Text {
			get { return Model.Data.Text; }
			set {
				Model.Data.Text = value;
				NotifyPropertyChanged(nameof(Text));
			}
		}

		/// <summary>
		/// an integer representing the key to PriorityTypes dictionary
		/// </summary>
		public int Priority {
			get { return Model.Data.Priority; }
			set {
				Model.Data.Priority = value;
				NotifyPropertyChanged(nameof(Priority));
			}
		}

		/// <summary>
		/// the time this object was created
		/// </summary>
		public DateTime DateCreated {
			get { return Utility.UnixToDateTime(Timestamp.UserCreated); }
		}

		#endregion



		#region Constructor

		public NoteListObjectViewModel (NoteListObject model)
		{
			f_Model = model;
			Previous = null;
			Next = null;
			IsSelected = false;
			NotifyPropertyChanged(nameof(Title));
			NotifyPropertyChanged(nameof(Text));
			NotifyPropertyChanged(nameof(Priority));
		}

		#endregion

		public bool Equals ([AllowNull] IListItem other)
		{
			return DataId == other?.DataId;
		}
	}
}
