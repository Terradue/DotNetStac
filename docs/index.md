# DotNetStac

.Net library for working with Spatio Temporal Asset Catalogs [STAC](https://stacspec.org)

![stac-logo](images/stac_logo_128.png)

**DotNetStac** helps you to work with [STAC](https://stacspec.org) ([catalog](https://github.com/radiantearth/stac-spec/tree/master/catalog-spec), [collection](https://github.com/radiantearth/stac-spec/tree/master/collection-spec), [item](https://github.com/radiantearth/stac-spec/tree/master/catalog-spec))

In a nutshell, the library allows serialization/desrialization of STAC JSON documents (using [Newtonsoft.JSON](https://www.newtonsoft.com/json)) to typed object modeling STAC objects with properties represented in enhanced objects such as geometries, time stamp/period/span, numerical values and many more via STAC extension plugins engine. Stac Item object is based on [GeoJSON.Net](https://github.com/GeoJSON-Net/GeoJSON.Net) feature.

## Features

* (De)Serialization engine fully compliant with current version of [STAC specifications](https://stacspec.org)
* Many helpers to support STAC objects manipulation:
  * Field accessors using class properties (e.g. Title, DateTime, Geometry)
  * Collection creation helper summarizing Items set
* STAC extensions support with C# extension classes with direct accessors to the fields.
* JSON Schema validation using [Json.NET Schema](https://github.com/JamesNK/Newtonsoft.Json.Schema)

## Reference API

Looking for the reference API docs, go directly to [https://terradue.github.io/DotNetStac/api/].

## Getting Started

### Install DotNetStac

Using [nuget package](https://www.nuget.org/packages/DotNetStac/)

```console
dotnet add package DotNetStac
```

### Deserialization of STAC documents

The (de)serialiation methods are wrapped in methods in [class `StacConvert`](https://terradue.github.io/DotNetStac/api/Stac.StacConvert.html) that is the main entry point from/to JSON/.Net.

Let's start reading a STAC catalog online. Please note that DotNetStac does not provide with data access middleware. You can integrate own data access or you can test the [`Stars` SDK](https://github.com/Terradue/Stars) that provides with integrated functions to manipulate STAC objects and their storage.

The following code is a very simple function loading a catalog and printing it's `id`, `description` and `stac_version`.

```csharp
using Stac;
using System;
using System.Net;

var webc = new WebClient();
Uri catalogUri = new Uri("https://cbers-stac-1-0.s3.amazonaws.com/CBERS4/catalog.json");

// StacConvert.Deserialize is the helper to start loading any STAC document
var json = webc.DownloadString(catalogUri);
StacCatalog catalog = StacConvert.Deserialize<StacCatalog>(json);

Console.Out.WriteLine(catalog.Id + ": " + catalog.Description);
Console.Out.WriteLine(catalog.StacVersion);
```

### Navigation from a Catalogue

Using the previously loaded catalog, the following code executes a recursive function navigating from a root catalog through all it's tree structure recursing in the `child` STAC links and listing the `item` links and their `assets`.

Please note the following:

* `GetChildrenLinks` and `GetItemLinks` are the recommanded ways to get the links for navigating through the tree.
* The previous functions as the rest of the library does not alter the `Uri`s. It is then up to the developer to resolve the relative ones. As in the code, Uri class provides with all the necessary methods to easily join a base Url with a relative one.
* The `StacConvert.Deserialize<>` methods allows to specify the interfaces to ease the deserialization when the STAC type is unknown: [`IStacObject`](https://terradue.github.io/DotNetStac/api/Stac.IStacObject.html) and [`IStacCatalog`](https://terradue.github.io/DotNetStac/api/Stac.IStacCatalog.html).

```csharp
using System.Linq;

public static void ListChildrensItemsAndAssets(IStacParent catalog, Uri baseUri, WebClient webc, string prefix = "", int limit = 2)
{
    // Get children and items (sub catalogs, collections and items)
    foreach (var childLink in catalog.GetChildrenLinks().Concat(catalog.GetItemLinks()).Take(limit))
    {
        // IMPORTANT: Relative Uri resolution
        Uri childUri = childLink.Uri;
        if (!childUri.IsAbsoluteUri)
            childUri = new Uri(baseUri, childUri.ToString());
        var childjson = webc.DownloadString(childUri);
        // STAC object loading (using the IStacObject interface)
        IStacObject child = StacConvert.Deserialize<IStacObject>(childjson);
        // Print the node
        Console.Out.WriteLine(prefix + child.Id + ": " + child.Title);

        // List assets if item
        if (child is StacItem)
        {
            StacItem item = child as StacItem;
            foreach (var asset in item.Assets.Values) {
                Console.Out.WriteLine(prefix + " *[" + asset.MediaType + "] " + asset.Uri);
            }
        }

        // Go deeper if catalog or collection
        if (child is StacCatalog || child is StacCollection)
            ListChildrensItemsAndAssets(child as IStacParent, childUri, webc, prefix + " ", limit);
        
    }
}

// Start the navigation
ListChildrensItemsAndAssets(catalog as IStacParent, catalogUri, webc);
```

### Creation and serialization of STAC documents

#### Collection creation from scratch

Let's now create a STAC collection programmatically using the plain Collection object. Here all collection specific fields are set manually.

```csharp
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Collection;
using System;
using System.Collections.Generic;


  // First the mandatory elements of a Collection
  // Spatial Extent
  StacExtent extent = new StacExtent();
  extent.Spatial = new StacSpatialExtent(-180, -56, 180, 83);
  // Temporal Extent (set null to specify an open time extent)
  extent.Temporal = new StacTemporalExtent(DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(), null);

  // Create the Collection
  StacCollection collection = new StacCollection("COPERNICUS/S2",
    "Sentinel-2 is a wide-swath, high-resolution, multi-spectral\nimaging mission supporting Copernicus Land Monitoring studies,...",
                                      extent);

  // Title
  collection.Title = "Sentinel-2 MSI: MultiSpectral Instrument, Level-1C";

  // Links (self, parent, root, other) via different helpers
  collection.Links.Add(StacLink.CreateSelfLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/COPERNICUS_S2.json")));
  collection.Links.Add(StacLink.CreateParentLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
  collection.Links.Add(StacLink.CreateRootLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
  collection.Links.Add(new StacLink(new Uri("https://scihub.copernicus.eu/twiki/pub/SciHubWebPortal/TermsConditions/Sentinel_Data_Terms_and_Conditions.pdf"), "license", "Legal notice on the use of Copernicus Sentinel Data and Service Information", null));

  // Keywords
  collection.Keywords.Add("copernicus");
  collection.Keywords.Add("esa");
  collection.Keywords.Add("eu");
  collection.Keywords.Add("msi");
  collection.Keywords.Add("radiance");
  collection.Keywords.Add("sentinel");

  // Collection of Providers
  collection.Providers.Add(new StacProvider("European Union/ESA/Copernicus",
                            new List<StacProviderRole>() { StacProviderRole.producer, StacProviderRole.licensor })
            {
                Uri = new Uri("https://sentinel.esa.int/web/sentinel/user-guides/sentinel-2-msi")
            });

  // Summaries
  // Stat Summary for the dates
  collection.Summaries.Add("datetime",
      new StacSummaryStatsObject<DateTime>(
          DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(),
          DateTime.Parse("2019-07-10T13:44:56Z").ToUniversalTime()
      )
  );

  // Value Set Summary for the platforms
  collection.Summaries.Add("platform",
      new StacSummaryValueSet<string>(new string[] { "sentinel-2a", "sentinel-2b" })
  );

  collection.Summaries.Add("constellation",
      new StacSummaryValueSet<string>(new string[] { "sentinel-2" })
  );

  collection.Summaries.Add("instruments",
      new StacSummaryValueSet<string>(new string[] { "msi" })
  );

  // Stat Summary for specific extensions
  collection.Summaries.Add("view:off_nadir",
      new StacSummaryStatsObject<double>(
          0.0,
          100
      )
  );

  collection.Summaries.Add("view:sun_elevation",
      new StacSummaryStatsObject<double>(
          6.78,
          89.9
      )
  );

  collection.Summaries.Add("sci:citation",
      new StacSummaryValueSet<string>(new string[] { "Copernicus Sentinel data [Year]" })
  );

  collection.Summaries.Add("gsd",
      new StacSummaryValueSet<int>(new int[] {
          10,
          30,
          60
      })
  );

  collection.Summaries.Add("proj:epsg",
      new StacSummaryValueSet<int>(new int[]
          { 32601,32602,32603,32604,32605,32606,32607,32608,32609,32610,32611,32612,32613,32614,32615,32616,32617,32618,32619,32620,32621,32622,32623,32624,32625,32626,32627,32628,32629,32630,32631,32632,32633,32634,32635,32636,32637,32638,32639,32640,32641,32642,32643,32644,32645,32646,32647,32648,32649,32650,32651,32652,32653,32654,32655,32656,32657,32658,32659,32660}
      )
  );

  // Value Set Summary can also be set using JToken objects from Newtonsoft.Json library
  collection.Summaries.Add("eo:bands",
      new StacSummaryValueSet<JObject>(new JObject[] {
          new JObject {
              { "name", "B1" },
              { "common_name", "coastal" },
              { "center_wavelength", 4.439 }
          },
          new JObject {
              { "name", "B2"},
              { "common_name", "blue"},
              { "center_wavelength", 4.966}
          },
          new JObject {
              { "name", "B3"},
              { "common_name", "green"},
              { "center_wavelength", 5.6}
          },
          new JObject {
              { "name", "B4"},
              { "common_name", "red"},
              { "center_wavelength", 6.645}
          },
          new JObject {
              { "name", "B5"},
              { "center_wavelength", 7.039}
          },
          new JObject {
              { "name", "B6"},
              { "center_wavelength", 7.402}
          },
          new JObject {
              { "name", "B7"},
              { "center_wavelength", 7.825}
          },
          new JObject {
              { "name", "B8"},
              { "common_name", "nir"},
              { "center_wavelength", 8.351}
          },
          new JObject {
              { "name", "B8A"},
              { "center_wavelength", 8.648}
          },
          new JObject {
              { "name", "B9"},
              { "center_wavelength", 9.45}
          },
          new JObject {
              { "name", "B10"},
              { "center_wavelength", 1.3735}
          },
          new JObject {
              { "name", "B11"},
              { "common_name", "swir16"},
              { "center_wavelength", 1.6137}
          },
          new JObject {
              { "name", "B12"},
              { "common_name", "swir22"},
              { "center_wavelength", 2.2024}
          }
      })
  );

  // Serialize
  var json = StacConvert.Serialize(collection);
  // Print JSON!
  Console.WriteLine(json)
  ```

#### Collection generation from a set of Items

[`StacCollection` class](https://terradue.github.io/DotNetStac/api/Stac.StacCollection.html) has static methods allowing the automatic generation of a collection from a set of `StacItem`. The following code loads the items of [the examples folder from STAC repository](https://github.com/radiantearth/stac-spec/tree/master/examples) and generates the corresponding collection with
- **Spatial and temporal extent** from geometry and time merge of the items
- **Fields summaries** with stats objects or value sets of the items' fields values
  
Please note that the function takes also the eventual uri of the collection in input. If specified, the items Uri are made relative to that uri.

```csharp
Uri simpleItemUri = new Uri("https://raw.githubusercontent.com/radiantearth/stac-spec/fix_examples/examples/simple-item.json");
Uri coreItemUri = new Uri("https://raw.githubusercontent.com/radiantearth/stac-spec/fix_examples/examples/core-item.json");
Uri extendedItemUri = new Uri("https://raw.githubusercontent.com/radiantearth/stac-spec/fix_examples/examples/extended-item.json");

StacItem simpleItem = StacConvert.Deserialize<StacItem>(webc.DownloadString(simpleItemUri));
StacItem coreItem = StacConvert.Deserialize<StacItem>(webc.DownloadString(coreItemUri));
StacItem extendedItem = StacConvert.Deserialize<StacItem>(webc.DownloadString(extendedItemUri));

Dictionary<Uri, StacItem> items = new Dictionary<Uri, StacItem>();
items.Add(simpleItemUri, simpleItem);
items.Add(coreItemUri,coreItem);
items.Add(extendedItemUri, extendedItem);

StacCollection stacCollection = StacCollection.Create(new Uri($"https://raw.githubusercontent.com/radiantearth/stac-spec/fix_examples/examples/collection.json"),
                                                                "simple-collection",
                                                                "A simple collection demonstrating core catalog fields with links to a couple of items",
                                                                items,
                                                                null,
                                                                "CC-BY-4.0");

Console.Out.Write(StacConvert.Serialize(collection));
```
