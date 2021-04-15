using Autofac.Extras.Moq;
using EventMonitor;
using EventMonitor.Business;
using EventMonitor.Interfaces;
using Moq;
using UnitTests._MockObjects;
using Xunit;

namespace UnitTests
{
    public class GetEventsStatsTests : IClassFixture<_TestFixtures>
    {
        private readonly EventMock _eventMock;
        private readonly EventStatsMock _eventStatsMock;
        private readonly _TestFixtures _;

        public GetEventsStatsTests(_TestFixtures testFixtures)
        {
            _ = testFixtures;
            _eventMock = new EventMock();
            _eventStatsMock = new EventStatsMock();
        }

        [Fact]
        //Padrão de nomenclatura: MétodoTestado_Cenário_RetornoEsperado
        public void GetEventsStats_ExistingData_ReturnsOrderedListWithSummarizedData()
        {
            using (var mock = AutoMock.GetLoose())
            {
                using (var context = Mock.Of<Context>())
                {
                    //arrange
                    var incomingList = _eventMock.MockEventList(5);

                    mock.Mock<IEventDAO>()
                        .Setup(x => x.GetStats())
                        .Returns(_eventStatsMock.MockEventStatsList(5));

                    var cls = mock.Create<EventBusiness>();

                    //act
                    var result = cls.GetEventsStats();

                    //assert: Assegurar que a lista está ordenada por região e que o primeiro
                    //item de cada região seja o seu totalizador
                    long currentCounter = 0;
                    long? expectedSum = null;

                    foreach (var item in result)
                    {
                        //verifica se o item é um resumo da região
                        if (item.Sensor == null && item.Status == null)
                        {
                            //para ignorar a primeira iteração
                            if (expectedSum != null)
                            {
                                //Verifica se a contagem do total de eventos de uma região é igual ao total no item de resumo
                                Assert.Equal(currentCounter, expectedSum);

                                expectedSum = 0;
                            }

                            currentCounter = item.Counter;
                        }
                        else if (expectedSum == null)
                        {
                            expectedSum = item.Counter;
                        }
                        else
                        {
                            expectedSum += item.Counter;
                        }
                    }
                }
            }
        }
    }
}
