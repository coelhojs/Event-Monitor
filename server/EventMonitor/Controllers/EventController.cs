using EventMonitor.Interfaces;
using EventMonitor.Services;
using EventMonitor.ViewObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EventMonitor.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;

        private readonly IEventsAggregator _eventsAggregator;
        private readonly IEventBusiness _eventBusiness;
        private readonly IEventsProcessor _eventsProcessor;

        private readonly string baseErrorMsg = "Contate o administrador do sistema.";

        public EventController(ILogger<EventController> logger, IEventBusiness eventBusiness, IEventsAggregator eventsAggregator, IEventsProcessor eventsProcessor)
        {
            _eventsAggregator = eventsAggregator;
            _eventBusiness = eventBusiness;
            _eventsProcessor = eventsProcessor;
            _logger = logger;

        }

        [HttpPost]
        public IActionResult NewEvent([FromBody] RawEventVO data)
        {
            try
            {
                var start = DateTime.Now;

                Task.Run(() => _eventsProcessor.Enqueue(data));

                _logger.LogDebug($"Tempo gasto: {(DateTime.Now - start).TotalSeconds}");

                return Accepted();
            }
            catch (FormatException ex)
            {
                var message = $"O evento possui dados inválidos: {ex.Message}";

                _logger.LogError(message, ex);

                return Problem(message);
            }
            catch (Exception ex)
            {
                return LogAndReturnError($"Erro ao processar o evento: {data}", ex);
            }
        }

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
                return LogAndReturnError("Houve um erro na requisição das estatísticas de eventos.", ex);
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

                    _logger.LogInformation(message);

                    return Ok(message);
                }
                else
                {
                    var message = "O agregador automático já está ativo.";

                    _logger.LogInformation(message);

                    return Conflict(message);
                }
            }
            catch (Exception ex)
            {
                return LogAndReturnError("Houve um erro na inicialização do agregador automático.", ex);
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

                    _logger.LogInformation(message);

                    return Ok(message);
                }
                else
                {
                    var message = "O agregador automático já está parado.";

                    _logger.LogInformation(message);

                    return Conflict(message);
                }
            }
            catch (Exception ex)
            {
                return LogAndReturnError("Houve um erro na interrupção do agregador automático", ex);
            }
        }

        private IActionResult LogAndReturnError(string msg, Exception ex)
        {
            _logger.LogError(msg, ex);

            return Problem($"{msg} {baseErrorMsg}");
        }
    }
}
