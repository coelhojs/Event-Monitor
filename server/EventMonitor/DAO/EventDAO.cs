using EventMonitor.Entities;
using EventMonitor.ViewObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventMonitor.DAO
{
    public class EventDAO
    {
        public List<EventVO> Get(RawEventVO filter)
        {
            List<Event> events = null;

            using (var context = new Context())
            {
                if (filter != null)
                {
                    //TODO: Usar filtro
                    events = context.Event.ToList();
                }
                else
                {
                    events = context.Event.ToList();
                }

                return events.Select(ev => FromEntityToVO(ev)).ToList();
            }
        }

        public List<Event> GetByTag(string tag)
        {
            using (var context = new Context())
            {
                var events = context.Event.Where(m => m.Region == tag).ToList();

                return events;
            }
        }

        public void Save(EventVO data)
        {
            try
            {
                using (var context = new Context())
                {
                    var entity = FromVOToEntity(data);

                    context.Event.Add(entity);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //TODO: Tratar inclusão de dados duplicados
                if (ex.InnerException.Message.ToLower().Contains("duplicate key") || ex.InnerException.Message.ToLower().Contains("already exists"))
                {
                    return;
                }

                throw ex;
            }
        }

        public EventVO FromEntityToVO(Event entity)
        {
            return new EventVO
            {
                Region = entity.Region,
                Sensor = entity.Sensor,
                Timestamp = new DateTimeOffset(entity.Timestamp).ToUnixTimeSeconds(),
                Value = entity.Value
            };
        }

        public Event FromVOToEntity(EventVO vo, Event entity = null)
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