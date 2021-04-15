using EventMonitor.Entities;
using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventMonitor.DAO
{
    public class EventDAO : IEventDAO
    {
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
                    stat.Errors = CountEventsErrors(context, stat.Region, stat.Sensor);
                    stat.Status = GetLatestEventStatus(context, stat.Region, stat.Sensor);
                }

                return stats;
            }
        }

        private long CountEventsErrors(Context context, string region, string sensor)
        {
            return context.Set<Event>().Count(item => item.Region == region && item.Sensor == sensor && string.IsNullOrEmpty(item.Value));
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
                Timestamp = new DateTimeOffset(entity.Timestamp).ToUnixTimeSeconds(),
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