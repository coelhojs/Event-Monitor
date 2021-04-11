using EventMonitor.ViewObjects;
using System.Collections.Generic;

namespace EventMonitor.Interfaces
{
    public interface IEventBusiness
    {
        List<EventVO> GetEvents(RawEventVO filter);
        List<EventStatsVO> GetEventsStats();
        EventVO ParseEvent(RawEventVO newEvent);
        void ProcessEvent(RawEventVO newEvent);
    }
}