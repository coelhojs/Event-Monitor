using EventMonitor.DAO;
using EventMonitor.ViewObjects;
using System;
using System.Collections.Generic;

namespace EventMonitor.Business
{
    public class EventBusiness : IEventBusiness
    {
        private EventDAO _eventDAO;

        public EventBusiness(EventDAO eventDAO = null)
        {
            _eventDAO = eventDAO ?? new EventDAO();
        }

        public List<EventVO> GetEvents(EventVO filter)
        {
            return _eventDAO.Get(filter);
        }

        public void ProcessEvent(EventVO newEvent)
        {
            ValidateEvent(newEvent);

            _eventDAO.Save(newEvent);
        }

        public void ValidateEvent(EventVO newEvent)
        {
            if (string.IsNullOrWhiteSpace(newEvent.Tag) || newEvent.Tag.Contains(" ") || string.IsNullOrWhiteSpace(newEvent.Value))
            {
                throw new Exception("O evento recebido possui um ou mais valores inválidos.");
            }
        }
    }
}
