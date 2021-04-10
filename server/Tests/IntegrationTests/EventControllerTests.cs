using System.Net;
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
        public async void NewEvent()
        {
            //arrange
            var mockEvent = _MockObjects.MockEvent();

            var content = _.SerializeObject(mockEvent);

            //act
            await _.Client.PostAsync($"{_.AppUrl}/Event", content);

            //assert
            var persistedEvent = _.Client.GetAsync($"{_.AppUrl}/Event?timestamp={mockEvent.Timestamp}&tag={mockEvent.Tag}&value={mockEvent.Value}").Result;

            Assert.Equal(HttpStatusCode.OK, persistedEvent.StatusCode);
        }
    }
}
