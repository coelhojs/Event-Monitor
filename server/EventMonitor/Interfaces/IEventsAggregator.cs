using System.Threading;
using System.Threading.Tasks;

namespace EventMonitor.Interfaces
{
    public interface IEventsAggregator
    {
        CancellationTokenSource AggregatorCTS { get; set; }
        Task AggregatorTask { get; set; }

        void AggregateStats();
        void StartAggregator();
        void StopAggregator();
    }
}