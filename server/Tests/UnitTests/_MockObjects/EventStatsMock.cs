using EventMonitor.ViewObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests._MockObjects
{
    public class EventStatsMock : BaseMock
    {
        private EventMock _eventMock;
        private RawEventMock _rawEventMock;

        public EventStatsMock()
        {
            _eventMock = new EventMock();
        }

        public List<EventStatsVO> MockEventStatsList(int v)
        {
            var events = _eventMock.MockEventList(5);

            var stats = events.GroupBy(ev => new { ev.Region, ev.Sensor })
                        .Select(group => new EventStatsVO
                        {
                            Counter = group.Count(),
                            Region = group.Key.Region,
                            Sensor = group.Key.Sensor,
                            Status = statuses[new Random().Next(0, statuses.Length)]
                        })
                        .ToList();

            return stats;
        }

        public List<EventStatsVO> MockSummarizedList(List<EventStatsVO> stats)
        {
            var summarizedStats = stats.GroupBy(stat => stat.Region)
                .Select(group => new EventStatsVO
                {
                    Counter = group.Sum(item => item.Counter),
                    Region = group.Key
                });

            stats.AddRange(summarizedStats);

            return stats
                .OrderBy(item => item.Region)
                .ThenBy(item => item.Sensor)
                .ToList();
        }
    }
}
