using System;
using System.Drawing;



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
				char[] rawColorChars = rawColor.ToCharArray();
				Array.Reverse(rawColorChars);
				color = ColorTranslator.FromHtml("#" + new string(rawColorChars));
			}

			LastColor = color;

			return LastColor;
		}

		public string GenerateHtml ()
		{
			Color color = Generate();

			return ColorTranslator.ToHtml(color);
		}

		public ColorGenerator ()
		{
			LastColor = ColorTranslator.FromHtml("#000");
		}
	}
}
