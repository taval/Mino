using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text;



namespace Mino.Model
{
	public class GroupConfig : IEntityTypeConfiguration<Group>
	{
		public void Configure (EntityTypeBuilder<Group> builder)
		{
			builder.Property(model => model.Title).IsRequired();
		}
	}
}
