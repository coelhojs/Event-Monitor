using System.Net;
using UnitTests;
using UnitTests._MockObjects;
using Xunit;

namespace IntegrationTests
{
    public class EventControllerTests : IClassFixture<_TestFixtures>
    {
        private readonly _TestFixtures _;
        private readonly RawEventMock _rawEventMock;

        public EventControllerTests(_TestFixtures testFixtures)
        {
            _ = testFixtures;
        }

        [Fact]
        [Trait("Category", "integration")]
        [Trait("Description", "Valida o processo de envio de novos eventos para a aplica��o.")]
        public async void NewEvent()
        {
            //arrange
            var mockEvent = _rawEventMock.MockRawEvent();

            var content = _.SerializeObject(mockEvent);

            //act
            var response = await _.Client.PostAsync($"{_.AppUrl}/Event", content);

            //assert
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }
    }
}
