using System;
using System.ComponentModel.DataAnnotations;

/** Model conventions
 * - Id is private set - assuming that only EF/db is setting the model's id and nothing requiring the use of fields/OnModelCreating over simple Properties.
 * - NOTE: parameterless constructor is called by and most object instantiation done via initializers ({} syntax).
 * - using custom object model hierarchy for association instead of ef core conventions to promote flexibility in usage of basic data types, e.g. a Note may be associated with a Group, but could it also be independently associated with a Category, SubGroup, etc?
 */

namespace Mino.Model
{
	/// <summary>
	/// The object which contains text and location data for the note table
	/// </summary>
	public class Note : IIdentifiable
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		public string Text { get; set; }

		[Required]
		public int Priority { get; set; }

		public Note ()
		{
			Title = String.Empty;
			Text = String.Empty;
			Priority = 0;
		}
	}
}
