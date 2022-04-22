using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;



namespace ViewModelExtended.ViewModel
{
	public class ObservableList : IObservableList
	{
		#region Collection

		protected readonly ObservableCollection<IListItem> m_Observables = new ObservableCollection<IListItem>();

		public IEnumerable<IListItem> Items {
			get { return m_Observables; }
		}

		#endregion

		#region Constructor

		public ObservableList () { } // TODO

		#endregion

		#region Methods

		public void Add (IListItem input)
		{
			if (Items.Contains(input)) {
				return;
			}

			IListItem? previous = null;
			int objCount = m_Observables.Count();

			// connect nodes
			if (objCount > 0) {
				previous = Items.Last();
				previous.Next = input;
			}

			input.Previous = previous;

			m_Observables.Add(input);
		}

		public void Insert (IListItem? target, IListItem input)
		{
			if (Items.Contains(input)) {
				return;
			}

			int objCount = m_Observables.Count();

			if (target == null && objCount > 0) {
				target = Items.First();
			}

			if (target != null) {
				// connect the nodes using the list's structure as a guide
				input.Previous = target.Previous;
				target.Previous = input;
			}

			if (input.Previous != null) {
				input.Previous.Next = input;
			}
			input.Next = target;

			// insert node into list (> 0: at target obj's index; <= 0: first index; no list items: add the first obj)
			if (objCount > 0) {
				//m_Observables.Insert((target != null && target.Idx >= 0) ? target.Idx : 0, input);
				m_Observables.Insert((target != null && Index(target) >= 0) ? Index(target) : 0, input);
			}
			else {
				m_Observables.Add(input);
			}
		}

		public void Reorder (IListItem source, IListItem target)
		{
			if (source == target) {
				return;
			}

			int oldIdx = m_Observables.IndexOf(source);
			int newIdx = m_Observables.IndexOf(target);

			IListItem? lhs = (oldIdx < newIdx) ? source.Previous : target.Previous;
			IListItem? rhs = (oldIdx < newIdx) ? target.Next : source.Next;

			if (oldIdx < newIdx) {

				if (lhs != null) {
					lhs.Next = target;
				}
				if (rhs != null) {
					rhs.Previous = source;
				}
				target.Previous = lhs;
				target.Next = source;
				source.Previous = target;
				source.Next = rhs;
			}
			else {
				if (lhs != null) {
					lhs.Next = source;
				}
				if (rhs != null) {
					rhs.Previous = target;
				}
				source.Previous = lhs;
				source.Next = target;
				target.Previous = source;
				target.Next = rhs;
			}

			// perform move within the container
			// if both items exist, move model w/ oldIdx to newIdx
			if (oldIdx != -1 && newIdx != -1) {
				m_Observables.Move(oldIdx, newIdx);
			}
		}

		public void Remove (IListItem input)
		{
			IListItem? oldPrevious = input.Previous;
			IListItem? oldNext = input.Next;

			// join extremities
			if (oldPrevious != null) {
				oldPrevious.Next = input.Next;
			}
			if (oldNext != null) {
				oldNext.Previous = input.Previous;
			}

			m_Observables.Remove(input);
		}

		public int Index (IListItem input)
		{
			return Array.IndexOf(m_Observables.ToArray(), input);
		}

		public void Clear ()
		{
			m_Observables.Clear();
		}

		//public void LinkAll ()
		//{
		//	// create the relationship between Notes in a linked list
		//	// assumes note begin/end nodes will correctly contain null upon immediate construction
		//	// this maybe useful later for something. check the model's implementation against this

		//	int id = 1;
		//	IListItem? prev = null;
		//	IListItem? current = null;
		//	IListItem? next = null;

		//	foreach (IListItem note in Items) {
		//		note.Id = id++; // TODO: id is set by model

		//		if (prev == null) {
		//			prev = note;
		//			continue;
		//		}
		//		if (prev != null && current == null) {
		//			current = note;
		//			continue;
		//		}
		//		if (prev != null && current != null) {
		//			next = note;
		//			prev.Next = current;
		//			current.Previous = prev;
		//			current.Next = next;
		//			next.Previous = current;
		//			prev = current;
		//			current = next;
		//		}
		//	}
		//}

		#endregion
	} // end class
} // end namespace
