#nullable enable

using Microsoft.EntityFrameworkCore;

namespace Entities.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.Person> Persons { get; set; }

        public DbSet<Entities.Country> Countries { get; set; }

        //services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=WARRIOR; Database=PersonsDb; Integrated Security=True; Trust Server Certificate=True"));
        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}