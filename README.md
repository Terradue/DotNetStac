

<h1 align="center"> DotNetStac</h1>


<h2 align="center">
.Net library for working with Spatio Temporal Asset Catalogs (<a href="https://stacspec.org">STAC</a>)

  ![](docs/logo/logo-wide.png)

</h2>

<h3 align="center">

  [![Build Status](https://travis-ci.com/Terradue/DotNetStac.svg?branch=develop)](https://travis-ci.com/Terradue/DotNetStac)
  [![NuGet](https://img.shields.io/nuget/vpre/DotNetStac)](https://www.nuget.org/packages/DotNetStac/)
  [![Gitter](https://img.shields.io/gitter/room/SpatioTemporal-Asset-Catalog/Lobby?color=yellow)](https://gitter.im/SpatioTemporal-Asset-Catalog/Lobby)
  [![License](https://img.shields.io/badge/license-AGPL3-blue.svg)](LICENSE)
  [![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/Terradue%2FDotNetStac/develop?urlpath=lab)

</h3>

<h3 align="center">
  <a href="#Features">Features</a>
  <span> · </span>
  <a href="#Getting-Started">Getting started</a>
  <span> · </span>
  <a href="#Documentation">Documentation</a>
  <span> · </span>
  <a href="#Developing">Developing</a>
</h3>

**DotNetStac** helps you to work with [STAC](https://stacspec.org) ([catalog](https://github.com/radiantearth/stac-spec/tree/master/catalog-spec), [collection](https://github.com/radiantearth/stac-spec/tree/master/collection-spec), [item](https://github.com/radiantearth/stac-spec/tree/master/catalog-spec))

In a nutshell, the library allows manipulating STAC JSON documents (Serialization/Deserialization using [Newtonsoft.JSON](https://www.newtonsoft.com/json)) with properties represented in enhanced objects such as geometries, time stamp/period/span, numerical values and many more via STAC extension plugins engine.

## Features

### Current features

* (De)Serialization engine supporting current and older versions of the specifications with an upgrade mechanism
* Navigation methods to seamlessly traverse a STAC catalog through collections and items
* Enhanced extensions support with plugin system for embedding extension related functions (e.g. sat: orbit file download, sar: interferometric search, eo: calibration parameters)

### Other features to come

* STAC API Client
* Importers from other spatio temporal domains (e.g. OGC O&M, Earth Observation profile…)

## Getting Started

### Installation

DotNetStac can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package DotNetStac
```

### Example #1 : Deserialization and Navigation

The following example load a catalog and loads all its items 

```csharp
using Stac;

    IStacCatalog catalog = (IStacCatalog)StacFactory.Load("https://cbers-stac-0-7.s3.amazonaws.com/CBERS4/MUX/027/069/catalog.json");

    Console.Out.WriteLine(catalog.Id);
    Console.Out.WriteLine(catalog.StacVersion);

    foreach (var item in catalog.GetItems().Values)
    {
        Console.Out.WriteLine(item.Id);
    }

```

### Example #2 : Serialization of a created collection

The following example load a catalog and loads all its items 

```csharp
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac;
using Stac.Catalog;
using Stac.Collection;
using System;
using System.Collections.Generic;

  StacExtent extent = new StacExtent();
  extent.Spatial = new StacSpatialExtent(-180, -56, 180, 83);
  extent.Temporal = new StacTemporalExtent(DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(), null);

  StacCollection collection = new StacCollection("COPERNICUS/S2",
    "Sentinel-2 is a wide-swath, high-resolution, multi-spectral\nimaging mission supporting Copernicus Land Monitoring studies,...",
                                      extent);

  collection.Title = "Sentinel-2 MSI: MultiSpectral Instrument, Level-1C";

  collection.Links.Add(StacLink.CreateSelfLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/COPERNICUS_S2.json")));
  collection.Links.Add(StacLink.CreateParentLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
  collection.Links.Add(StacLink.CreateRootLink(new Uri("https://storage.cloud.google.com/earthengine-test/catalog/catalog.json")));
  collection.Links.Add(new StacLink(new Uri("https://scihub.copernicus.eu/twiki/pub/SciHubWebPortal/TermsConditions/Sentinel_Data_Terms_and_Conditions.pdf"), "license", "Legal notice on the use of Copernicus Sentinel Data and Service Information", null));

  collection.Keywords = new System.Collections.ObjectModel.Collection<string>(new string[] {
      "copernicus",
      "esa",
      "eu",
      "msi",
      "radiance",
      "sentinel"});

  collection.Providers = new System.Collections.ObjectModel.Collection<StacProvider>(
      new StacProvider[]{new StacProvider("European Union/ESA/Copernicus"){
          Roles = new List<StacProviderRole>() { StacProviderRole.producer, StacProviderRole.licensor},
          Uri = new Uri("https://sentinel.esa.int/web/sentinel/user-guides/sentinel-2-msi")
  }});

  collection.Summaries.Add("datetime",
      new StacSummaryStatsObject<DateTime>(
          DateTime.Parse("2015-06-23T00:00:00Z").ToUniversalTime(),
          DateTime.Parse("2019-07-10T13:44:56Z").ToUniversalTime()
      )
  );

  collection.Summaries.Add("platform",
      new StacSummaryValueSet<string>(new string[] { "sentinel-2a", "sentinel-2b" })
  );

  collection.Summaries.Add("constellation",
      new StacSummaryValueSet<string>(new string[] { "sentinel-2" })
  );

  collection.Summaries.Add("instruments",
      new StacSummaryValueSet<string>(new string[] { "msi" })
  );

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

  var json = JsonConvert.SerializeObject(collection);

  Console.WriteLine(json);

```

## Documentation

*Full Documentation shall be available soon*

## Developing

To ensure development libraries are installed, restore all dependencies

```
> dotnet restore src
```

### Unit Tests

Unit tests are in the `src/DotNetStac.Test` folder. To run unit tests:

```
> dotnet test src
```

