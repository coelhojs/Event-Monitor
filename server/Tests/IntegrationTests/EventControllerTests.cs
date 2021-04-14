using System.Net;
using Xunit;

namespace IntegrationTests
{
    public class EventControllerTests : IClassFixture<_TestFixtures>
    {
        private readonly _TestFixtures _;

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
            var response = await _.Client.PostAsync($"{_.AppUrl}/Event", content);

            //assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }
    }
}
