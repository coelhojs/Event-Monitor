using Event_Simulator.ViewObjects;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Event_Simulator
{
    public class MockObjects
    {
        public string[] regions = new string[] { "norte", "nordeste", "sudeste", "sul" };
        public string[] sensors = new string[] { "sensor01", "sensor02", "sensor03", "sensor04" };
        public string[] values = new string[] { "0.13", "0.357", "0.542", "0.211", "" };

        public StringContent GetEvent(string region, string sensor)
        {
            var mockEvent = MockEvent(region, sensor);

            return SerializeObject(mockEvent);
        }

        public RawEventVO MockEvent(string region, string sensor)
        {
            return new RawEventVO
            {
                Tag = $"brasil.{region}.{sensor}",
                Timestamp = DateTimeOffset.Now.AddHours(-3).ToUnixTimeMilliseconds(),
                Value = values[new Random().Next(0, values.Length)]
            };
        }

        public StringContent SerializeObject(RawEventVO mockEvent)
        {
            var serializedObject = JsonConvert.SerializeObject(mockEvent);

            return new StringContent(serializedObject, Encoding.UTF8, "application/json");
        }
    }
}
