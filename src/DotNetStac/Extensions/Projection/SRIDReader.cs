﻿// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: SRIDReader.cs

using System.Collections.Generic;
using System.IO;
using ProjNet.CoordinateSystems;
using ProjNet.IO.CoordinateSystems;

namespace Stac.Extensions.Projection
{
    internal class SRIDReader
    {
        private const string Filename = @"SRID.csv";

        /// <summary>
        /// Enumerates all SRID's in the SRID.csv file.
        /// </summary>
        /// <returns>Enumerator</returns>
        public static IEnumerable<WKTstring> GetSRIDs()
        {
            using (StreamReader sr = System.IO.File.OpenText(Filename))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    int split = line.IndexOf(';');
                    if (split > -1)
                    {
                        WKTstring wkt = default(WKTstring);
                        wkt.WKID = int.Parse(line.Substring(0, split));
                        wkt.WKT = line.Substring(split + 1);
                        yield return wkt;
                    }
                }

                sr.Close();
            }
        }

        /// <summary>
        /// Gets a coordinate system from the SRID.csv file
        /// </summary>
        /// <param name="id">EPSG ID</param>
        /// <returns>Coordinate system, or null if SRID was not found.</returns>
        public static CoordinateSystem GetCSbyID(int id)
        {
            CoordinateSystemFactory fac = new CoordinateSystemFactory();
            foreach (WKTstring wkt in GetSRIDs())
            {
                if (wkt.WKID == id)
                {
                    return CoordinateSystemWktReader.Parse(wkt.WKT) as CoordinateSystem;
                }
            }

            return null;
        }

        public struct WKTstring
        {
            /// <summary>
            /// Well-known ID
            /// </summary>
            public int WKID;

            /// <summary>
            /// Well-known Text
            /// </summary>
            public string WKT;
        }
    }
}
