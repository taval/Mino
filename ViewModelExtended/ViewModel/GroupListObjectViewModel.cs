using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupListObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		public GroupListObject Model {
			get { return m_Model; }
		}

		private readonly GroupListObject m_Model;

		public int ItemId {
			get { return Model.Item.Id; }
			private set { Model.Item.Id = value; }
		}

		public INode Node {
			get { return Model.Node; }
		}

		public int? PreviousId {
			get {
				PreviousId = Node.PreviousId;
				return m_PreviousId;
			}
			private set { Set(ref m_PreviousId, value); }
		}

		private int? m_PreviousId;

		public int? NextId {
			get {
				NextId = Node.NextId;
				return m_NextId;
			}
			private set { Set(ref m_NextId, value); }
		}

		private int? m_NextId;

		public Timestamp Timestamp {
			get { return Model.Timestamp; }
		}

		public string Title {
			get { return Model.Data.Title; }
			set {
				Set(ref m_Title, value);
				Model.Data.Title = m_Title;
			}
		}

		private string m_Title;

		public string Color {
			get { return Model.Data.Color; }
			set {
				Set(ref m_Color, value);
				Model.Data.Color = m_Color;
			}
		}

		private string m_Color;

		// ViewModel link
		public IListItem Value => this;
		public IListItem? Previous { get; set; }
		public IListItem? Next { get; set; }

		public bool IsSelected {
			get { return m_IsSelected; }
			set { Set(ref m_IsSelected, value); }
		}

		private bool m_IsSelected;

		//public int Idx => throw new NotImplementedException();

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
