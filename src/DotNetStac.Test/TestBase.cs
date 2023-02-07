// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: TestBase.cs

using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Schema;
using Stac.Schemas;

namespace Stac.Test
{
    public abstract class TestBase
    {
        private static readonly Assembly ThisAssembly = typeof(TestBase)
#if NETCOREAPP1_1
        .GetTypeInfo()
#endif
        .Assembly;
        private static readonly string AssemblyName = ThisAssembly.GetName().Name;

        private static readonly StacValidator stacValidator = new(new JSchemaUrlResolver());

        protected HttpClient httpClient = new();

        protected TestBase()
        {
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = ThisAssembly.Location;
                return Path.GetDirectoryName(codeBase);
            }
        }

        protected string GetJson(string folder, [CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources", folder, type + "_" + name + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }

        protected Uri GetUri(string folder, [CallerMemberName] string name = null)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources", folder, type + "_" + name + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return new Uri(path);
        }

        protected Uri GetUseCaseFileUri(string name)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources/UseCases", type, name);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return new Uri(path);
        }

        protected string GetUseCaseJson(string name)
        {
            var type = GetType().Name;
            var path = Path.Combine(AssemblyDirectory, @"../../..", "Resources/UseCases", type, name);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("file not found at " + path);
            }

            return File.ReadAllText(path);
        }

        public bool ValidateJson(string jsonstr)
        {
            return stacValidator.ValidateJson(jsonstr);
        }

    }
}
