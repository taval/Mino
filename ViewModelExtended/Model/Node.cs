﻿using System;
using System.Collections.Generic;
using System.Text;



namespace ViewModelExtended.Model
{
	public class Node : INode
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
