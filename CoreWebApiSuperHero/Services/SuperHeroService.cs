
using CoreWebApiSuperHero.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace CoreWebApiSuperHero.Services
{
    public class SuperHeroService : ISuperHeroService
    {
        /*public static List<SuperHero> SuperHeroes = new List<SuperHero> 
        {
            new SuperHero  { Id = 1,Name ="BatMan", FirstName = "Bruce", LastName = "Wayne", Place = "Gotham" },
            new SuperHero  { Id = 2, Name = "SuperMan", FirstName = "Clark", LastName = "Kent", Place = "Metropolis" }
        };*/

        private readonly DataContext _datacontext;

        public SuperHeroService(DataContext datacontext)
        {
            _datacontext = datacontext; // Injecting the DataContext to interact with the database
        }

        public async Task<List<SuperHero>> GetSuperHeroesAsync()
        {
            //return SuperHeroRepository.SuperHeroes.ToList(); // Fetch from in-memory repository
            return await _datacontext.SuperHeroes.ToListAsync(); // Fetch from database if using EF Core
        }

        public async Task<SuperHero?> GetSuperHeroByIdAsync(int id)
        {
            //SuperHero? superHero = SuperHeroes.FirstOrDefault(x => x.Id == id);
           // return SuperHeroRepository.SuperHeroes.FirstOrDefault(x => x.Id == id); // Fetch from in-memory repository
            SuperHero? superHero = await _datacontext.SuperHeroes.FindAsync(id);
            if (superHero == null)
            {
                return null; // or throw an exception if preferred
            }
            return superHero;
        }

        public async Task<SuperHero?> GetSuperHeroByNameAsync(string heroName)
        {
            //SuperHero? superHero = SuperHeroes.FirstOrDefault(x => x.Id == id);
            // return SuperHeroRepository.SuperHeroes.FirstOrDefault(x => x.Id == id); // Fetch from in-memory repository
            SuperHero? superHero = await _datacontext.SuperHeroes.Where(x => x.Name == heroName).FirstOrDefaultAsync();
            if (superHero == null)
            {
                return null; // or throw an exception if preferred
            }
            return superHero;
        }
        public async Task<List<SuperHero>> AddSuperHeroAsync(SuperHero hero)
        {
            /*SuperHeroes.Add(hero);
            return SuperHeroes;*/

            await _datacontext.SuperHeroes.AddAsync(hero);// using database ef 
            await _datacontext.SaveChangesAsync(); // Save changes to the database  
            return await _datacontext.SuperHeroes.ToListAsync(); // Fetch from database if using EF Core
        }

        public async Task<List<SuperHero>?> UpdateSuperHeroAsync(int id, SuperHero hero)
        {
            //SuperHero? existingHero = SuperHeroes.FirstOrDefault(x => x.Id == id);
            SuperHero? existingHero = await _datacontext.SuperHeroes.FindAsync(id);
            if (existingHero == null)
            {
                return null;
            }

            existingHero.Name = hero.Name;
            existingHero.FirstName = hero.FirstName;
            existingHero.LastName = hero.LastName;
            existingHero.Place = hero.Place;

            await _datacontext.SaveChangesAsync(); // Save changes to the database            
            return await _datacontext.SuperHeroes.ToListAsync(); // Fetch from database if using EF Core
            //return SuperHeroes;
        }

        public async Task<List<SuperHero>?> UpdateSuperHeroPartiallyAsync(int id, JsonPatchDocument<SuperHero> patchdocument)
        {
            //SuperHero? existingHero = SuperHeroes.FirstOrDefault(x => x.Id == id);
            SuperHero? existingHero = await _datacontext.SuperHeroes.FindAsync(id);
            if (existingHero == null)
            {
                return null; // or throw an exception if preferred
            }
            
            var updatedHero = new SuperHero
            {
                Id          = existingHero.Id,
                Name        = existingHero.Name,
                FirstName   = existingHero.FirstName,
                LastName    = existingHero.LastName,
                Place       = existingHero.Place
            };
            patchdocument.ApplyTo(updatedHero); // Apply the patch document to the existing hero

            existingHero.Name       = updatedHero.Name;
            existingHero.FirstName  = updatedHero.FirstName;
            existingHero.LastName   = updatedHero.LastName;
            existingHero.Place      = updatedHero.Place;
            

            await _datacontext.SaveChangesAsync(); // Save changes to the database
                                                   // 
            return await _datacontext.SuperHeroes.ToListAsync(); // Fetch from database if using EF Core
            //return SuperHeroes;
        }
        public async Task<List<SuperHero>?> DeleteSuperHeroAsync(int id)
        {
            //SuperHero? superHero = SuperHeroes.FirstOrDefault(x => x.Id == id);
            SuperHero? existingHero = await _datacontext.SuperHeroes.FindAsync(id);
            if (existingHero == null)
            {
                return null; // or throw an exception if preferred
            }
            _datacontext.SuperHeroes.Remove(existingHero);
            await _datacontext.SaveChangesAsync(); // Save changes to the database            
            return await _datacontext.SuperHeroes.ToListAsync(); // Fetch from database if using EF Core
            //return SuperHeroes;
        }
    }
}
