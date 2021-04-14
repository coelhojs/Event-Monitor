using EventMonitor.ViewObjects;
using System.Threading.Tasks;

namespace EventMonitor.Services
{
    public interface IEventsProcessor
    {
        void Enqueue(RawEventVO newEvent);
        Task Process();
    }
}