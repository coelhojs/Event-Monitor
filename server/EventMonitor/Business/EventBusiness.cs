using EventMonitor.DAO;
using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMonitor.Business
{
    public class EventBusiness : IEventBusiness
    {
        private readonly ILogger<EventBusiness> _logger;

        private readonly IEventDAO _eventDAO;

        public EventBusiness(ILogger<EventBusiness> logger, IEventDAO eventDAO)
        {
            _logger = logger;

            _eventDAO = eventDAO;
        }

        public List<EventStatsVO> GetEventsStats()
        {
            _logger.LogDebug("Obtendo dados agregados dos eventos.");

            var stats = _eventDAO.GetStats();

            _logger.LogDebug("Obtendo dados agregados das regiões.");

            var summarizedStats = stats.GroupBy(stat => stat.Region)
                .Select(group => new EventStatsVO
                {
                    Counter = group.Sum(item => item.Counter),
                    Region = group.Key
                });

            stats.AddRange(summarizedStats);

            _logger.LogDebug("Ordenando dados por região e então por sensor.");

            return stats
                .OrderBy(item => item.Region)
                .ThenBy(item => item.Sensor)
                .ToList();
        }

        public async Task ProcessEvent(RawEventVO newEvent)
        {
            _logger.LogDebug($"Processando novo evento: {newEvent.Tag}");

            var parsedEvent = ParseEvent(newEvent);

            await _eventDAO.Save(parsedEvent);
        }

        public EventVO ParseEvent(RawEventVO newEvent)
        {
            if (string.IsNullOrWhiteSpace(newEvent.Tag))
            {
                throw new FormatException("O evento recebido possui um ou mais valores inválidos.");
            }

            var tagParts = newEvent.Tag.Split('.');

            if (tagParts.Length < 3)
            {
                throw new FormatException($"Formato inválido de tag: {newEvent.Tag}");
            }

            return new EventVO
            {
                Region = $"{tagParts[0]}.{tagParts[1]}",
                Sensor = tagParts[2],
                Timestamp = newEvent.Timestamp,
                Value = newEvent.Value
            };
        }

        public List<ChartDataVO> GetChartData(List<EventStatsVO> stats)
        {
            var regions = new string[] { "brasil.nordeste", "brasil.norte", "brasil.sudeste", "brasil.sul" };
            var chartData = new List<ChartDataVO>();

            var orderedStats = stats
                .Where(item => item.Sensor != null)
                .OrderBy(item => item.Sensor)
                .ToList();

            var sensors = orderedStats.Select(item => item.Sensor).ToHashSet<string>();

            foreach (var item in sensors)
            {
                var north = orderedStats.FirstOrDefault(data => data.Region == regions[0] && data.Sensor == item)?.Counter;
                var northeast = orderedStats.FirstOrDefault(data => data.Region == regions[1] && data.Sensor == item)?.Counter;
                var southeast = orderedStats.FirstOrDefault(data => data.Region == regions[2] && data.Sensor == item)?.Counter;
                var south = orderedStats.FirstOrDefault(data => data.Region == regions[3] && data.Sensor == item)?.Counter;

                chartData.Add(new ChartDataVO
                {
                    Name = item,
                    Data = new long[]{
                        north ?? 0,
                        northeast ?? 0,
                        southeast ?? 0,
                        south ?? 0
                    }
                });
            }

            return chartData;
        }
    }
}
