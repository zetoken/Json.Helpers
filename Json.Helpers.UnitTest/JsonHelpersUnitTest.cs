using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace ZTn.Json.Helpers.UnitTest
{
    [TestFixture]
    public class JsonHelpersUnitTest
    {
        private const string IntArrayJson = "[31,32,33,34]";
        private const string PiObjectJson = "{\"Key\":\"Pi\",\"Value\":3.14159}";

        [Test]
        public void IntArrayToJson()
        {
            var result = new[] { 31, 32, 33, 34 }.WriteToJsonString();

            Assert.AreEqual(IntArrayJson, result);
        }

        [Test]
        public void KeyValuePairToJson()
        {
            var result = new KeyValuePair<string, double>("Pi", 3.14159).WriteToJsonString();

            Assert.AreEqual(PiObjectJson, result);
        }

        [Test]
        public void KeyValuePairToIndentedJson()
        {
            var result = new KeyValuePair<string, double>("Pi", 3.14159).WriteToJsonString(true);
            var expected =
                "{" + Environment.NewLine +
                "  \"Key\": \"Pi\"," + Environment.NewLine +
                "  \"Value\": 3.14159" + Environment.NewLine +
                "}";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void JsonToIntArray()
        {
            var result = "[31, 32, 33, 34]".CreateFromJsonString<int[]>();

            CollectionAssert.AreEqual(new[] { 31, 32, 33, 34 }, result);
        }

        [Test]
        public void JsonToKeyValuePair()
        {
            var result = PiObjectJson.CreateFromJsonString<KeyValuePair<string, double>>();

            Assert.AreEqual("Pi", result.Key);
            Assert.AreEqual(3.14159, result.Value);
        }

        [Test]
        public void JsonStreamToKeyValuePair()
        {
            KeyValuePair<string, double> result;
            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(PiObjectJson)))
            {
                result = memoryStream.CreateFromJsonStream<KeyValuePair<string, double>>();
                Assert.Throws<ObjectDisposedException>(() => memoryStream.Position = 0);
            }

            Assert.AreEqual("Pi", result.Key);
            Assert.AreEqual(3.14159, result.Value);
        }

        [Test]
        public void JsonStreamToKeyValuePairWithPersistence()
        {
            KeyValuePair<string, double> result;
            using (var memoryStream = new MemoryStream(Encoding.Default.GetBytes(PiObjectJson)))
            {
                result = memoryStream.CreateFromJsonPersistentStream<KeyValuePair<string, double>>();
                Assert.DoesNotThrow(() => memoryStream.Position = 0);
            }

            Assert.AreEqual("Pi", result.Key);
            Assert.AreEqual(3.14159, result.Value);
        }
    }
}
