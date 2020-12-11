using Amo.Lib.RestClient.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Extensions
{
    public class JsonExtensionsTest
    {
        [Fact]
        public void DeserializeTest()
        {
            string userStr = "{\"Id\":11, \"Name\":\"Xiaoming\"}";
            var userDo = JsonExtensions.Deserialize<UserDo>(userStr);
            Assert.NotNull(userDo);
            Assert.Equal(11, userDo.Id);
            Assert.Equal("Xiaoming", userDo.Name);

            string otherStr = "\"Key\":11, \"Index\":\"Xiaoming\"}";
            Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => JsonExtensions.Deserialize<UserDo>(otherStr));

            string otherStr2 = "{\"Key\":11, \"Index\":\"Xiaoming\"}";
            var userDo2 = JsonExtensions.Deserialize<UserDo>(otherStr2);
            Assert.NotNull(userDo2);
            Assert.Equal(0, userDo2.Id);
            Assert.Null(userDo2.Name);
        }

        [Fact]
        public void DeserializeTest2()
        {
            string userStr = "{\"Id\":11, \"Name\":\"Xiaoming\"}";
            var jsonDo = JsonExtensions.Deserialize(userStr, typeof(UserDo));
            Assert.True(jsonDo is UserDo);
            var userDo = jsonDo as UserDo;
            Assert.NotNull(userDo);
            Assert.Equal(11, userDo.Id);
            Assert.Equal("Xiaoming", userDo.Name);
        }

        [Fact]
        public void SerializeTest()
        {
            var userDo = new UserDo() { Id = 20, Name = "Po" };
            Assert.Equal("{\"Id\":20,\"Name\":\"Po\"}", JsonExtensions.Serialize(userDo));
        }

        [Fact]
        public void ToJsonTest()
        {
            var userDo = new UserDo() { Id = 20, Name = "Po" };
            Assert.Equal("{\"Id\":20,\"Name\":\"Po\"}", userDo.ToJson());
        }
    }
}
