using EventMonitor.Business;
using EventMonitor.DAO;
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
        private readonly _TestFixtures _;
        private readonly IEventBusiness _eventBusiness;
        private readonly EventDAO _eventDAO;
        private readonly ILogger<EventBusiness> _logger;


        public EventBusinessTests(_TestFixtures testFixtures)
        {
            _ = testFixtures;
            _eventDAO = new EventDAO();
            _logger = Mock.Of<ILogger<EventBusiness>>();
            _eventBusiness = new EventBusiness(_logger, _eventDAO);
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
