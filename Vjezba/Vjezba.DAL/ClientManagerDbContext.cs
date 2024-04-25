using Vjezba.Model; 
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Vjezba.DAL
{
    public class ClientManagerDbContext : DbContext
    {
        protected ClientManagerDbContext() { }
        public ClientManagerDbContext(DbContextOptions<ClientManagerDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Meeting> Meetings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { 
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<City>().HasData(new City { ID = 1, Name = "Zagreb" });
            modelBuilder.Entity<City>().HasData(new City { ID = 2, Name = "Split" });
            modelBuilder.Entity<City>().HasData(new City { ID = 3, Name = "Osijek" });
            modelBuilder.Entity<Client>().HasData(new Client {ID=1, FirstName="Ivan", LastName="Horvat", Email="ivan.horvat@gmail.com", Gender='M', Address="Ulica grada Mainza 3", PhoneNumber="0956789012", CityID=2});
        }

    }
}