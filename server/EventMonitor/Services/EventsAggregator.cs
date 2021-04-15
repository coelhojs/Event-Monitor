using EventMonitor.Hubs;
using EventMonitor.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventMonitor.Services
{
    public class EventsAggregator : IEventsAggregator
    {
        public CancellationTokenSource AggregatorCTS { get; set; }
        public Task AggregatorTask { get; set; }

        private readonly IEventBusiness _eventBusiness;
        private readonly IHubContext<EventHub> _eventHub;
        private readonly ILogger<EventsAggregator> _logger;

        public EventsAggregator(ILogger<EventsAggregator> logger, IHubContext<EventHub> eventHub, IEventBusiness eventBusiness)
        {
            _eventBusiness = eventBusiness;
            _eventHub = eventHub;
            _logger = logger;

            StartAggregator();
        }

        public void StartAggregator()
        {
            _logger.LogInformation("Iniciando agregador automático de estatísticas de eventos");

            AggregatorCTS = new CancellationTokenSource();

            AggregatorTask = new Task(() => AggregateStats(), AggregatorCTS.Token);

            AggregatorTask.Start();
        }

        public async void StopAggregator()
        {
            try
            {
                _logger.LogInformation("Interrompendo agregador automático.");

                AggregatorCTS.Cancel();

                await AggregatorTask;

            }
            finally
            {
                AggregatorTask.Dispose();

                AggregatorTask = null;
            }
        }

        public async void AggregateStats()
        {
            try
            {
                while (AggregatorCTS.IsCancellationRequested == false)
                {
                    _logger.LogDebug("Enviando dados atualizados para a tabela e o gráfico de eventos.");

                    var stats = _eventBusiness.GetEventsStats();
                    await _eventHub.Clients.All.SendAsync("updateEvents", stats, AggregatorCTS);

                    var histogramData = _eventBusiness.GetHistogramData(stats);
                    await _eventHub.Clients.All.SendAsync("updateHistogram", histogramData, AggregatorCTS);

                    var chartData = _eventBusiness.GetChartData();
                    await _eventHub.Clients.All.SendAsync("updateChart", chartData, AggregatorCTS);

                    await Task.Delay(int.Parse(Environment.GetEnvironmentVariable("UPDATE_INTERVALMS")));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro no agregador de estatísticas. Será feita uma tentativa de reinicialização daqui 10 segundos." + '\n' + ex);

                await Task.Delay(10000);

                StartAggregator();
            }
        }
    }
}