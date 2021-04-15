using EventMonitor.ViewObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventMonitor.Interfaces
{
    public interface IEventBusiness
    {
        List<EventStatsVO> GetEventsStats();
        EventVO ParseEvent(RawEventVO newEvent);
        Task ProcessEvent(RawEventVO newEvent);
        List<ChartDataVO> GetChartData();
        List<HistogramDataVO> GetHistogramData(List<EventStatsVO> stats, string status);
    }
}