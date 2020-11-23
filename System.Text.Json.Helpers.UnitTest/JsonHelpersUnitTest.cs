using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ZTn.System.Text.Json.Helpers;

namespace ZTn.Json.Helpers.UnitTest
{
    [TestFixture]
    public class JsonHelpersUnitTest
    {
        private const string IntArrayJson = "[31,32,33,34]";
#if NETFRAMEWORK
        private const string PiObjectJson = "{\"Key\":\"Pi\",\"Value\":3.1415899999999999}";
#else
        private const string PiObjectJson = "{\"Key\":\"Pi\",\"Value\":3.14159}";
#endif
        private const string SomeAAsJson = "{\"S\":\"Some A\",\"A\":\"MjAyMA==\",\"B\":{\"I\":42}}";
        private readonly string _someAAsPrettyJson;

        private SomeA SomeAInstance { get; }

        public JsonHelpersUnitTest()
        {
            SomeAInstance = new SomeA("Some A", new byte[] { 0x32, 0x30, 0x32, 0x30 }, new SomeB(42));
            _someAAsPrettyJson =
                "{" + Environment.NewLine +
                "  \"S\": \"Some A\"," + Environment.NewLine +
                "  \"A\": \"MjAyMA==\"," + Environment.NewLine +
                "  \"B\": {" + Environment.NewLine +
                "    \"I\": 42" + Environment.NewLine +
                "  }" + Environment.NewLine +
                "}";
        }

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

            Assert.AreEqual(PiObjectJson, result, "Expected to fail with .net core (https://github.com/dotnet/runtime/issues/435)");
            Assert.AreEqual(PiObjectJson, result, "Expected to fail with .net 4.x framework (https://github.com/dotnet/runtime/issues/435)");
        }

        [Test]
        public void KeyValuePairAsPrettyJson()
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
            using var memoryStream = new MemoryStream(Encoding.Default.GetBytes(PiObjectJson));

            var result = memoryStream.ReadAsJson<KeyValuePair<string, double>>();

            Assert.DoesNotThrow(() => memoryStream.Position = 0);

            Assert.AreEqual("Pi", result.Key);
            Assert.AreEqual(3.14159, result.Value);
        }

        [Test]
        public async Task ReadAsJsonAsync()
        {
            using var memoryStream = new MemoryStream(Encoding.Default.GetBytes(SomeAAsJson));

            var someA = await memoryStream.ReadAsJsonAsync<SomeA>();

            Assert.AreEqual(SomeAInstance.S, someA.S);
            Assert.AreEqual(SomeAInstance.A, someA.A);
            Assert.AreEqual(SomeAInstance.B.I, someA.B.I);
        }

        [Test]
        public void WriteAndReadAsJsonToFile()
        {
            var fileName = Path.GetRandomFileName();

            SomeAInstance.WriteAsJson(fileName);

            Assert.AreEqual(SomeAAsJson, File.ReadAllText(fileName));

            var someA = fileName.ReadAsJson<SomeA>();

            Assert.AreEqual(SomeAInstance.S, someA.S);
            Assert.AreEqual(SomeAInstance.A, someA.A);
            Assert.AreEqual(SomeAInstance.B.I, someA.B.I);
        }

        [Test]
        public async Task WriteAndReadAsJsonToFileAsync()
        {
            var fileName = Path.GetRandomFileName();

            await SomeAInstance.WriteAsJsonAsync(fileName);

            Assert.AreEqual(SomeAAsJson, File.ReadAllText(fileName));

            var someA = await fileName.ReadAsJsonAsync<SomeA>();

            Assert.AreEqual(SomeAInstance.S, someA.S);
            Assert.AreEqual(SomeAInstance.A, someA.A);
            Assert.AreEqual(SomeAInstance.B.I, someA.B.I);
        }

        [Test]
        public void WriteAsPrettyJsonToFile()
        {
            var fileName = Path.GetRandomFileName();

            SomeAInstance.WriteAsPrettyJson(fileName);

            Assert.AreEqual(_someAAsPrettyJson, File.ReadAllText(fileName));
        }

        [Test]
        public async Task WriteAsPrettyJsonToFileAsync()
        {
            var fileName = Path.GetRandomFileName();

            await SomeAInstance.WriteAsPrettyJsonAsync(fileName);

            Assert.AreEqual(_someAAsPrettyJson, File.ReadAllText(fileName));
        }

        internal class SomeA
        {
            public SomeA(string s, byte[] a, SomeB b)
            {
                S = s;
                A = a;
                B = b;
            }

            public string S { get; set; }

            public byte[] A { get; set; }

            public SomeB B { get; set; }
        }

        internal class SomeB
        {
            public SomeB(int i)
            {
                I = i;
            }

            public int I { get; set; }
        }
    }
}
