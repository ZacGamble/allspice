using System.Threading.Tasks;
using allspice.Services;
using allspice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeWorks.Auth0Provider;

namespace allspice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepsController : ControllerBase
    {
        private readonly StepsService _stepServ;
        public StepsController(StepsService stepServ)
        {
            _stepServ = stepServ;
        }
        //GetStep
        [HttpGet]
        public ActionResult<Step> GetById(int id)
        {
            try
            {
                Step step = _stepServ.GetById(id);
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
        public async Task<ActionResult<Step>> Create([FromBody] Step stepData)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                stepData.CreatorId = userInfo.Id;
                Step newStep = _stepServ.Create(stepData);
                return Ok(newStep);
            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        //EDIT
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Step>> Edit(int id, [FromBody] Step stepData)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                stepData.CreatorId = userInfo.Id;
                stepData.Id = id;
                Step step = _stepServ.Edit(stepData);
                return Ok(step);

            }
            catch (System.Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        //DELETE
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<string>> Delete(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                _stepServ.Delete(id, userInfo.Id);
                return Ok("Step Deleted");
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}