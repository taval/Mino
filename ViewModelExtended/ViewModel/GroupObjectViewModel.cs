using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		public GroupObject Data {
			get { return m_Data; }
		}

		private readonly GroupObject m_Data;

		public int ItemId {
			get { return Data.Item.Id; }
			private set { Data.Item.Id = value; }
		}

		public INode Node {
			get { return Data.Node; }
		}

		// NOTE: for testing purposes only. assigns to itself solely to trigger INotifyPropertyChanged
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
			get { return Data.Timestamp; }
		}

		public string Title {
			get { return Data.Data.Title; }
			set {
				Set(ref m_Title, value);
				Data.Data.Title = m_Title;
			}
		}

		private string m_Title;

		public string Text {
			get { return Data.Data.Text; }
			set {
				Set(ref m_Text, value);
				Data.Data.Text = m_Text;
			}
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

		public GroupObjectViewModel (GroupObject data)
		{
			m_Data = data;
			Previous = null;
			Next = null;
			m_Title = string.Empty;
			m_Text = string.Empty;
		}
	}
}
