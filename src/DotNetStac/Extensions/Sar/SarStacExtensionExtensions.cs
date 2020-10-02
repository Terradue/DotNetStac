namespace Stac.Extensions.Sar
{
    public static class SarStacExtensionExtensions
    {

        public static string[] GetPolarizations(this StacAsset stacAsset)
        {
            string key = Sar.SarStacExtension.Prefix + ":" + Sar.SarStacExtension.PolarizationsField;
            if (stacAsset.Properties.ContainsKey(key))
                return (string[])stacAsset.Properties[key];
            return null;
        }

        public static void SetEoBandObjects(this StacAsset stacAsset, string[] eoBandObjects)
        {
            string key = Sar.SarStacExtension.Prefix + ":" + Sar.SarStacExtension.PolarizationsField;
            if (stacAsset.Properties.ContainsKey(key))
                stacAsset.Properties.Remove(key);
            stacAsset.Properties.Add(key, eoBandObjects);
        }

    }
}
