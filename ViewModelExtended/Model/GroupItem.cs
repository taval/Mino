using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.Model
{
	public class GroupItem : IIdentifiable
	{
		public int Id { get; set; }
		public int GroupId { get; set; }
		public int ObjectId { get; set; }
		public int NodeId { get; set; }
		public int TimestampId { get; set; }

		public GroupItem ()
		{
			//Id = 0;
			GroupId = 0;
			ObjectId = 0;
			NodeId = 0;
			TimestampId = 0;
		}
	}
}
