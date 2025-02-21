using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.DAL.Configuration
{
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.ToTable("Advertisements");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Description)
                .HasMaxLength(1000);

            builder.Property(a => a.ImageUrl)
                .HasMaxLength(500);

            builder.Property(a => a.ViewCount)
                .IsRequired();

            builder.Property(a => a.Like)
                .IsRequired();

            builder.Property(a => a.Price)
                .IsRequired();

            builder.Property(a => a.Year)
                .IsRequired();

            builder.Property(a => a.PhoneNumber)
                .IsRequired();

            builder.Property(a => a.MinimalGunSayi)
                .IsRequired();

            builder.Property(a => a.MinimalSuruculukVesiqesi)
                .IsRequired();

            builder.Property(a => a.Model)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Status)
                .IsRequired();

            builder.Property(a => a.VipStarted)
                .IsRequired();

            builder.Property(a => a.VipEnded)
                .IsRequired();

            // User ve Category ile ilişkiler
            builder.HasOne(a => a.User)
                .WithMany(u => u.Advertisements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION          

            
        }
    }

}
