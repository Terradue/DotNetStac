using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Stac.Extensions;
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

        private StacValidator stacValidator = new StacValidator(new JSchemaUrlResolver());

        protected HttpClient httpClient = new HttpClient();

        protected TestBase()
        {
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = ThisAssembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
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