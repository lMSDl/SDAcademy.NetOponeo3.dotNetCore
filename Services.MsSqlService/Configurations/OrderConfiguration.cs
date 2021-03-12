using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Services.MsSqlService.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(x => x.User).WithMany().IsRequired().HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Tires).WithMany(x => x.Orders);

        }
    }
}