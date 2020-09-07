using System.Collections.Generic;
using Stac.Item;

namespace Stac
{
    public interface IStacItem : IStacObject
    {
        IDictionary<string, StacAsset> Assets { get; }

        Itenso.TimePeriod.ITimePeriod DateTime { get; }
    }
}