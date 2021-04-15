using EventMonitor.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventMonitor.Hubs
{
    public class EventHub : Hub
    {
        private readonly IEventsAggregator _eventsAggregator;
        private readonly IEventBusiness _eventBusiness;
        private readonly ILogger<EventHub> _logger;

        public CancellationTokenSource AggregatorCTS { get; set; }
        public Task AggregatorTask { get; set; }

        public EventHub(ILogger<EventHub> logger, IEventBusiness eventBusiness, IEventsAggregator eventsAggregator)
        {
            _eventsAggregator = eventsAggregator;
            _eventBusiness = eventBusiness;
            _logger = logger;
        }

        public async void Update()
        {
            try
            {
                _eventsAggregator.AggregateStats();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro no envio de mensagens para os clientes: {ex}");
            }
        }

        public async void UpdateHistogram()
        {
            try
            {
                var stats = _eventBusiness.GetEventsStats();
                var histogramData = _eventBusiness.GetHistogramData(stats, "erro");
                await Clients.All.SendAsync(WebSocketActions.UPDATEERRORHISTOGRAM, histogramData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro no envio de mensagens para os clientes: {ex}");
            }
        }

        public async void UpdateChart()
        {
            try
            {
                var stats = _eventBusiness.GetEventsStats();
                var histogramData = _eventBusiness.GetHistogramData(stats, "erro");
                await Clients.All.SendAsync(WebSocketActions.UPDATEPROCESSEDHISTOGRAM, histogramData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro no envio de mensagens para os clientes: {ex}");
            }
        }

        public async void UpdateStats()
        {
            try
            {
                var stats = _eventBusiness.GetEventsStats();
                await Clients.All.SendAsync(WebSocketActions.UPDATE, stats);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro no envio de mensagens para os clientes: {ex}");
            }
        }

        public async Task Stop()
        {
            try
            {
                AggregatorCTS.Cancel();

                await AggregatorTask;
            }
            finally
            {
                AggregatorTask.Dispose();

                AggregatorTask = null;
            }

            await Clients.All.SendAsync(WebSocketActions.STOP);

        }
    }

    public struct WebSocketActions
    {
        public static readonly string UPDATE = "updateEvents";
        public static readonly string UPDATEPROCESSEDHISTOGRAM = "updateProcessedHistogram";
        public static readonly string UPDATEERRORHISTOGRAM = "updateErrorHistogram";
        public static readonly string START = "startMonitor";
        public static readonly string STOP = "stopMonitor";
    }
}
