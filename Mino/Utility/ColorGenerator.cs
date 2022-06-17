using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;



namespace Mino
{
	public class ColorGenerator
	{
		private Random f_Random = new Random();

		public Color LastColor { get; private set; }

		public Color Generate ()
		{
			Color color = LastColor;

			for (int i = 0; i < 100; i++) {
				color = Color.FromArgb(
					(byte)f_Random.Next(0, 256), (byte)f_Random.Next(0, 256), (byte)f_Random.Next(0, 256));

				if (!color.Equals(LastColor)) break;
			}

			// if no useful color found, just reverse the existing color
			if (color.Equals(LastColor)) {
				string colorString = ColorTranslator.ToHtml(color);
				string rawColor = colorString.Substring(1, colorString.Length - 1);
				char[] rawColorchars = rawColor.ToCharArray();
				Array.Reverse(rawColorchars);
				color = ColorTranslator.FromHtml("#" + new string(rawColorchars));
			}

			LastColor = color;

			return LastColor;
		}

		public ColorGenerator ()
		{
			LastColor = ColorTranslator.FromHtml("#000");
		}
	}
}
