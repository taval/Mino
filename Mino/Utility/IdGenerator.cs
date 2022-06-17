using System;
using System.Collections.Generic;
using System.Text;



namespace Mino
{
	public class IdGenerator
	{
		public int LastId { get; private set; }

		public int Generate ()
		{
			LastId = LastId + 1;
			return LastId;
		}

		public IdGenerator ()
		{
			LastId = -1;
		}
	}
}
