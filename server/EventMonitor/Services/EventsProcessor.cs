using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace EventMonitor.Services
{
    public class EventsProcessor : IEventsProcessor
    {
        private readonly ILogger<EventsProcessor> _logger;
        private readonly IEventBusiness _eventBusiness;
        private readonly ConcurrentQueue<RawEventVO> _concurrentQueue;

        public EventsProcessor(ILogger<EventsProcessor> logger, IEventBusiness eventBusiness)
        {
            _concurrentQueue = new ConcurrentQueue<RawEventVO>();
            _eventBusiness = eventBusiness;
            _logger = logger;

            Task.Run(Process);
        }

        public void Enqueue(RawEventVO newEvent)
        {
           _concurrentQueue.Enqueue(newEvent);
        }

        public async Task Process()
        {
            try
            {
                while (true)
                {
                    if (_concurrentQueue.TryDequeue(out RawEventVO rawEvent))
                    {
                        await _eventBusiness.ProcessEvent(rawEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Houve um erro no processamento da fila de eventos a serem registrados.", ex);
            }
        }
    }
}
