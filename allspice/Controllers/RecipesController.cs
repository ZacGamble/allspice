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
        private readonly IngredientsService _ingredientServ;
        private readonly StepsService _stepServ;

        public RecipesController(RecipesService recServ, IngredientsService ingredientServ, StepsService stepServ)
        {
            _recServ = recServ;
            _ingredientServ = ingredientServ;
            _stepServ = stepServ;
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

        // GET INGREDIENTS BY RECIPE
        [HttpGet("{id}/ingredients")]
        public ActionResult<List<Ingredient>> GetIngredients(int id)
        {
            try
            {
                List<Ingredient> ingredients = _ingredientServ.GetIngredients(id);
                return Ok(ingredients);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // GET step BY RECIPE
        [HttpGet("{id}/steps")]
        public ActionResult<List<Step>> GetSteps(int id)
        {
            try
            {
                List<Step> step = _stepServ.GetStepsByRecipe(id);
                return Ok(step);
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
                recipe.Creator = userInfo;
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
                return BadRequest(e.Message);
            }
        }
    }
}