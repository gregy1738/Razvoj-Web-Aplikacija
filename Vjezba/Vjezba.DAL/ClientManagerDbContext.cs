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

    }
}