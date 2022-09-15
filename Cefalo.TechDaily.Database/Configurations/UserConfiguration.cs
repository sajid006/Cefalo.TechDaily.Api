using Cefalo.TechDaily.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Database.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).IsRequired();
            builder.HasIndex(user => user.Id).IsUnique();

            builder.Property(user => user.Username).IsRequired().HasMaxLength(256);
            builder.HasIndex(user => user.Username).IsUnique();

            builder.Property(user => user.Email).IsRequired().HasMaxLength(256);
            builder.HasIndex(user => user.Email).IsUnique();

            builder.Property(user => user.PasswordHash).IsRequired();

            builder.Property(user => user.PasswordSalt).IsRequired();

            builder.Property(user => user.CreatedAt).IsRequired().ValueGeneratedOnAdd();

            builder.Property(user => user.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate();

        }
    }
}
