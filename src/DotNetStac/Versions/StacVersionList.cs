// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: StacVersionList.cs

using Semver;

namespace Stac.Versions
{
    /// <summary>
    /// List of STAC versions
    /// </summary>
    public static class StacVersionList
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static SemVersion Current => V100;

        public static SemVersion V100 => new SemVersion(1, 0, 0);

        public static SemVersion V100rc4 => new SemVersion(1, 0, 0, "rc.4");

        public static SemVersion V100rc3 => new SemVersion(1, 0, 0, "rc.3");

        public static SemVersion V100rc2 => new SemVersion(1, 0, 0, "rc.2");

        public static SemVersion V100rc1 => new SemVersion(1, 0, 0, "rc.1");

        public static SemVersion V100beta2 => new SemVersion(1, 0, 0, "beta.2");

        public static SemVersion V060 => new SemVersion(0, 6, 0);

        public static SemVersion V070 => new SemVersion(0, 7, 0);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
