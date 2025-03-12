using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_A_Car.DAL.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<CarImages> CarImages { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Question> Questions{ get; set; }
        public DbSet<Odenis> Odenis{ get; set; }
        public DbSet<Cixaris> Cixaris{ get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
