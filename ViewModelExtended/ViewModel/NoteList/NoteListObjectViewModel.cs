using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using ViewModelExtended.Model;



namespace ViewModelExtended.ViewModel
{
	public class NoteListObjectViewModel : ViewModelBase, IListItem, ISelectable
	{
		#region Model

		public NoteListObject Model {
			get { return m_Model; }
		}

		private readonly NoteListObject m_Model;

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

		public IListItem? Previous {
			get { return m_Previous; }
			set {
				Set(ref m_Previous, value);
				NotifyPropertyChanged(nameof(PreviousId));
			}
		}

		private IListItem? m_Previous;

		public IListItem? Next {
			get { return m_Next; }
			set {
				Set(ref m_Next, value);
				NotifyPropertyChanged(nameof(NextId));
			}
		}

		private IListItem? m_Next;

		#endregion

		#region ISelectable

		public bool IsSelected {
			get { return m_IsSelected; }
			set { Set(ref m_IsSelected, value); }
		}

		private bool m_IsSelected;

		#endregion

		public int? PreviousId {
			get { return Node.PreviousId; }
		}

		public int? NextId {
			get { return Node.NextId; }
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

		public DateTime DateCreated {
			get { return Utility.UnixToDateTime(Timestamp.UserCreated); }
		}

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
