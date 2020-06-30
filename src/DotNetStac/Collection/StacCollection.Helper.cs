using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Catalog;
using Stac.Collection;
using Stac.Model;

namespace Stac.Collection
{
    public partial class StacCollection : IStacObject
    {
        internal static StacCollection LoadStacCollection(JToken jsonRoot)
        {
            Type collectionType = null;
            if (jsonRoot["stac_version"] == null)
            {
                throw new InvalidDataException("The document is not a STAC document. No 'stac_version' property found");
            }

            if (jsonRoot["extent"] == null)
            {
                throw new InvalidDataException("The document is not a STAC collection document. No 'extent' property found. Probably a catalog.");
            }

            try
            {
                collectionType = Stac.Model.SchemaDictionary.GetCollectionTypeFromVersion(jsonRoot["stac_version"].Value<string>());
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException(string.Format("The document has a non supprted version: '{0}'.", jsonRoot["stac_version"].Value<string>()));
            }

            IStacCollectionVersion catalog = (IStacCollectionVersion)jsonRoot.ToObject(collectionType);

            while (catalog.GetType() != typeof(StacCollection))
            {
                catalog = catalog.Upgrade();
            }

            return (StacCollection)catalog;
        }

    }
}
