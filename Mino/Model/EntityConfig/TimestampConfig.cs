using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text;



namespace Mino.Model
{
	public class TimestampConfig : IEntityTypeConfiguration<Timestamp>
	{
		public void Configure (EntityTypeBuilder<Timestamp> builder)
		{
			builder.Property(model => model.UserCreated).IsRequired();
		}
	}
}
