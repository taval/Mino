using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text;



namespace Mino.Model
{
	public class NoteConfig : IEntityTypeConfiguration<Note>
	{
		public void Configure (EntityTypeBuilder<Note> builder)
		{
			builder.Property(model => model.Title).IsRequired();
			builder.Property(model => model.Priority).IsRequired();
		}
	}
}
