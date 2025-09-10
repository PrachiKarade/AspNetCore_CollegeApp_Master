using Microsoft.EntityFrameworkCore;

namespace CoreWebApiSuperHero.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<SuperHero> SuperHeroes { get; set; } = null!;// This property represents the collection of SuperHero entities (SuperHeroes Table) in the database
    }    
}
