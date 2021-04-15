using EventMonitor.ViewObjects;
using System;
using System.Collections.Generic;

namespace UnitTests._MockObjects
{
    public class EventMock : BaseMock
    {
        public EventVO MockEvent()
        {
            return new EventVO
            {
                Region = regions[new Random().Next(0, regions.Length)].Replace("brasil.", ""),
                Sensor = sensors[new Random().Next(0, sensors.Length)],
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Value = values[new Random().Next(0, values.Length)]
            };
        }

        public List<EventVO> MockEventList(int listSize)
        {
            var eventsList = new List<EventVO>();

            for (int i = 0; i < listSize; i++)
            {
                eventsList.Add(MockEvent());
            }

            return eventsList;
        }

    }
}
