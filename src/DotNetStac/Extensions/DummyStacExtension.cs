namespace Stac.Extensions
{
    internal class DummyStacExtension : IStacExtension
    {
        public DummyStacExtension(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; private set; }
    }
}