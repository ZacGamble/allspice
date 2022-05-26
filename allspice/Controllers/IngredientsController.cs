using Microsoft.AspNetCore.Mvc;
using allspice.Services;
using allspice.Models;
using System.Collections.Generic;

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
        [HttpGet]
        public ActionResult<List<Ingredient>> Get()
        {
            try
            {
                List<Ingredient> ingredients = _ingServ.Get();
                return Ok(ingredients);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST

        // PUT

        // DELETE

    }
}