using EventMonitor.ViewObjects;
using System;

namespace IntegrationTests
{
    public class _MockObjects
    {
        public static EventVO MockEvent()
        {
            return new EventVO
            {
                Tag = "brasil.sudeste.sensor01",
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Value = "processado"
            };
        }
    }
}
