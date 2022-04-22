using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class GroupListObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		// base object - Db link and timestamp
		public GroupListObject Data {
			get { return m_Data; }
		}

		private readonly GroupListObject m_Data;

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

		public string Color {
			get { return Data.Data.Color; }
			set {
				Set(ref m_Color, value);
				Data.Data.Color = m_Color;
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

		public GroupListObjectViewModel (GroupListObject data)
		{
			m_Data = data;
			Previous = null;
			Next = null;
			m_IsSelected = false;
			m_Title = string.Empty;
			m_Color = "#888";
		}
	}
}
