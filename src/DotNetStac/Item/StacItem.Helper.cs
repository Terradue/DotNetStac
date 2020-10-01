using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Stac;
using Stac.Converters;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Catalog;
using Stac.Extensions;
using Stac.Model;

namespace Stac.Item
{
    public partial class StacItem : IStacItem, IStacObject
    {

        public void SetBoundingBoxOnGeometryExtent()
        {
            var boundingBoxes = Geometry.GetBoundingBox();
            if (boundingBoxes[0].Altitude.HasValue)
                this.BoundingBoxes = new double[] {
                    boundingBoxes[0].Longitude, boundingBoxes[0].Latitude, boundingBoxes[0].Altitude.Value,
                    boundingBoxes[1].Longitude, boundingBoxes[1].Latitude, boundingBoxes[1].Altitude.Value,
                };
            else
                this.BoundingBoxes = new double[] {
                    boundingBoxes[0].Longitude, boundingBoxes[0].Latitude,
                    boundingBoxes[1].Longitude, boundingBoxes[1].Latitude,
                };
        }

        public static async Task<IStacItem> LoadUri(Uri uri)
        {
            var catalog = await StacFactory.LoadUriAsync(uri);
            if (catalog is IStacItem)
                return (IStacItem)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC item {0}", catalog.Uri));
        }

        public static async Task<IStacItem> LoadStacLink(StacLink link)
        {
            var catalog = await StacFactory.LoadStacLink(link);
            if (catalog is IStacItem)
                return (IStacItem)catalog;
            throw new InvalidOperationException(string.Format("This is not a STAC item {0}", catalog.Uri));
        }

        public static IStacItem LoadJToken(JToken jsonRoot, Uri uri)
        {
            IStacItem item = LoadStacItem(jsonRoot);
            ((IInternalStacObject)item).Uri = uri;
            return item;
        }

        private static IStacItem LoadStacItem(JToken jsonRoot)
        {
            Type itemType = null;
            if (jsonRoot["stac_version"] == null)
            {
                throw new InvalidDataException("The document is not a STAC document. No 'stac_version' property found");
            }

            try
            {
                itemType = Stac.Model.SchemaDictionary.GetItemTypeFromVersion(jsonRoot["stac_version"].Value<string>());
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException(string.Format("The document has a non supprted version: '{0}'.", jsonRoot["stac_version"].Value<string>()));
            }

            return (IStacItem)jsonRoot.ToObject(itemType);

        }
    }
}
