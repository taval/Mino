using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Mino.Model
{
	public class GroupListItemConfig : IEntityTypeConfiguration<GroupListItem>
	{
		public void Configure (EntityTypeBuilder<GroupListItem> builder)
		{
			builder.Property(model => model.ObjectId).IsRequired();
			builder.Property(model => model.NodeId).IsRequired();
			builder.Property(model => model.TimestampId).IsRequired();
		}
	}
}
