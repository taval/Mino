using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



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
