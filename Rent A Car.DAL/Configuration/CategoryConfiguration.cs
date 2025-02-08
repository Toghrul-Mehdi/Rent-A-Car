using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rent_A_Car.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.DAL.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasIndex(c=>c.CategoryName)
                .IsUnique();

            builder.Property(c => c.CategoryName)
                   .IsRequired()
                   .HasMaxLength(100); 

            builder.Property(c => c.CreatedTime)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()"); 

            builder.Property(c => c.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false);

            
                    
        }
    }
}
