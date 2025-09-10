using System.Collections.Generic;

namespace CoreWebApiSuperHero.Models
{
    public static class SuperHeroRepository
    {
        public static IEnumerable<SuperHero> SuperHeroes = new List<SuperHero> 
        {
            new SuperHero  { Id = 1,Name ="BatMan", FirstName = "Bruce", LastName = "Wayne", Place = "Gotham" },
            new SuperHero  { Id = 2, Name = "SuperMan", FirstName = "Clark", LastName = "Kent", Place = "Metropolis" }
        };
    }
}
