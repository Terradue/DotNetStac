using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stac.Collection;
using Stac.Extensions.Disaster;
using Xunit;

namespace Stac.Test.Extensions
{
    public class DisastersCharterExtensionTests : TestBase
    {
        [Fact]
        public void ActivationTest()
        {
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.IsType<StacItem>(item);
            Assert.Equal(DisastersItemClass.Activation, item.DisasterExtension().Class);
            Assert.Equal(744, item.DisasterExtension().ActivationId);
            Assert.Equal(855, item.DisasterExtension().CallIds.First());
            Assert.Equal(new DisastersType[2] { DisastersType.Other, DisastersType.Volcano }, item.DisasterExtension().Types);
            Assert.Equal("TON", item.DisasterExtension().Country);
            Assert.Equal(new string[1] { "Tonga" }, item.DisasterExtension().Regions);
            Assert.Equal(DisastersActivationStatus.Open, item.DisasterExtension().ActivationStatus);
        }

        [Fact]
        public void AreaTest()
        {
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.IsType<StacItem>(item);
            Assert.Equal(DisastersItemClass.Area, item.DisasterExtension().Class);
            Assert.Equal(744, item.DisasterExtension().ActivationId);
            Assert.Equal(855, item.DisasterExtension().CallIds.First());
            Assert.Equal(new DisastersType[2] { DisastersType.Other, DisastersType.Volcano }, item.DisasterExtension().Types);
            Assert.Equal("TON", item.DisasterExtension().Country);
            Assert.Equal(new string[1] { "Tonga" }, item.DisasterExtension().Regions);
        }

        [Fact]
        public void AcquisitionTest()
        {
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.IsType<StacItem>(item);
            Assert.Equal(DisastersItemClass.Acquisition, item.DisasterExtension().Class);
            Assert.Equal(855, item.DisasterExtension().CallIds.First());
            Assert.Equal(DisastersResolutionClass.HR, item.DisasterExtension().ResolutionClass);
        }

        [Fact]
        public void CallCollectionTest()
        {
            var json = GetJson("Extensions");

            var collection = StacConvert.Deserialize<StacCollection>(json);

            Assert.IsType<StacCollection>(collection);
            Assert.Equal(DisastersItemClass.Acquisition, collection.DisasterExtension().Class);
            Assert.Equal(855, collection.DisasterExtension().CallIds.First());
            StacSummaryValueSet<string> resolutionClassSummary = collection.Summaries
                .FirstOrDefault(k => k.Key == DisastersCharterStacExtension.ResolutionClassField).Value as StacSummaryValueSet<string>;

            Assert.Contains(DisastersResolutionClass.HR.ToString(), resolutionClassSummary);
        }

    }
}
