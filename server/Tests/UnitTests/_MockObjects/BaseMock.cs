namespace UnitTests
{
    public class BaseMock
    {
        public readonly string[] regions = new string[] { "brasil.norte", "brasil.nordeste", "brasil.sudeste", "brasil.sul" };
        public readonly string[] sensors = new string[] { "sensor01", "sensor02", "sensor03", "sensor04" };
        public readonly string[] statuses = new string[] { "erro", "processado" };
        public readonly string[] values = new string[] { "0.13", "0.357", "0.542", "0.211", "" };
    }
}
