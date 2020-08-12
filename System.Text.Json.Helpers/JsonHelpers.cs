using System;
using System.IO;
using System.Text.Json;

namespace ZTn.System.Text.Json.Helpers
{
    /// <summary>
    /// Provides some extension methods for convenient use of System.Text.Json serializer.
    /// </summary>
    public static class JsonHelpers
    {
        /// <summary>
        /// Creates an instance of type <typeparamref Name="T"/> from a JSON string.
        /// </summary>
        /// <typeparam name="T">Target instance type.</typeparam>
        /// <param name="json">JSON source string.</param>
        /// <param name="options">JSON options.</param>
        /// <returns>A new instance of <typeparamref Name="T"/> read from <paramref Name="json"/>.</returns>
        public static T FromJson<T>(this string json, JsonSerializerOptions options = null)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            return (T)JsonSerializer.Deserialize(json, typeof(T), options);
        }

        /// <summary>
        /// Reads (deserializes) an instance of type <typeparamref Name="T"/> from a JSON stream.
        /// The stream is safely disposed by the method.
        /// </summary>
        /// <typeparam name="T">Target instance type.</typeparam>
        /// <param name="stream">JSON source stream.</param>
        /// <param name="options">JSON options.</param>
        /// <returns>A new instance of <typeparamref Name="T"/> read from <paramref Name="stream"/>.</returns>
        public static T ReadAsJson<T>(this Stream stream, JsonSerializerOptions options = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return (T)JsonSerializer.DeserializeAsync(stream, typeof(T), options).Result;
        }

        /// <summary>
        /// Reads (deserializes) an instance of type <typeparamref Name="T"/> from a JSON file.
        /// </summary>
        /// <typeparam name="T">Target instance type.</typeparam>
        /// <param name="fileName">Path to source JSON file.</param>
        /// <param name="options">JSON options.</param>
        /// <returns>A new instance of <typeparamref Name="T"/> read from <paramref Name="fileName"/>.</returns>
        public static T ReadAsJson<T>(this string fileName, JsonSerializerOptions options = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                return (T)JsonSerializer.DeserializeAsync(stream, typeof(T), options).Result;
            }
        }

        /// <summary>
        /// Converts (serializes) an instance to a JSON string.
        /// </summary>
        /// <param name="instance">Source instance.</param>
        /// <param name="options">JSON options.</param>
        public static string ToJson(this object instance, JsonSerializerOptions options = null)
        {
            return JsonSerializer.Serialize(instance, options);
        }

        /// <summary>
        /// Converts (serializes) an instance to a pretty JSON string (indented).
        /// </summary>
        /// <param name="instance">Source instance.</param>
        /// <param name="options">JSON options.</param>
        public static string ToPrettyJson(this object instance, JsonSerializerOptions options = null)
        {
            return JsonSerializer.Serialize(instance, options.MakeItPretty());
        }

        /// <summary>
        /// Writes (serializes) an instance to a stream as JSON.
        /// </summary>
        /// <param name="instance">Source instance.</param>
        /// <param name="stream">Target stream.</param>
        /// <param name="options">JSON options.</param>
        public static void WriteAsJson(this object instance, Stream stream, JsonSerializerOptions options = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var jsonOptions = options.MakeItPretty();

            using (var jsonWriter = new Utf8JsonWriter(stream, jsonOptions.ToJsonWriterOptions()))
            {
                JsonSerializer.Serialize(jsonWriter, instance, jsonOptions);
            }
        }

        /// <summary>
        /// Writes (serializes) an instance to a JSON file.
        /// </summary>
        /// <param name="instance">Source instance.</param>
        /// <param name="fileName">Path to target JSON file.</param>
        /// <param name="options">JSON options.</param>
        public static void WriteAsJson(this object instance, string fileName, JsonSerializerOptions options = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                instance.WriteAsJson(stream, options);
            }
        }

        /// <summary>
        /// Writes (serializes) an instance to a stream as pretty JSON (indented).
        /// </summary>
        /// <param name="instance">Source instance.</param>
        /// <param name="stream">Target stream.</param>
        /// <param name="options">JSON options.</param>
        public static void WriteAsPrettyJson(this object instance, Stream stream, JsonSerializerOptions options = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var jsonOptions = options.MakeItPretty();

            using (var jsonWriter = new Utf8JsonWriter(stream, jsonOptions.ToJsonWriterOptions()))
            {
                JsonSerializer.Serialize(jsonWriter, instance, jsonOptions);
            }
        }

        /// <summary>
        /// Writes (serializes) an instance to a pretty JSON file (indented).
        /// </summary>
        /// <param name="instance">Source instance.</param>
        /// <param name="fileName">Path to target JSON file.</param>
        /// <param name="options">JSON options.</param>
        public static void WriteAsPrettyJson(this object instance, string fileName, JsonSerializerOptions options = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                instance.WriteAsPrettyJson(stream, options);
            }
        }

        /// <summary>
        /// Creates a new <see cref="JsonSerializerOptions"/> or updates an existing one and set <see cref="JsonSerializerOptions.WriteIndented"/> to <c>true</c>.
        /// </summary>
        /// <param name="options">JSON options.</param>
        /// <returns></returns>
        private static JsonSerializerOptions MakeItPretty(this JsonSerializerOptions options)
        {
            var jsonOptions = options ?? new JsonSerializerOptions();
            jsonOptions.WriteIndented = true;

            return jsonOptions;
        }

        /// <summary>
        /// Creates a <see cref="JsonWriterOptions"/> matching the given <see cref="JsonSerializerOptions"/>.<br/>
        /// From https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializer.serialize?view=netcore-3.1:
        /// <quote>The JsonWriterOptions used to create the instance of the Utf8JsonWriter take precedence over the JsonSerializerOptions when they conflict.</quote>
        /// </summary>
        /// <param name="options">JSON options.</param>
        /// <returns></returns>
        private static JsonWriterOptions ToJsonWriterOptions(this JsonSerializerOptions options)
        {
            if (options == null)
            {
                return new JsonWriterOptions();
            }

            return new JsonWriterOptions
            {
                Encoder = options.Encoder,
                Indented = options.WriteIndented
            };
        }
    }
}
