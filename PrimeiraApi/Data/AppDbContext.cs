using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Models;

namespace PrimeiraApi.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.UseSqlite( "Data Source=database.sqlite" );
            optionsBuilder.LogTo( Console.WriteLine , LogLevel.Information );
            base.OnConfiguring( optionsBuilder );
        }
    }
}
