using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace IntegrationTests
{
    public class EventControllerTests : IClassFixture<_TestFixtures>
    {
        private _TestFixtures _;

        public EventControllerTests(_TestFixtures testFixtures)
        {
            _ = testFixtures;
        }

        [Fact]
        public void NewEvent()
        {
            //arrange
            var mockEvent = _MockObjects.MockEvent();
            var data = JsonConvert.SerializeObject(mockEvent);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            //act
            var result = _.Client.PostAsync($"{_.AppUrl}/Event", content).Result;

            //assert
            var persistedEvent = _.Client.GetAsync($"{_.AppUrl}/Event?tag='{mockEvent.Tag}'").Result;

            Assert.Equal(HttpStatusCode.OK, persistedEvent.StatusCode);
        }
    }
}
