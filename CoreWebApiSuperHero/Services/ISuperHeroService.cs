using Microsoft.AspNetCore.JsonPatch;

namespace CoreWebApiSuperHero.Services
{
    public interface ISuperHeroService
    {
        Task<List<SuperHero>> GetSuperHeroesAsync();
        Task<SuperHero?> GetSuperHeroByIdAsync(int id);
        Task<SuperHero?> GetSuperHeroByNameAsync(string heroName);
        Task<List<SuperHero>> AddSuperHeroAsync(SuperHero hero);
        Task<List<SuperHero>?> UpdateSuperHeroAsync(int id, SuperHero hero);

        Task<List<SuperHero>?> UpdateSuperHeroPartiallyAsync(int id, JsonPatchDocument<SuperHero> hero);
        Task<List<SuperHero>?> DeleteSuperHeroAsync(int id);
    }
}
