#define USE_SQLITE

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace webapi_sample.Entity
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> Persons {get; set;}

        private readonly IConfiguration _configuration;

        public PersonContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if USE_SQLITE
            optionsBuilder.UseSqlite(_configuration.GetValue<System.String>("connectionString_sqlite"));
#else
            optionsBuilder.UseSqlServer(_configuration.GetValue<System.String>("connectionString_sqlserver"));
#endif

        }
    }
}
