using EventMonitor.ViewObjects;
using System;

namespace IntegrationTests
{
    public class _MockObjects
    {
        private static string[] regions = new string[] { "norte", "nordeste", "sudeste", "sul" };
        private static string[] sensors = new string[] { "sensor01", "sensor02", "sensor03", "sensor04" };
        private static string[] values = new string[] { "0.13", "0.357", "0.542", "0.211", "" };

        public static RawEventVO MockEvent()
        {
            return new RawEventVO
            {
                Tag = $"brasil.{regions[new Random().Next(0, regions.Length)]}.{sensors[new Random().Next(0, sensors.Length)]}",
                //TODO: Corrigir fuso horário em UTC pata GMT-3
                Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Value = values[new Random().Next(0, values.Length)]
            };
        }
    }
}
