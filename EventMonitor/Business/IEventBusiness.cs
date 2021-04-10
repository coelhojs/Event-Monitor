using EventMonitor.ViewObjects;
using System.Collections.Generic;

namespace EventMonitor.Business
{
    public interface IEventBusiness
    {
        List<EventVO> GetEvents(EventVO filter);
        void ProcessEvent(EventVO newEvent);
        void ValidateEvent(EventVO newEvent);
    }
}