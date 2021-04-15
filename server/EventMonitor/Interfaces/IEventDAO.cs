using EventMonitor.Entities;
using EventMonitor.ViewObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventMonitor.Interfaces
{
    public interface IEventDAO
    {
        EventVO FromEventToVO(Event entity);
        Event FromVOToEvent(EventVO vo, Event entity = null);
        List<EventStatsVO> GetStats();
        List<EventVO> GetTagsHistory(int timeWindow);
        Task Save(EventVO data);
    }
}