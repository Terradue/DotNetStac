# DotNetStac API

Main entry point API is `StacConvert.Deserialize<IStacObject>`, which deserializes a Stac Object (Catalog, COllection or Item) that implements `IStacObject`:

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