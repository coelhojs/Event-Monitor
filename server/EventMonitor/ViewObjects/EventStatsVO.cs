﻿namespace EventMonitor.ViewObjects
{
    public class EventStatsVO
    {
        public long Counter { get; set; }
        public long Errors { get; set; }
        public long Processed { get; set; }
        public string Region { get; set; }
        public string Sensor { get; set; }
        public string Status { get; set; }
    }
}
