namespace Stac.Extensions
{
    public interface IStacExtensionsFactory
    {

        IStacExtension CreateStacExtension(string prefix);

    }
}