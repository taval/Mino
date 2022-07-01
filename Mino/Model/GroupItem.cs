namespace Mino.Model
{
	/// <summary>
	/// container for ids of Note-in-Group plus list item model components
	/// </summary>
	public class GroupItem : IIdentifiable
	{
		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int NodeId { get; set; }
		public int TimestampId { get; set; }
		public int GroupId { get; set; }

		public GroupItem ()
		{
			ObjectId = 0;
			NodeId = 0;
			TimestampId = 0;
			GroupId = 0;
		}
	}
}
