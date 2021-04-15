using Autofac.Extras.Moq;
using EventMonitor.Business;
using System;
using UnitTests._MockObjects;
using Xunit;

namespace UnitTests
{
    public class ParseEventTests : IClassFixture<_TestFixtures>
    {
        private readonly _TestFixtures _;
        private readonly RawEventMock _rawEventMock;

        public ParseEventTests(_TestFixtures testFixtures)
        {
            _ = testFixtures;
            _rawEventMock = new RawEventMock();
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("brasil-sudeste-sensor01")]
        [InlineData("brasil,sudeste,sensor01")]
        [InlineData("brasil.sudeste")]
        [InlineData("brasil sudeste sensor01")]
        [InlineData("sudeste.sensor01")]
        //Padrão de nomenclatura: MétodoTestado_Cenário_RetornoEsperado
        public void ParseEvent_InvalidEvents_ThrowsFormatException(string tag)
        {
            using (var mock = AutoMock.GetLoose())
            {
                try
                {
                    //arrange
                    var rawEvent = _rawEventMock.MockRawEvent();

                    rawEvent.Tag = tag;

                    var cls = mock.Create<EventBusiness>();

                    //act
                    var result = cls.ParseEvent(rawEvent);

                    //assert
                    Assert.False(true, "Era esperado que o código falhasse na linha acima.");
                }
                catch (Exception ex)
                {
                    //assert
                    Assert.IsType<FormatException>(ex);
                }
            }
        }
    }
}
