using System;
using System.Collections.Generic;
using System.Drawing;



namespace ViewModelExtended.Model
{
	public class Group : IIdentifiable
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Color {
			get => m_Color;
			set {
				try {
					Color c = System.Drawing.ColorTranslator.FromHtml(value);
					m_Color = value;
				}
				catch (Exception ex) {
					throw new ArgumentException(
						@$"Invalid color value. Must be valid HTML (3 or 6 digit hex code preceded by # symbol.
						{ ex.Message }"
					);
				}
			}
		}

		private string m_Color;

		public Group ()
		{
			//Id = 0;
			m_Color = "#000";
			Title = String.Empty;
		}
	}
}
