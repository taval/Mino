using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupObjectViewModel : ViewModelBase, IListItem, ISelectable, IEquatable<IListItem>
	{
		#region Model

		public GroupObject Model {
			get { return f_Model; }
		}

		private readonly GroupObject f_Model;

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
			get {
				Title = Model.Data.Title;
				return f_Title;
			}
			private set { Set(ref f_Title, value); }
		}

		private string f_Title;

		public string Text {
			get {
				Text = Model.Data.Text;
				return f_Text;
			}
			private set { Set(ref f_Text, value); }
		}

		private string f_Text;

		#endregion



		#region constructor

		public GroupObjectViewModel (GroupObject model)
		{
			f_Model = model;
			Previous = null;
			Next = null;
			f_IsSelected = false;
			f_Title = Model.Data.Title;
			Title = f_Title;
			f_Text = Model.Data.Text;
			Text = f_Text;
		}

		#endregion

		public bool Equals ([AllowNull] IListItem other)
		{
			return DataId == other?.DataId;
		}
	}
}
