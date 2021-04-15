using EventMonitor.ViewObjects;
using System.Threading.Tasks;

namespace EventMonitor.Interfaces
{
    public interface IEventsProcessor
    {
        void Enqueue(RawEventVO newEvent);
        Task Process();
    }
}