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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.UserID)
                .IsRequired();

            builder.Property(b => b.AdvertisementId)
                .IsRequired();

            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.Property(b => b.PickupTime)
                .IsRequired();

            builder.Property(b => b.DropoffTime)
                .IsRequired();

            builder.Property(b => b.TotalPrice)
                .IsRequired();

            builder.Property(b => b.Status)
                .IsRequired();

            builder.Property(b => b.PickupLocation)
                .IsRequired();

            builder.Property(b => b.DriverLicensePhoto)
                .IsRequired();

            // User ve Advertisement ilişkileri için ON DELETE NO ACTION
            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION

            builder.HasOne(b => b.Advertisement)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.AdvertisementId)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION
        }
    }

}
