using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text;



namespace Mino.Model
{
	public class GroupItemConfig : IEntityTypeConfiguration<GroupItem>
	{
		public void Configure (EntityTypeBuilder<GroupItem> builder)
		{
			builder.Property(model => model.ObjectId).IsRequired();
			builder.Property(model => model.NodeId).IsRequired();
			builder.Property(model => model.TimestampId).IsRequired();
			builder.Property(model => model.GroupId).IsRequired();
		}
	}
}
