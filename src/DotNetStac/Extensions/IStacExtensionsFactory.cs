namespace Stac.Extensions
{
    public interface IStacExtensionsFactory
    {

        IStacExtension InitStacExtension(string prefix);

    }
}