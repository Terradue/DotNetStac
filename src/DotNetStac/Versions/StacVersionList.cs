using System;
using Semver;

namespace Stac.Versions
{
    public static class StacVersionList
    {
        public static SemVersion Current => V100rc2;

        public static SemVersion V100rc2 => new SemVersion(1, 0, 0, "rc.2");

        public static SemVersion V100rc1 => new SemVersion(1, 0, 0, "rc.1");

        public static SemVersion V100beta2 => new SemVersion(1, 0, 0, "beta.2");

        public static SemVersion V060 => new SemVersion(0, 6, 0);

        public static SemVersion V070 => new SemVersion(0, 7, 0);
    }
}
