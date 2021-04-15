namespace EventMonitor.ViewObjects
{
    public class RawEventVO
    {
        public long Timestamp { get; set; }
        public string Tag { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Tag: {Tag}, Timestamp: {Timestamp}, Value: {Value}";
        }
    }
}
