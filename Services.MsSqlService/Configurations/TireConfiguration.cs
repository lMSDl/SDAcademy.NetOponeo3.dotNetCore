using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Services.MsSqlService.Configurations
{
    public class TireConfiguration : IEntityTypeConfiguration<Tire>
    {
        public void Configure(EntityTypeBuilder<Tire> builder)
        {
            builder.HasOne(x => x.Producer).WithMany(x => x.Tires).IsRequired();
        }
    }
}