using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Event_Simulator
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private MockObjects _mockObjects;
        private Simulator _simulator;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _mockObjects = new MockObjects();
            _simulator = new Simulator();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var region in _mockObjects.regions)
                {
                    foreach (var sensor in _mockObjects.sensors)
                    {
                        var content = _mockObjects.GetEvent(region, sensor);

                        _simulator.Client.PostAsync($"{_simulator.AppUrl}/Event", content);
                    }
                }

                _logger.LogInformation("Simulando eventos para todos os sensores: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
