using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace EventMonitor.Services
{
    public class EventsProcessor : IEventsProcessor
    {
        private readonly IEventBusiness _eventBusiness;
        private readonly ConcurrentQueue<RawEventVO> _concurrentQueue;

        public EventsProcessor(IEventBusiness eventBusiness)
        {
            _concurrentQueue = new ConcurrentQueue<RawEventVO>();
            _eventBusiness = eventBusiness;

            Task.Run(Process);
        }

        public void Enqueue(RawEventVO newEvent)
        {
            _concurrentQueue.Enqueue(newEvent);
        }

        public async Task Process()
        {
            RawEventVO rawEvent;

            while (true)
            {
                if (_concurrentQueue.TryDequeue(out rawEvent))
                {
                    await _eventBusiness.ProcessEvent(rawEvent);
                }
            }
        }
    }
}
