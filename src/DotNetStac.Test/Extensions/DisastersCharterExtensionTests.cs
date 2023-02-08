// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: DisastersCharterExtensionTests.cs

using System;
using System.Linq;
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

        [Fact]
        public void PropertiesTest()
        {
            var json = GetJson("Extensions");

            var item = StacConvert.Deserialize<StacItem>(json);

            Assert.IsType<StacItem>(item);
            Assert.Throws<ArgumentException>(() => item.DisasterExtension().Country = "Belgium");
            item.DisasterExtension().Country = "BEL";
            Assert.Equal("BEL", item.DisasterExtension().Country);
            item.DisasterExtension().ActivationId = 123;
            Assert.Equal(123, item.DisasterExtension().ActivationId);
            item.DisasterExtension().CallIds = new int[2] { 123, 456 };
            Assert.Equal(123, item.DisasterExtension().CallIds.First());
            Assert.Equal(456, item.DisasterExtension().CallIds.Last());
            item.DisasterExtension().Regions = new string[3] { "Belgium", "Wallonie", "Namur" };
            Assert.Equal("Belgium", item.DisasterExtension().Regions.First());
            Assert.Equal("Namur", item.DisasterExtension().Regions.Last());
            item.DisasterExtension().Types = new DisastersType[3] { DisastersType.Earthquake, DisastersType.FloodFlash, DisastersType.Other };
            Assert.Equal(DisastersType.Earthquake, item.DisasterExtension().Types.First());
            Assert.Equal(DisastersType.Other, item.DisasterExtension().Types.Last());
            item.DisasterExtension().ActivationStatus = DisastersActivationStatus.Closed;
            Assert.Equal(DisastersActivationStatus.Closed, item.DisasterExtension().ActivationStatus);
            item.DisasterExtension().ResolutionClass = DisastersResolutionClass.HR;
            Assert.Equal(DisastersResolutionClass.HR, item.DisasterExtension().ResolutionClass);
            


        }

    }
}
