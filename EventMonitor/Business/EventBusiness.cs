using EventMonitor.DAO;
using EventMonitor.Model;
using System;

namespace EventMonitor.Business
{
    public class EventBusiness
    {
        private EventDAO _eventDAO;

        public EventBusiness(EventDAO eventDAO = null)
        {
            _eventDAO = eventDAO ?? new EventDAO();
        }

        public void ProcessEvent(EventVO newEvent)
        {
            ValidateEvent(newEvent);

            _eventDAO.Save(newEvent);
        }

        private void ValidateEvent(EventVO newEvent)
        {
            if (string.IsNullOrWhiteSpace(newEvent.Tag) || newEvent.Tag.Contains(" ") || string.IsNullOrWhiteSpace(newEvent.Value))
            {
                throw new Exception("O evento recebido possui um ou mais valores inválidos.");
            }
        }
    }
}
