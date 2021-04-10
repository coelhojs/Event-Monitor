using EventMonitor.Business;
using EventMonitor.ViewObjects;
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
        private readonly IEventBusiness _eventBusiness;

        public EventController(IEventBusiness eventBusiness)
        {
            _eventBusiness = eventBusiness;
        }

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

        [HttpGet]
        public ActionResult<List<EventVO>> GetEvents([FromQuery] EventVO filter)
        {
            try
            {
                var events = _eventBusiness.GetEvents(filter);

                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);

            }
        }
    }
}
