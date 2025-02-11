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
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {

            builder.Property(v => v.Name)
                   .IsRequired()
                   .HasMaxLength(100);


           

            builder.Property(c => c.CreatedTime)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");



            builder.Property(c => c.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.HasOne(v => v.Brand)
                    .WithMany(b => b.Models)
                    .HasForeignKey(v => v.BrandId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Category)
                   .WithMany(c => c.Models)
                   .HasForeignKey(v => v.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
