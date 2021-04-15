using EventMonitor.Entities;
using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMonitor.DAO
{
    public class EventDAO : IEventDAO
    {
        private readonly ILogger<EventDAO> _logger;

        public EventDAO(ILogger<EventDAO> logger)
        {
            _logger = logger;
        }

        public List<EventStatsVO> GetStats()
        {
            using (var context = new Context())
            {
                var stats = context.Set<Event>()
                    .GroupBy(ev => new { ev.Region, ev.Sensor })
                        .Select(group => new EventStatsVO
                        {
                            Counter = group.Count(),
                            Region = group.Key.Region,
                            Sensor = group.Key.Sensor
                        })
                        .ToList();

                foreach (var stat in stats)
                {
                    (stat.Errors, stat.Processed) = CountEventsStatuses(context, stat.Region, stat.Sensor);
                    stat.Status = GetLatestEventStatus(context, stat.Region, stat.Sensor);
                }

                return stats;
            }
        }

        public List<EventVO> GetTagsHistory(int timeWindow)
        {
            _logger.LogDebug("Definindo a data hora minima para filtrar os dados historicos.");

            var startingTime = DateTime.Now.AddHours(-timeWindow);

            using (var context = new Context())
            {
                var history = context.Set<Event>()
                    .Where(ev => string.IsNullOrEmpty(ev.Value) == false && ev.Timestamp > startingTime)
                    .ToList();

                return history.Select(ev => FromEventToVO(ev)).ToList();
            }
        }

        private (long, long) CountEventsStatuses(Context context, string region, string sensor)
        {
            var errors = context.Set<Event>().Count(item => item.Region == region && item.Sensor == sensor && string.IsNullOrEmpty(item.Value));
            var processed = context.Set<Event>().Count(item => item.Region == region && item.Sensor == sensor && !string.IsNullOrEmpty(item.Value));

            return (errors, processed);
        }

        private string GetLatestEventStatus(Context context, string region, string sensor)
        {
            var value = context.Set<Event>().OrderByDescending(ev => ev.Id)
                       .FirstOrDefault(ev => ev.Region.Equals(region) && ev.Sensor.Equals(sensor)).Value;

            return string.IsNullOrEmpty(value) ? "erro" : "processado";

        }

        public async Task Save(EventVO data)
        {
            using (var context = new Context())
            {
                var entity = FromVOToEvent(data);

                await context.Event.AddAsync(entity);

                await context.SaveChangesAsync();
            }
        }

        public EventVO FromEventToVO(Event entity)
        {
            return new EventVO
            {
                Region = entity.Region,
                Sensor = entity.Sensor,
                Timestamp = ((DateTimeOffset)entity.Timestamp).ToUnixTimeSeconds(),
                Value = entity.Value
            };
        }

        public Event FromVOToEvent(EventVO vo, Event entity = null)
        {
            return new Event
            {
                Region = vo.Region,
                Sensor = vo.Sensor,
                Timestamp = new DateTime(1970, 1, 1).AddMilliseconds(vo.Timestamp),
                Value = vo.Value
            };
        }

    }
}