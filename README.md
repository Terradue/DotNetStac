

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
  [![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/Terradue/DotNetStac/develop?filepath=example.ipynb)

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

In a nutshell, the library allows import/export of STAC JSON documents (Serialization/Deserialization using [Newtonsoft.JSON](https://www.newtonsoft.com/json)) to typed object with properties represented in enhanced objects such as geometries, time stamp/period/span, numerical values and many more via STAC extension plugins engine.

## Features

### Current features

* (De)Serialization engine supporting current and older versions of the specifications with an upgrade mechanism
* Navigation methods to seamlessly traverse a STAC catalog through collections and items
* Enhanced extensions support with plugin system for embedding extension related functions (e.g. sat: orbit file download, sar: interferometric search, eo: calibration parameters)

### Other features to come

* STAC API Client
* Importers from other spatio temporal domains (e.g. OGC O&M, Earth Observation profile…)

## Getting Started

A [dedicated notebook](notebooks/example.ipynb) is available to get started. If you want to play directly with the notebook, you can [![Binder](https://mybinder.org/badge_logo.svg)](https://mybinder.org/v2/gh/Terradue/DotNetStac/develop?filepath=example.ipynb)

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

