namespace EventMonitor.ViewObjects
{
    public class EventVO
    {
        public long Timestamp { get; set; }
        public string Tag { get; set; }
        //TODO: Cada evento poderá ter o estado processado ou erro, caso o campo valor chegue vazio, o status do evento será erro caso contrário processado.
        public string Value { get; set; }
    }
}
