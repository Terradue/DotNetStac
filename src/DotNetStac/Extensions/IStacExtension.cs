namespace Stac.Extensions
{
    public interface IStacExtension
    {
        string Id { get; }

        IStacExtension CopyForStacObject(IStacObject stacObject);
    }
}