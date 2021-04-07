using EventMonitor.Entities;
using EventMonitor.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventMonitor.DAO
{
    public class EventDAO
    {
        public List<Event> GetByTag(string tag)
        {
            using (var context = new Context())
            {
                var events = context.Event.Where(m => m.Tag == tag).ToList();

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

        public virtual EventVO FromEntityToVO(Event entity)
        {
            return new EventVO
            {
                Tag = entity.Tag,
                Timestamp = new DateTimeOffset(entity.Timestamp).ToUnixTimeSeconds(),
                Value = entity.Value
            };
        }

        public virtual Event FromVOToEntity(EventVO vo, Event entity = null)
        {
            return new Event
            {
                Tag = vo.Tag,
                Timestamp = new DateTime(1970, 1, 1).AddMilliseconds(vo.Timestamp),
                Value = vo.Value
            };
        }

    }
}