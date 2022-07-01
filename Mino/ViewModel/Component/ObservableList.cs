using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

/** NOTE:
 * because there is no input option for a comparator object, the input type must implement IEquatable<IListItem>
 * so that it does not rely on reference equality
 */

namespace Mino.ViewModel
{
	internal class ObservableList<T> : IObservableList<T> where T : IListItem
	{
		#region Collection

		protected readonly ObservableCollection<T> f_Observables = new ObservableCollection<T>();

		public IEnumerable<T> Items {
			get { return f_Observables; }
		}

		#endregion



		#region Constructor

		public ObservableList () { } // TODO: make a constructor for observablelist w/ data

		#endregion



		#region Methods

		public void AddSortedRange (IEnumerable<T> source)
		{
			if (Any()) {
				source.First().Previous = f_Observables.Last();
				f_Observables.Last().Next = source.First();
			}

			foreach (T item in source) {
				if (!f_Observables.Contains(item)) f_Observables.Add(item);
			}
		}

		public void AddRange (IEnumerable<T> source)
		{
			if (Any()) {
				source.First().Previous = f_Observables.Last();
				f_Observables.Last().Next = source.First();
			}

			foreach (T item in source) {
				Add(item);
			}
		}

		public void Add (T input)
		{
			if (f_Observables.Contains(input)) {
				return;
			}

			IListItem? previous = null;

			if (Any()) {
				previous = f_Observables.Last();
				previous.Next = input;
			}

			input.Previous = previous;

			f_Observables.Add(input);
		}

		public void Insert (IListItem? target, T input)
		{
			if (f_Observables.Contains(input)) {
				return;
			}

			if (target == null && Any()) {
				target = f_Observables.First();
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
			if (Any()) {
				f_Observables.Insert((target != null && Index(target) >= 0) ? Index(target) : 0, input);
			}
			else {
				f_Observables.Add(input);
			}
		}

		public void Reorder (IListItem source, IListItem target)
		{
			if (source == target) return;

			int oldIdx = f_Observables.IndexOf((T)source);
			int newIdx = f_Observables.IndexOf((T)target);

			IListItem? sourcePrevious = source.Previous;
			IListItem? sourceNext = source.Next;

			IListItem? targetPrevious = target.Previous;
			IListItem? targetNext = target.Next;

			int sourceIndex = Index(source);
			int targetIndex = Index(target);

			int diff = targetIndex - sourceIndex;
			bool isAdjacent = Math.Abs(diff) == 1;
			if (isAdjacent == true) {
				if (diff > 0) { // forward
					if (sourcePrevious != null) sourcePrevious.Next = target;
					source.Previous = target;
					target.Previous = sourcePrevious;
					target.Next = source;
					source.Next = targetNext;
					if (targetNext != null) targetNext.Previous = source;
				}
				else { // reverse
					if (sourceNext != null) sourceNext.Previous = target;
					target.Next = sourceNext;
					target.Previous = source;
					source.Next = target;
					source.Previous = targetPrevious;
					if (targetPrevious != null) targetPrevious.Next = source;
				}
			}
			else {
				if (diff > 0) { // forward
					if (sourcePrevious != null) sourcePrevious.Next = sourceNext;
					if (sourceNext != null) sourceNext.Previous = sourcePrevious;
					target.Next = source;
					source.Previous = target;
					source.Next = targetNext;
					if (targetNext != null) targetNext.Previous = source;
				}
				else { // reverse
					if (sourceNext != null) sourceNext.Previous = sourcePrevious;
					if (sourcePrevious != null) sourcePrevious.Next = sourceNext;
					target.Previous = source;
					source.Next = target;
					source.Previous = targetPrevious;
					if (targetPrevious != null) targetPrevious.Next = source;
				}
			}
			// perform move within the container
			// if both items exist, move model w/ oldIdx to newIdx
			if (oldIdx != -1 && newIdx != -1) {
				f_Observables.Move(oldIdx, newIdx);
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

			f_Observables.Remove((T)input);
		}

		public int Index (IListItem input)
		{
			return Array.IndexOf(f_Observables.ToArray(), input);
		}

		public void Clear ()
		{
			f_Observables.Clear();
		}

		public T Find (Func<T, bool> predicate)
		{
			IEnumerable<T> match = f_Observables.Where(predicate);
			if (!match.Any()) throw new Exception($"no matching { nameof(T) } records found");
			return match.First();
		}

		public bool Any ()
		{
			return f_Observables.Any();
		}

		public IEnumerable<T> Where (Func<T, bool> predicate)
		{
			return f_Observables.Where(predicate);
		}

		#endregion
	}
}
