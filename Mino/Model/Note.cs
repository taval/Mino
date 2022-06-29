using System;
using System.ComponentModel.DataAnnotations;



namespace Mino.Model
{
	/// <summary>
	/// The object which contains text and location data for the note table
	/// </summary>
	public class Note : IIdentifiable
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public int Priority { get; set; }

		public Note ()
		{
			Title = String.Empty;
			Text = String.Empty;
			Priority = 0;
		}
	}
}
