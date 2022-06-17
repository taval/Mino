using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;



namespace ViewModelExtended.Model
{
	public class NoteListItem : IIdentifiable
	{
		public int Id { get; set; }

		[Required]
		public int ObjectId { get; set; }

		[Required]
		public int NodeId { get; set; }

		[Required]
		public int TimestampId { get; set; }

		public NoteListItem ()
		{
			ObjectId = 0;
			NodeId = 0;
			TimestampId = 0;
		}
	}
}
