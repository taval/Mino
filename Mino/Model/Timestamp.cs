namespace Mino.Model
{
	public class Timestamp : IIdentifiable
	{
		public int Id { get; set; }

		public long UserCreated { get; set; }
		
		public long? UserModified { get; set; }
		public long? UserIndexed { get; set; }
		public long? AutoModified { get; set; }

		public Timestamp ()
		{
			UserCreated = Utility.UnixDateTime();
			UserModified = 0;
			UserIndexed = 0;
			AutoModified = 0;
		}
	}
}
