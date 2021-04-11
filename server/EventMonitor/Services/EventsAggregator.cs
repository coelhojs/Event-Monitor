using EventMonitor.Hubs;
using EventMonitor.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventMonitor.Services
{
    public class EventsAggregator : IEventsAggregator
    {
        public CancellationTokenSource AggregatorCTS { get; set; }
        public Task AggregatorTask { get; set; }

        private IEventBusiness _eventBusiness;
        private IHubContext<EventHub> _eventHub;

        public EventsAggregator(IEventBusiness eventBusiness, IHubContext<EventHub> eventHub)
        {
            _eventBusiness = eventBusiness;
            _eventHub = eventHub;

            StartAggregator();
        }

        public async void StartAggregator()
        {
            AggregatorCTS = new CancellationTokenSource();

            AggregatorTask = new Task(() => AggregateStats(), AggregatorCTS.Token);
            AggregatorTask.Start();
        }

        public async void StopAggregator()
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
        }

        public async void AggregateStats()
        {
            while (AggregatorCTS.IsCancellationRequested == false)
            {
                var stats = _eventBusiness.GetEventsStats();

                await _eventHub.Clients.All.SendAsync("updateEvents", stats, AggregatorCTS);

                await Task.Delay(int.Parse(Environment.GetEnvironmentVariable("UpdateIntervalMs")));
            }
        }
    }
}