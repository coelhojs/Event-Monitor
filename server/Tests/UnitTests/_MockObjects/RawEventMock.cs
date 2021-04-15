using EventMonitor.ViewObjects;
using System;

namespace UnitTests._MockObjects
{
    public class RawEventMock : BaseMock
    {
        public RawEventVO MockRawEvent()
        {
            return new RawEventVO
            {
                Tag = $"{regions[new Random().Next(0, regions.Length)]}.{sensors[new Random().Next(0, sensors.Length)]}",
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Value = values[new Random().Next(0, values.Length)]
            };
        }
    }
}
