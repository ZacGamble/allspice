using Microsoft.AspNetCore.Mvc;
using allspice.Services;

namespace allspice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientsService _ingServ;

        public IngredientsController(IngredientsService ingServ)
        {
            _ingServ = ingServ;
        }
        // GET
        // [HttpGet("{recipeId}")]
        // public ActionResult<List<Ingredient>> Get(int recipeId)
        // {
        //     try
        //     {
        //         List<Ingredient> ingredients = _ingServ.Get(recipeId);
        //         return Ok(ingredients);
        //     }
        //     catch (System.Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

        // POST

        // PUT

        // DELETE

    }
}