using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;



namespace Mino.Model
{
	public class GroupItem : IIdentifiable
	{
		public int Id { get; set; }

		[Required]
		public int GroupId { get; set; }

		[Required]
		public int ObjectId { get; set; }
		
		[Required]
		public int NodeId { get; set; }

		[Required]
		public int TimestampId { get; set; }

		public GroupItem ()
		{
			GroupId = 0;
			ObjectId = 0;
			NodeId = 0;
			TimestampId = 0;
		}
	}
}
