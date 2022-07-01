namespace Mino.Model
{
	/// <summary>
	/// container for ids of Note plus list item model components
	/// </summary>
	public class NoteListItem : IIdentifiable
	{
		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int NodeId { get; set; }
		public int TimestampId { get; set; }

		public NoteListItem ()
		{
			ObjectId = 0;
			NodeId = 0;
			TimestampId = 0;
		}
	}
}
