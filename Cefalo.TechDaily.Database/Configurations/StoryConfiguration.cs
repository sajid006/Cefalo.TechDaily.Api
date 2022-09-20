using Cefalo.TechDaily.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.TechDaily.Database.Configurations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.HasKey(story => story.Id);

            builder.Property(story => story.Title).IsRequired().HasMaxLength(256);

            builder.Property(story => story.Description).IsRequired();

            builder.Property(story => story.AuthorName).IsRequired();

            builder.Property(story => story.CreatedAt).IsRequired();

            builder.Property(story => story.UpdatedAt).IsRequired();

            builder.HasOne(story => story.User).WithMany(user => user.Stories).HasForeignKey(user => user.AuthorName);
        }
    }
}
