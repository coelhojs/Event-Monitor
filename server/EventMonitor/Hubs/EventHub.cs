using EventMonitor.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace EventMonitor.Hubs
{
    public class EventHub : Hub
    {
        private readonly IEventBusiness _eventBusiness;

        public CancellationTokenSource AggregatorCTS { get; set; }
        public Task AggregatorTask { get; set; }

        public EventHub(IEventBusiness eventBusiness)
        {
            _eventBusiness = eventBusiness;
        }

        public async void Update()
        {
            var stats = _eventBusiness.GetEventsStats();
            var chartData = _eventBusiness.GetChartData(stats);

            await Clients.All.SendAsync(WebSocketActions.UPDATE, stats);
            await Clients.All.SendAsync(WebSocketActions.UPDATECHART, chartData);
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
        public static readonly string UPDATECHART = "updateChart";
        public static readonly string START = "startMonitor";
        public static readonly string STOP = "stopMonitor";
    }
}
