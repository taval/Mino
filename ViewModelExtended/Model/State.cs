using System;
using System.ComponentModel.DataAnnotations.Schema;



namespace ViewModelExtended.Model
{
	// TODO: this class should be removed as it doesn't serve the intended purpose of simplifying preloading of ids so SaveChanges() doesn't have to be called repeatedly

	// NOTE: This class should never be used directly. The only available access to it is through DbContext via key.

	/// <summary>
	/// table with rows that coorespond to persistent variables, e.g. the id for the most recently created object
	/// </summary>
	public class State
	{
		#region Public Properties

		/// <summary>
		/// the identifier/PK for a row
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// a key for a persistent variable value
		/// currently used keys:
		/// - LastNoteId
		/// </summary>
		public string SKey { get; set; }

		/// <summary>
		/// the value associated with an SKey
		/// </summary>
		public int SValue { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		///  default constructor
		/// </summary>
		public State ()
		{
			this.SKey = String.Empty;
			this.SValue = 0;
		}

		#endregion
	}
}
