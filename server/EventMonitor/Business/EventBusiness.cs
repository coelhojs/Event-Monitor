using EventMonitor.DAO;
using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventMonitor.Business
{
    public class EventBusiness : IEventBusiness
    {
        private EventDAO _eventDAO;

        public EventBusiness(EventDAO eventDAO = null)
        {
            _eventDAO = eventDAO ?? new EventDAO();
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
                            Region = group.Key.Region
                        })
                        .OrderBy(x => x.Region)
                        .ToList();

            return stats;
        }

        public void ProcessEvent(RawEventVO newEvent)
        {
            var parsedEvent = ParseEvent(newEvent);

            _eventDAO.Save(parsedEvent);
        }

        //
        public EventVO ParseEvent(RawEventVO newEvent)
        {
            if (string.IsNullOrWhiteSpace(newEvent.Tag) || newEvent.Tag.Contains(" "))
            {
                throw new Exception("O evento recebido possui um ou mais valores inválidos.");
            }

            var tagParts = newEvent.Tag.Split('.');

            return new EventVO
            {
                Region = $"{tagParts[0]}.{tagParts[1]}",
                Sensor = tagParts[2],
                Timestamp = newEvent.Timestamp,
                Value = newEvent.Value
            };
        }
    }
}
