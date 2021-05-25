

<h1 align="center"> DotNetStac</h1>


<h2 align="center">
.Net library for working with Spatio Temporal Asset Catalogs (<a href="https://stacspec.org">STAC</a>)

  ![](docs/logo/logo-wide.png)

</h2>

<h3 align="center">

![Build Status](https://github.com/Terradue/DotNetStac/actions/workflows/build.yaml/badge.svg?branch=release/0.9.0)
[![NuGet](https://img.shields.io/nuget/vpre/DotNetStac)](https://www.nuget.org/packages/DotNetStac/)
[![codecov](https://codecov.io/gh/Terradue/DotNetStac/branch/release/0.9.0/graph/badge.svg)](https://codecov.io/gh/Terradue/DotNetStac)
[![Gitter](https://img.shields.io/gitter/room/SpatioTemporal-Asset-Catalog/Lobby?color=yellow)](https://gitter.im/SpatioTemporal-Asset-Catalog/Lobby)
[![License](https://img.shields.io/badge/license-AGPL3-blue.svg)](LICENSE)
[![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/Terradue/DotNetStac/master?filepath=example.ipynb)

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

In a nutshell, the library allows serialization/desrialization of STAC JSON documents (using [Newtonsoft.JSON](https://www.newtonsoft.com/json)) to typed object modeling STAC objects with properties represented in enhanced objects such as geometries, time stamp/period/span, numerical values and many more via STAC extension plugins engine. Stac Item object is based on [GeoJSON.Net](https://github.com/GeoJSON-Net/GeoJSON.Net) feature.

## Features

* (De)Serialization engine fully compliant with current version of [STAC specifications](https://stacspec.org)
* Many helpers to support STAC objects manipulation:
  * Field accessors using class properties and common metadata (e.g. Title, DateTime, Geometry)
  * Collection creation helper summarizing Items set
* JSON Schema validation using [Json.NET Schema](https://github.com/JamesNK/Newtonsoft.Json.Schema)
* STAC extensions support with C# extension classes with direct accessors to the fields:
  * [Electro-Optical](https://github.com/stac-extensions/eo) with `Common Band Name` enumeration
  * [File Info](https://github.com/stac-extensions/file) with helpers to calculate [multihash](https://github.com/multiformats/cs-multihash) checksum
  * [Processing](https://github.com/stac-extensions/processing)
  * [Projection](https://github.com/stac-extensions/projection) with helpers to set `epsg` id and `wkt2` representation from [Proj.Net Coordinate Systems](https://github.com/NetTopologySuite/ProjNet4GeoAPI)
  * [Raster](https://github.com/stac-extensions/raster)
  * [SAR](https://github.com/stac-extensions/sar) with helpers for interferometric searches
  * [Satellite](https://github.com/stac-extensions/sat) with extra orbit state vector and baseline calculation
  * [Scientific Citation](https://github.com/stac-extensions/scientific)
  * [View Geometry](https://github.com/stac-extensions/view)

## Getting Started

### Install package

```console
$ dotnet add package DotNetStac
```

### Deserialize and validate your first catalog

```csharp
using Stac;
using Stac.Schemas;
using System;
using System.Net;
using Newtonsoft.Json.Schema;

var webc = new WebClient();
Uri catalogUri = new Uri("https://raw.githubusercontent.com/radiantearth/stac-spec/master/examples/catalog.json");
StacValidator stacValidator = new StacValidator(new JSchemaUrlResolver());

// StacConvert.Deserialize is the helper to start loading any STAC document
var json = webc.DownloadString(catalogUri);
bool valid = stacValidator.ValidateJson(json);
StacCatalog catalog = StacConvert.Deserialize<StacCatalog>(json);

Console.Out.WriteLine(catalog.Id + ": " + catalog.Description + (valid ? " [VALID]" : "[INVALID]"));
Console.Out.WriteLine(catalog.StacVersion);
```

### Learn more

A [dedicated notebook](notebooks/example.ipynb) is available to get started with all DotNetStac features. If you want to play directly with the notebook, you can [![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/Terradue/DotNetStac/develop?filepath=example.ipynb)

## Documentation

An API documentation site is available at https://terradue.github.io/DotNetStac.

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

