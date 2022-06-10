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

		public GroupListObject Model {
			get { return f_Model; }
		}

		private readonly GroupListObject f_Model;

		#endregion



		#region IObject

		public Node Node {
			get { return Model.Node; }
		}

		public Timestamp Timestamp {
			get { return Model.Timestamp; }
		}

		#endregion



		#region IListItem

		public int ItemId {
			get { return Model.Item.Id; }
		}

		public int DataId {
			get { return Model.Data.Id; }
		}

		public IListItem? Previous { get; set; }
		public IListItem? Next { get; set; }

		#endregion



		#region ISelectable

		public bool IsSelected {
			get { return f_IsSelected; }
			set { Set(ref f_IsSelected, value); }
		}

		private bool f_IsSelected;

		#endregion



		#region Data

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
