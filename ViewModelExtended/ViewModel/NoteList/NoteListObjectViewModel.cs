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
			get { return f_Model; }
		}

		private readonly NoteListObject f_Model;

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
			get { return f_Previous; }
			set {
				Set(ref f_Previous, value);
				NotifyPropertyChanged(nameof(PreviousId));
			}
		}

		private IListItem? f_Previous;

		public IListItem? Next {
			get { return f_Next; }
			set {
				Set(ref f_Next, value);
				NotifyPropertyChanged(nameof(NextId));
			}
		}

		private IListItem? f_Next;

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

		public string Text {
			get { return Model.Data.Text; }
			set {
				Model.Data.Text = value;
				Set(ref f_Text, value);
			}
		}

		private string f_Text;

		public DateTime DateCreated {
			get { return Utility.UnixToDateTime(Timestamp.UserCreated); }
		}

		#endregion



		#region Node Id

		public int? PreviousId {
			get { return Node.PreviousId; }
		}

		public int? NextId {
			get { return Node.NextId; }
		}

		#endregion



		#region Constructor

		public NoteListObjectViewModel (NoteListObject model)
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
	}
}
