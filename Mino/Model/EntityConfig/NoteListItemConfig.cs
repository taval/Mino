using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text;



namespace Mino.Model
{
	public class NoteListItemConfig : IEntityTypeConfiguration<NoteListItem>
	{
		public void Configure (EntityTypeBuilder<NoteListItem> builder)
		{
			builder.Property(model => model.ObjectId).IsRequired();
			builder.Property(model => model.NodeId).IsRequired();
			builder.Property(model => model.TimestampId).IsRequired();
		}
	}
}
