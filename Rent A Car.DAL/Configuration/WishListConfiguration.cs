using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rent_A_Car.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.DAL.Configuration
{
    public class WishListConfiguration : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            

            builder.HasKey(x => new { x.UserId, x.AdvertisementId });

            builder
                .HasOne(w => w.User)
                .WithMany(u => u.WishList)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(w => w.Advertisement)
                .WithMany(a => a.WishList)
                .HasForeignKey(w => w.AdvertisementId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
