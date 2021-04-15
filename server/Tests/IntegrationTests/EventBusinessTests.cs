using EventMonitor.Business;
using EventMonitor.Interfaces;
using EventMonitor.ViewObjects;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public class EventBusinessTests : IClassFixture<_TestFixtures>
    {
        private readonly ILogger<EventBusiness> _logger;

        private readonly _TestFixtures _;
        private readonly IEventBusiness _eventBusiness;

        public EventBusinessTests(_TestFixtures testFixtures)
        {
            _ = testFixtures;
            _logger = Mock.Of<ILogger<EventBusiness>>();
            _eventBusiness = new EventBusiness(_logger);
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
            catch (Exception ex)
            {
                Assert.Null(ex);
            }
        }
    }
}
