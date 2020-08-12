using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZTn.System.Text.Json.Helpers.Migration
{
    /// <summary>
    /// Provides some extension methods for convenient migration from ZTn.Json.Helpers to Ztn.System.Text.Json.Helpers serializer (same .
    /// </summary>
    public static class JsonHelpers
    {
        public static T CreateFromJsonStream<T>(this Stream stream)
        {
            var instance = stream.ReadAsJson<T>();

            stream.Dispose();

            return instance;
        }

        public static T CreateFromJsonPersistentStream<T>(this Stream stream)
        {
            return stream.ReadAsJson<T>();
        }

        public static T CreateFromJsonString<T>(this string json)
        {
            return json.FromJson<T>();
        }

        public static T CreateFromJsonString<T>(this string json, Encoding encoding)
        {
            return json.FromJson<T>();
        }

        public static T CreateFromJsonFile<T>(this string fileName)
        {
            return fileName.ReadAsJson<T>();
        }

        public static void WriteToJsonFile(this object instance, string fileName)
        {
            instance.WriteAsJson(fileName);
        }

        public static void WriteToJsonFile(this object instance, string fileName, bool indented)
        {
            if (indented)
            {
                instance.WriteAsPrettyJson(fileName);
            }
            else
            {
                instance.WriteAsJson(fileName);
            }
        }

        public static string WriteToJsonString(this object instance, params JsonConverter[] jsonConverters)
        {
            var jsonOptions = new JsonSerializerOptions();
            foreach (var converter in jsonConverters)
            {
                jsonOptions.Converters.Add(converter);
            }

            return instance.ToJson(jsonOptions);
        }

        public static string WriteToJsonString(this object instance, bool indented, params JsonConverter[] jsonConverters)
        {
            var jsonOptions = new JsonSerializerOptions();
            foreach (var converter in jsonConverters)
            {
                jsonOptions.Converters.Add(converter);
            }

            return indented ? instance.ToPrettyJson(jsonOptions) : instance.ToJson(jsonOptions);
        }
    }
}
