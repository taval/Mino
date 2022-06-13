using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupListObjectViewModel : ViewModelBase, IListItem, ISelectable, IEquatable<IListItem>
	{
		#region Model

		/// <summary>
		/// the wrapper for all instantiated data associated with a particular model object
		/// </summary>
		public GroupListObject Model {
			get { return f_Model; }
		}

		private readonly GroupListObject f_Model;

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
				Set(ref f_Title, value);
			}
		}

		private string f_Title;

		public string Color {
			get { return Model.Data.Color; }
			set {
				Model.Data.Color = value;
				Set(ref f_Color, value);
			}
		}

		private string f_Color;

		#endregion



		#region Constructor

		public GroupListObjectViewModel (GroupListObject model)
		{
			f_Model = model;
			Previous = null;
			Next = null;
			f_IsSelected = false;
			f_Title = Model.Data.Title;
			Title = f_Title;
			f_Color = Model.Data.Color;
			Color = f_Color;
		}

		#endregion

		public bool Equals ([AllowNull] IListItem other)
		{
			return DataId == other?.DataId;
		}
	}
}
