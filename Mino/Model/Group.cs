using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;



namespace Mino.Model
{
	public class Group : IIdentifiable
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		public string Color {
			get => f_Color;
			set {
				try {
					Color c = System.Drawing.ColorTranslator.FromHtml(value);
					f_Color = value;
				}
				catch (Exception ex) {
					throw new ArgumentException(
						@$"Invalid color value. Must be valid HTML (3 or 6 digit hex code preceded by # symbol.
						{ ex.Message }"
					);
				}
			}
		}

		private string f_Color;

		public Group ()
		{
			f_Color = "#000";
			Title = String.Empty;
		}
	}

	public class GroupEqualityComparer : IEqualityComparer<Group>
	{
		public bool Equals (Group? lhs, Group? rhs)
		{
			return lhs?.Id == rhs?.Id;
		}

		public int GetHashCode ([DisallowNull] Group obj)
		{
			//return obj.GetHashCode();
			return obj.Id.GetHashCode() * 17;
		}
	}
}
