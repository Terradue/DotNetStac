// Copyright (c) by Terradue Srl. All Rights Reserved.
// License under the AGPL, Version 3.0.
// File Name: JsonMergeUtils.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Stac.Common
{
    public class JsonMergeUtils
    {

        /// <summary>
        /// Return the result of merging the original JSON document with the JSON Merge patch document
        /// according to https://tools.ietf.org/html/rfc7386 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="patch"></param>
        /// <param name="writerOptions">Writer options used to write the merge result.</param>
        /// <returns>The document that represents the merge result.</returns>
        public static string Merge(string original, string patch, JsonWriterOptions? writerOptions = null)
        {
            var memStream = new MemoryStream();

            using (var originalDoc = JsonDocument.Parse(original))
            using (var patchDoc = JsonDocument.Parse(patch))
            using (var jsonWriter = new Utf8JsonWriter(memStream, writerOptions ?? new JsonWriterOptions { Indented = true }))
            {

                var originalKind = originalDoc.RootElement.ValueKind;
                var patchKind = patchDoc.RootElement.ValueKind;

                if (originalKind != JsonValueKind.Object)
                {
                    throw new InvalidOperationException($"The original JSON document to merge new content into must be an object type. Instead it is {originalKind}.");
                }

                if (patchKind != JsonValueKind.Object)
                {
                    throw new InvalidOperationException($"The patch JSON document must be an object type. Instead it is {originalKind}.");
                }

                if (originalKind != patchKind)
                {
                    return original;
                }

                MergeObjects(jsonWriter, originalDoc.RootElement, patchDoc.RootElement);
            }

            return Encoding.UTF8.GetString(memStream.ToArray());
        }

        /// <summary>
        /// Return the result of merging the original JSON document with the JSON Merge patch document
        /// according to https://tools.ietf.org/html/rfc7386 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="patch"></param>
        /// <param name="token"></param>
        /// <param name="writerOptions">Writer options used to write the merge result.</param>
        public static async Task<string> MergeAsync(string original, Stream patch, CancellationToken token = default, JsonWriterOptions? writerOptions = null)
        {
            var outputBuffer = new MemoryStream();
            var jsonDocumentOptions = new JsonDocumentOptions();
            using (var originalDoc = JsonDocument.Parse(original, jsonDocumentOptions))
            using (var patchDoc = await JsonDocument.ParseAsync(patch, jsonDocumentOptions, token))
            using (var jsonWriter = new Utf8JsonWriter(outputBuffer, writerOptions ?? new JsonWriterOptions { Indented = true }))
            {
                var originalKind = originalDoc.RootElement.ValueKind;
                var patchKind = patchDoc.RootElement.ValueKind;

                if (originalKind != JsonValueKind.Object)
                {
                    throw new InvalidOperationException($"The original JSON document to merge new content into must be an object type. Instead it is {originalKind}.");
                }

                if (patchKind != JsonValueKind.Object)
                {
                    throw new InvalidOperationException($"The patch JSON document must be an object type. Instead it is {originalKind}.");
                }

                if (originalKind != patchKind)
                {
                    originalDoc.WriteTo(jsonWriter);
                }
                else
                {
                    MergeObjects(jsonWriter, originalDoc.RootElement, patchDoc.RootElement);
                }
            }

            return Encoding.UTF8.GetString(outputBuffer.ToArray());
        }

        /// <summary>
        /// Extract property names with a null value.
        /// </summary>
        /// <remarks>Nested field names are returned joined by "." 
        /// Array items are ignored.
        /// </remarks>
        /// <param name="patch"></param>
        /// <returns>The list of null properties.</returns>
        public static List<string> ExtractNullProperties(string patch)
        {
            var patchDoc = JsonDocument.Parse(patch);
            if (patchDoc.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException($"The patch JSON document must be an object type. Instead it is {patchDoc.RootElement.ValueKind}.");
            }

            return ExtractNullPropertiesFromObject(patchDoc.RootElement).ToList();
        }

        /// <summary>
        /// Extract property names with a null value.
        /// </summary>
        /// <remarks>Nested field names are returned joined by "." 
        /// Array items are ignored.
        /// </remarks>
        /// <param name="patch"></param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>The list of null properties.</returns>
        public static async Task<List<string>> ExtractNullPropertiesAsync(Stream patch, CancellationToken token = default)
        {
            var patchDoc = await JsonDocument.ParseAsync(patch, new JsonDocumentOptions(), token);
            if (patchDoc.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException($"The patch JSON document must be an object type. Instead it is {patchDoc.RootElement.ValueKind}.");
            }

            return ExtractNullPropertiesFromObject(patchDoc.RootElement).ToList();
        }

        /// <summary>
        /// Apply the result of a JSON merge patch to the given model, using System.Text.Json serializer
        /// to serialize and deserialize the model.
        /// </summary>
        /// <typeparam name="T">the model type</typeparam>
        /// <param name="original"></param>
        /// <param name="patch"></param>
        /// <param name="options">JSON serialization options</param>
        /// <returns>A new model representing the patched instance.</returns>
        public static T MergeModel<T>(T original, string patch, JsonSerializerOptions options = null)
        {
            var originalJson = JsonSerializer.Serialize(original, options);
            return JsonSerializer.Deserialize<T>(Merge(originalJson, patch), options);
        }

        /// <summary>
        /// Apply the result of a JSON merge patch to the given model, using System.Text.Json serializer
        /// to serialize and deserialize the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="patch"></param>
        /// <param name="options">JSON serialization options</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>A task that returns a new model representing the patched instance.</returns>
        public static async Task<T> MergeModelAsync<T>(T original, Stream patch, JsonSerializerOptions options = null, CancellationToken token = default)
        {
            var originalJson = JsonSerializer.Serialize(original, options);
            return JsonSerializer.Deserialize<T>(await MergeAsync(originalJson, patch, token), options);
        }

        private static IEnumerable<string> ExtractNullPropertiesFromObject(JsonElement patch)
        {
            Debug.Assert(patch.ValueKind == JsonValueKind.Object);
            foreach (var property in patch.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Null)
                {
                    yield return property.Name;
                }
                else if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    foreach (var field in ExtractNullPropertiesFromObject(property.Value))
                    {
                        yield return string.Join(".", property.Name, field);
                    }
                }
            }
        }

        private static void MergeObjects(Utf8JsonWriter jsonWriter, JsonElement original, JsonElement patch)
        {
            Debug.Assert(original.ValueKind == JsonValueKind.Object);
            Debug.Assert(patch.ValueKind == JsonValueKind.Object);

            jsonWriter.WriteStartObject();

            // Write all the properties of the original document.
            // If a property exists in both documents, either:
            // * Merge them, if they are both objects
            // * Completely override the value of the original with the one from the patch, if the value kind mismatches (e.g. one is object, while the other is an array or string)
            // * Ignore the original property if the patch property value is null
            foreach (var property in original.EnumerateObject())
            {
                if (patch.TryGetProperty(property.Name, out JsonElement patchPropValue))
                {
                    if (patchPropValue.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }

                    jsonWriter.WritePropertyName(property.Name);

                    var propValue = property.Value;

                    if (patchPropValue.ValueKind == JsonValueKind.Object && propValue.ValueKind == JsonValueKind.Object)
                    {
                        MergeObjects(jsonWriter, propValue, patchPropValue); // Recursive call
                    }
                    else
                    {
                        patchPropValue.WriteTo(jsonWriter);
                    }
                }
                else
                {
                    property.WriteTo(jsonWriter);
                }
            }

            // Write all the properties of the patch document that are unique to it (beside null values).
            foreach (var property in patch.EnumerateObject())
            {
                if (!original.TryGetProperty(property.Name, out JsonElement patchPropValue) && patchPropValue.ValueKind != JsonValueKind.Null)
                {
                    property.WriteTo(jsonWriter);
                }
            }

            jsonWriter.WriteEndObject();
        }
    }
}
