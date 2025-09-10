using System.Xml.Linq;
using Azure;
using CoreWebApiSuperHero.Models;
using CoreWebApiSuperHero.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace CoreWebApiSuperHero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly ISuperHeroService _superHeroService ;

        public SuperHeroController(ISuperHeroService superHeroService) 
        {
            this._superHeroService = superHeroService;
        }

        #region Action Methods

        #region GET 

        [HttpGet]
        [Route("All",Name = "GetSuperHeros")] // rouute attribute is used to specify the route for this action method. Name is used to give a name to the route
        [ProducesResponseType(StatusCodes.Status200OK)] // this is used to specify the response type for this action method
        public async Task<ActionResult<List<SuperHero>>> GetSuperHeroes()
        {
            return Ok(await _superHeroService.GetSuperHeroesAsync());
        }


        [HttpGet("{id:int}",Name = "GetSuperHeroById")] // this is used to get a SuperHero by ID and the id must be an integer
        [ProducesResponseType(StatusCodes.Status200OK)] // this is used to specify the response type for this action method
        [ProducesResponseType(StatusCodes.Status404NotFound)] // this is used to specify the response type for this action method when not found
        public async Task<ActionResult<SuperHero>> GetSuperHeroById(int id)
        {
            SuperHero? superHero = await _superHeroService.GetSuperHeroByIdAsync(id);
            if (superHero == null)
            {
                return NotFound($"SuperHero with ID {id} not found.");
            }
            return Ok(superHero);
        }

        [HttpGet("{name:alpha}")] // this is used to get a SuperHero by ID and the id must be an integer
        [ProducesResponseType(StatusCodes.Status200OK)] // this is used to specify the response type for this action method
        [ProducesResponseType(StatusCodes.Status404NotFound)] // this is used to specify the response type for this action method when not found
        public async Task<ActionResult<SuperHero>> GetSuperHeroByName(string name)
        {
            SuperHero? superHero = await _superHeroService.GetSuperHeroByNameAsync(name);
            if (superHero == null)
            {
                return NotFound($"SuperHero with name {name} not found.");
            }
            return Ok(superHero);
        }

        #endregion

        #region POST

        [HttpPost] // this is used to create a new SuperHero
        public async Task<ActionResult<List<SuperHero>>> AddSuperHero(SuperHero hero)
        {
            return Ok(await _superHeroService.AddSuperHeroAsync(hero));
        }

        #endregion

        #region PUT

        [HttpPut("{id}")]// this is used to update an existing SuperHero
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(int id, SuperHero hero)
        {
           var heroes = await _superHeroService.UpdateSuperHeroAsync(id,hero);

            if (heroes == null)
            {
                return NotFound($"SuperHero with ID {id} not found.");
            }
           return Ok(heroes);
           // return  CreatedAtRoute("GetSuperHeroById", new { id = hero.Id }, heroes); // this will return the updated SuperHero with a 201 Created status code       
        }

        #endregion

        #region PATCH


        [HttpPatch("{id}")]// this is used to update an existing SuperHero
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHeroPartially(int id, [FromBody] JsonPatchDocument<SuperHero> pachDocument) // this is used to update an existing SuperHero partially using JSON Patch Document. json patch document is used to update only the properties that are specified in the document
        {
            var heroes = await _superHeroService.UpdateSuperHeroPartiallyAsync(id, pachDocument);

            if (heroes == null)
            {
                return NotFound($"SuperHero with ID {id} not found.");
            }
            return Ok(heroes);
            // return  CreatedAtRoute("GetSuperHeroById", new { id = hero.Id }, heroes); // this will return the updated SuperHero with a 201 Created status code       
        }

        #endregion

        #region DELETE

        [HttpDelete("{id}")]    // this is used to delete an existing SuperHero
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int id)
        {
            var heroes = await _superHeroService.DeleteSuperHeroAsync(id);

            if (heroes == null)
            {
                return NotFound($"SuperHero with ID {id} not found.");
            }
            return Ok(heroes);
        }

        #endregion

        #endregion

    }
}
