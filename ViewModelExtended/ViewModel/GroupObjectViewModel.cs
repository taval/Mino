using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		public GroupObject Model {
			get { return m_Model; }
		}

		private readonly GroupObject m_Model;

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
