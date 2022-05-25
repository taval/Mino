using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupListObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		#region Model

		public GroupListObject Model {
			get { return m_Model; }
		}

		private readonly GroupListObject m_Model;

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
			get { return m_IsSelected; }
			set { Set(ref m_IsSelected, value); }
		}

		private bool m_IsSelected;

		#endregion

		//public int? PreviousId {
		//	get { return Node.PreviousId; }
		//}

		//public int? NextId {
		//	get { return Node.NextId; }
		//}

		public string Title {
			get { return Model.Data.Title; }
			set {
				//Set(ref m_Title, value);
				Model.Data.Title = value;
				Set(ref m_Title, value);
			}
		}

		private string m_Title;

		public string Color {
			get { return Model.Data.Color; }
			set {
				//Set(ref m_Color, value);
				Model.Data.Color = value;
				Set(ref m_Color, value);
			}
		}

		private string m_Color;

		public GroupListObjectViewModel (GroupListObject model)
		{
			m_Model = model;
			Previous = null;
			Next = null;
			m_IsSelected = false;
			m_Title = Model.Data.Title;
			Title = m_Title;
			m_Color = Model.Data.Color;
			Color = m_Color;
		}
	}
}
