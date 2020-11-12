using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using ZTn.System.Text.Json.Helpers;

namespace ZTn.Json.Helpers.UnitTest
{
    [TestFixture]
    public class JsonHelpersUnitTest
    {
        private const string IntArrayJson = "[31,32,33,34]";
        private const string PiObjectJson = "{\"Key\":\"Pi\",\"Value\":3.14159}";
        private const string PiObjectJsonNetFramework = "{\"Key\":\"Pi\",\"Value\":3.1415899999999999}";

        [Test]
        public void IntArrayToJson()
        {
            var result = new[] { 31, 32, 33, 34 }.ToJson();

            Assert.AreEqual(IntArrayJson, result);
        }

        [Test]
        public void KeyValuePairToJson()
        {
            var result = new KeyValuePair<string, double>("Pi", 3.14159).ToJson();

#if NETFRAMEWORK
            Assert.AreEqual(PiObjectJsonNetFramework, result, "Expected to fail with .net core (https://github.com/dotnet/runtime/issues/435)");
#else
            Assert.AreEqual(PiObjectJson, result, "Expected to fail with .net 4.x framework (https://github.com/dotnet/runtime/issues/435)");
#endif
        }


        [Test]
        public void KeyValuePairToIndentedJson()
        {
            var result = new KeyValuePair<string, double>("Pi", 3.14159).ToPrettyJson();
#if NETFRAMEWORK
            var expected =
                "{" + Environment.NewLine +
                "  \"Key\": \"Pi\"," + Environment.NewLine +
                "  \"Value\": 3.1415899999999999" + Environment.NewLine +
                "}";
#else
            var expected =
                "{" + Environment.NewLine +
                "  \"Key\": \"Pi\"," + Environment.NewLine +
                "  \"Value\": 3.14159" + Environment.NewLine +
                "}";
#endif

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void JsonToIntArray()
        {
            var result = "[31, 32, 33, 34]".FromJson<int[]>();

            CollectionAssert.AreEqual(new[] { 31, 32, 33, 34 }, result);
        }

        [Test]
        public void JsonToKeyValuePair()
        {
            var result = PiObjectJson.FromJson<KeyValuePair<string, double>>();

            Assert.AreEqual("Pi", result.Key);
            Assert.AreEqual(3.14159, result.Value);
        }

        [Test]
        public void JsonStreamToKeyValuePair()
        {
            KeyValuePair<string, double> result;
            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(PiObjectJson)))
            {
                result = memoryStream.ReadAsJson<KeyValuePair<string, double>>();
                Assert.DoesNotThrow(() => memoryStream.Position = 0);
            }

            Assert.AreEqual("Pi", result.Key);
            Assert.AreEqual(3.14159, result.Value);
        }
    }
}
