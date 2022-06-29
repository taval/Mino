using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text;



namespace Mino.Model
{
	public class StateConfig : IEntityTypeConfiguration<State>
	{
		public void Configure (EntityTypeBuilder<State> builder)
		{
			builder.Property(model => model.Key).IsRequired();
			builder.Property(model => model.Value).IsRequired();
		}
	}
}
