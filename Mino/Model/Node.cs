namespace Mino.Model
{
	public class Node : IIdentifiable
	{
		public int Id { get; set; }
		public int? PreviousId { get; set; }
		public int? NextId { get; set; }

		public Node ()
		{
			// Id = 0;
			PreviousId = null;
			NextId = null;
		}
	}
}
