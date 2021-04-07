using EventMonitor.Business;
using EventMonitor.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventMonitor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        public static EventVO _event = new EventVO();

        //TODO: Usar Injeção de dependencia no Startup
        private EventBusiness _eventBusiness = new EventBusiness();

        [HttpPost]
        public ActionResult NewEvent([FromBody] EventVO data)
        {
            try
            {
                _eventBusiness.ProcessEvent(data);

                //TODO: Override ToString
                return Ok($"Evento {data} recebido");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);

            }
        }
    }
}
