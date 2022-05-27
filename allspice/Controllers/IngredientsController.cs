using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using allspice.Services;
using allspice.Models;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;


namespace allspice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientsService _ingServ;
        private readonly RecipesService _recServ;

        public IngredientsController(IngredientsService ingServ, RecipesService recServ)
        {
            _ingServ = ingServ;
            _recServ = recServ;
        }
        // GETBYID
        [HttpGet]
        public ActionResult<Ingredient> Get(int id)
        {
            try
            {
                Ingredient ingredient = _ingServ.Get(id);
                return Ok(ingredient);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Ingredient>> Create([FromBody] Ingredient ingredientData)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                ingredientData.CreatorId = userInfo.Id;
                Ingredient newIngredient = _ingServ.Create(ingredientData);
                return Ok(newIngredient);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // PUT

        // DELETE
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<string>> Delete(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                _ingServ.Delete(id, userInfo.Id);
                return Ok("Ingredient Deleted");
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}