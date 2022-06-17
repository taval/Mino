using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;



namespace ViewModelExtended.Model
{
	public class State : IIdentifiable
	{
		public int Id { get; set; }

		[Required]
		public string Key {
			get { return f_Key; }
			set { if (f_Key.Equals(String.Empty)) f_Key = value; }
		}

		private string f_Key;

		[Required]
		public int Value { get; set; }

		public State ()
		{
			f_Key = String.Empty;
			Value = 0;
		}
	}
}
