using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace Services.MsSqlService.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Login).IsRequired();
            builder.Property(x => x.Password).IsRequired().HasMaxLength(72);

            //builder.HasKey(x => x.Id);

            builder.HasData(new User {Id = 1, Login = "Admin", Password = "nimdA", Role = (Roles)Enum.GetValues<Roles>().Cast<int>().Sum()});
        }
    }
}