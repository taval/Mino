using System;



namespace Mino.Model
{
	public class State : IIdentifiable
	{
		public int Id { get; set; }

		public string Key {
			get { return f_Key; }
			set { if (f_Key.Equals(String.Empty)) f_Key = value; }
		}

		private string f_Key;

		public int Value { get; set; }

		public State ()
		{
			f_Key = String.Empty;
			Value = 0;
		}
	}
}
