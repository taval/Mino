using System;



/** Model conventions
 * - Id is private set - assuming that only EF/db is setting the model's id and nothing requiring the use of fields/OnModelCreating over simple Properties.
 * - NOTE: parameterless constructor is called by and most object instantiation done via initializers ({} syntax).
 * - convention 4 from https://www.entityframeworktutorial.net/efcore/one-to-many-conventions-entity-framework-core.aspx
 */

namespace ViewModelExtended.Model
{
	/// <summary>
	/// The object which contains text and location data for the note table
	/// </summary>
	//public class Note : IIdentifiable
	public class Note
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }

		public Note ()
		{
			// Id = 0;
			Title = String.Empty;
			Text = String.Empty;
		}
	}
}
