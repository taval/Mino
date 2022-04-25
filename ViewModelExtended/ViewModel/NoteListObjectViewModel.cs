using System;
using System.Collections.Generic;
using System.Text;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class NoteListObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		public NoteListObject Model {
			get { return m_Model; }
		}

		private readonly NoteListObject m_Model;

		public int ItemId {
			get { return Model.Item.Id; }
			private set { Model.Item.Id = value; }
		}

		public INode Node {
			get { return Model.Node; }
		}

		public int? PreviousId {
			get { return Node.PreviousId; }
		}

		public int? NextId {
			get { return Node.NextId; }
		}

		public Timestamp Timestamp {
			get { return Model.Timestamp; }
		}

		public string Title {
			get { return Model.Data.Title; }
			set {
				Model.Data.Title = value;
				Set(ref m_Title, value);
			}
		}

		private string m_Title;

		public string Text {
			get { return Model.Data.Text; }
			set {
				Model.Data.Text = value;
				Set(ref m_Text, value);
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

		public NoteListObjectViewModel (NoteListObject model)
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
