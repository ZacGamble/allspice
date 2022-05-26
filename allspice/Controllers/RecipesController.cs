using Microsoft.AspNetCore.Mvc;
using allspice.Services;
using allspice.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using CodeWorks.Auth0Provider;

namespace allspice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly RecipesService _recServ;

        public RecipesController(RecipesService recServ)
        {
            _recServ = recServ;
        }
        [HttpGet]
        public ActionResult<List<Recipe>> Get()
        {

            try
            {
                List<Recipe> recipe = _recServ.Get();
                return Ok(recipe);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //GetByID
        [HttpGet("{id}")]
        public ActionResult<Recipe> Get(int id)
        {

            try
            {
                Recipe recipe = _recServ.Get(id);
                return Ok(recipe);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //POST
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Recipe>> Create([FromBody] Recipe recipeData)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                recipeData.CreatorId = userInfo.Id;
                Recipe recipe = _recServ.Create(recipeData);
                recipe.CreatorId = userInfo.Id;
                return Ok(recipe);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<string>> Delete(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                _recServ.Delete(id, userInfo.Id);
                return Ok("Deleted the recipe.");
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}