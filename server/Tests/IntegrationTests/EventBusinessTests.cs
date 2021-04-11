using EventMonitor.Business;
using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using System.Collections.Generic;
using Xunit;

namespace IntegrationTests
{
    public class EventBusinessTests : IClassFixture<_TestFixtures>
    {
        private _TestFixtures _;
        private IEventBusiness _eventBusiness;

        public EventBusinessTests(_TestFixtures testFixtures)
        {
            _ = testFixtures;
            _eventBusiness = new EventBusiness();
        }

        [Fact]
        public void GetEventsStats()
        {
            List<EventStatsVO> stats;

            try
            {
                stats = _eventBusiness.GetEventsStats();

                Assert.NotNull(stats);
            }
            catch (System.Exception ex)
            {
                Assert.Null(ex);
            }
        }
    }
}
