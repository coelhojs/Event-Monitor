using EventMonitor.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EventMonitor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpPost]
        public ActionResult NewEvent([FromBody] Event data)
        {
            try
            {


                //TODO: Verificar e fazer um override ToString se necessário
                return Ok($"Evento {data} recebido");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);

            }
        }
    }
}
