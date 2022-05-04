using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		#region Model

		public GroupObject Model {
			get { return m_Model; }
		}

		private readonly GroupObject m_Model;

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
			get {
				Title = Model.Data.Title;
				return m_Title;
			}
			private set { Set(ref m_Title, value); }
		}

		private string m_Title;

		public string Text {
			get {
				Text = Model.Data.Text;
				return m_Text;
			}
			private set { Set(ref m_Text, value); }
		}

		private string m_Text;

		//public int Idx => throw new NotImplementedException();

		public GroupObjectViewModel (GroupObject model)
		{
			m_Model = model;
			Previous = null;
			Next = null;
			m_IsSelected = false;
			m_Title = Model.Data.Title;
			Title = m_Title;
			m_Text = Model.Data.Text;
			Text = m_Text;
		}
	}
}
