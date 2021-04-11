using EventMonitor.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventMonitor.Hubs
{
    public class EventHub : Hub
    {
        private IEventBusiness _eventBusiness;

        public CancellationTokenSource AggregatorCTS { get; set; }
        public Task AggregatorTask { get; set; }

        public EventHub(IEventBusiness eventBusiness)
        {
            _eventBusiness = eventBusiness;
        }

        public async Task Start()
        {
            var stats = _eventBusiness.GetEventsStats();

            await Clients.All.SendAsync(WebSocketActions.START, stats);
        }

        public async void Update()
        {
            while (AggregatorCTS.IsCancellationRequested == false)
            {
                var stats = _eventBusiness.GetEventsStats();

                await Clients.All.SendAsync(WebSocketActions.UPDATE, stats);

                await Task.Delay(int.Parse(Environment.GetEnvironmentVariable("UpdateIntervalMs")));
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
        public static readonly string START = "startMonitor";
        public static readonly string STOP = "stopMonitor";
    }
}
