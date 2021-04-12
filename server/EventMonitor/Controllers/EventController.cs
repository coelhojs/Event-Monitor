using EventMonitor.Hubs;
using EventMonitor.Interfaces;
using EventMonitor.Services;
using EventMonitor.ViewObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace EventMonitor.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventBusiness _eventBusiness;
        private readonly IHubContext<EventHub> _eventHub;

        private readonly EventsAggregator _eventsAggregator;
        private readonly EventsProcessor _eventsProcessor;

        public EventController(IEventBusiness eventBusiness, IHubContext<EventHub> eventHub)
        {
            _eventBusiness = eventBusiness;
            _eventHub = eventHub;
            _eventsAggregator = new EventsAggregator(_eventBusiness, _eventHub);
            _eventsProcessor = new EventsProcessor(_eventBusiness);
        }

        [HttpPost]
        public IActionResult NewEvent([FromBody] RawEventVO data)
        {
            try
            {
                Task.Run(() => _eventsProcessor.Enqueue(data));
                return Accepted();
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());

            }
        }

        //[HttpGet]
        //public IActionResult GetEvents([FromQuery] RawEventVO filter)
        //{
        //    try
        //    {
        //        var events = _eventBusiness.GetEvents(filter);

        //        return Ok(events);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem(ex.ToString());
        //    }
        //}

        [HttpGet("GetStats")]
        public IActionResult GetStats()
        {
            try
            {
                var events = _eventBusiness.GetEventsStats();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
        }

        [HttpGet("StartAggregator")]
        public IActionResult StartAggregator()
        {
            try
            {
                if (_eventsAggregator.AggregatorTask == null || _eventsAggregator.AggregatorTask.Status.Equals(TaskStatus.Running) == false)
                {
                    _eventsAggregator.StartAggregator();

                    var message = "Agregador automático iniciado.";

                    return Ok(message);
                }
                else
                {
                    var message = "O agregador automático já está ativo.";

                    return Conflict(message);
                }
            }
            catch (Exception ex)
            {
                var message = $"Houve um erro na inicialização do agregador automático: {ex.Message}";

                return Problem(message);
            }
        }

        [HttpGet("StopAggregator")]
        public IActionResult StopAggregator()
        {
            try
            {
                if (_eventsAggregator.AggregatorTask == null || _eventsAggregator.AggregatorTask.Status.Equals(TaskStatus.Running) == false)
                {
                    _eventsAggregator.StopAggregator();

                    string message = "O agregador automático foi interrompido com sucesso.";

                    return Ok(message);
                }
                else
                {
                    var message = "O agregador automático já está parado.";

                    return Conflict(message);
                }
            }
            catch (Exception e)
            {
                var message = $"Houve um erro na interrupção do agregador automático: {e.Message}";

                return Problem(message);
            }
        }
    }
}
