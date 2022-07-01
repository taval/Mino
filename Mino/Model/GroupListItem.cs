namespace Mino.Model
{
	/// <summary>
	/// container for ids of Group plus list item model components
	/// </summary>
	public class GroupListItem : IIdentifiable
	{
		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int NodeId { get; set; }
		public int TimestampId { get; set; }

		public GroupListItem ()
		{
			//Id = 0;
			ObjectId = 0;
			NodeId = 0;
			TimestampId = 0;
		}
	}
}
