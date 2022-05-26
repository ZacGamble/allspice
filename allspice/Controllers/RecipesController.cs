using Microsoft.AspNetCore.Mvc;
using allspice.Services;
using allspice.Models;
using System.Collections.Generic;

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
                return Ok();
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}