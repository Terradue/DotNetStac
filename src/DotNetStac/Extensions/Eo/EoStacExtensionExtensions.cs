namespace Stac.Extensions.Eo
{
    public static class EoStacExtensionExtensions
    {

        public static EoBandObject[] GetEoBandObjects(this StacAsset stacAsset)
        {
            string key = Eo.EoStacExtension.Prefix + ":" + Eo.EoStacExtension.BandsField;
            if (stacAsset.Properties.ContainsKey(key))
                return (EoBandObject[])stacAsset.Properties[key];
            return null;
        }

        public static void SetEoBandObjects(this StacAsset stacAsset, EoBandObject[] eoBandObjects)
        {
            string key = Eo.EoStacExtension.Prefix + ":" + Eo.EoStacExtension.BandsField;
            if (stacAsset.Properties.ContainsKey(key))
                stacAsset.Properties.Remove(key);
            stacAsset.Properties.Add(key, eoBandObjects);
        }

    }
}
