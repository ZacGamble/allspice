using allspice.Services;
using Microsoft.AspNetCore.Mvc;

namespace allspice.Controllers
{
    public class StepsController : ControllerBase
    {
        private readonly StepsService _stepServ;
        public StepsController(StepsService stepServ)
        {
            _stepServ = stepServ;
        }

        //POST

        //EDIT

        //DELETE
    }
}