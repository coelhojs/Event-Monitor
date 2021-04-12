using EventMonitor.DAO;
using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMonitor.Business
{
    public class EventBusiness : IEventBusiness
    {
        private EventDAO _eventDAO;
        private Dictionary<string, string> _status;

        public EventBusiness(EventDAO eventDAO = null)
        {
            _eventDAO = eventDAO ?? new EventDAO();
            _status = new Dictionary<string, string>();
        }

        public List<EventVO> GetEvents(RawEventVO filter = null)
        {
            return _eventDAO.Get(filter);
        }

        public List<EventStatsVO> GetEventsStats()
        {
            var events = GetEvents();

            var stats = AggregateEvents(events);

            return stats;
        }

        private List<EventStatsVO> AggregateEvents(List<EventVO> events)
        {
            var stats = events.GroupBy(ev => new { ev.Region, ev.Sensor })
                        .Select(group => new EventStatsVO
                        {
                            Counter = group.Count(),
                            Sensor = group.Key.Sensor,
                            Region = group.Key.Region,
                            Status = _status.GetValueOrDefault($"{group.Key.Region}.{group.Key.Sensor}")
                        })
                        .OrderBy(x => x.Region)
                        .ToList();

            var summarizedStats = stats.GroupBy(stat => stat.Region)
                .Select(group => new EventStatsVO
                {
                    Counter = group.Sum(item => item.Counter),
                    Region = group.Key
                });

            stats.AddRange(summarizedStats);

            return stats
                .OrderBy(item => item.Sensor)
                .OrderBy(item => item.Region)
                .ToList();
        }

        public async Task ProcessEvent(RawEventVO newEvent)
        {
            try
            {
                var parsedEvent = ParseEvent(newEvent);

                _eventDAO.Save(parsedEvent);

                if (string.IsNullOrEmpty(newEvent.Value))
                {
                    _status.Add(newEvent.Tag, "erro");
                }
                else
                {

                    _status.Add(newEvent.Tag, "processado");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public EventVO ParseEvent(RawEventVO newEvent)
        {
            //TODO: Validar se a tag tem 3 (brasil.sudeste.sensor01), se não tiver rejeitar evento

            if (string.IsNullOrWhiteSpace(newEvent.Tag))
            {
                throw new Exception("O evento recebido possui um ou mais valores inválidos.");
            }

            var tagParts = newEvent.Tag.Split('.');

            if (tagParts.Length < 3)
            {
                throw new Exception($"Tag inválida: {newEvent.Tag}");
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
