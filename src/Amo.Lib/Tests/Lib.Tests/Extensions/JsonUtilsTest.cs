using Amo.Lib.Extensions;
using Xunit;

namespace Amo.Lib.Tests.Extensions
{
    public class JsonUtilsTest
    {
        [Fact]
        public void DeserializeTest()
        {
            string userStr = "{\"Id\":11, \"Name\":\"Xiaoming\"}";
            var userDo = JsonUtils.Deserialize<UserDo>(userStr);
            Assert.NotNull(userDo);
            Assert.Equal(11, userDo.Id);
            Assert.Equal("Xiaoming", userDo.Name);

            string otherStr = "\"Key\":11, \"Index\":\"Xiaoming\"}";
            Assert.Throws<Newtonsoft.Json.JsonSerializationException>(() => JsonUtils.Deserialize<UserDo>(otherStr));

            string otherStr2 = "{\"Key\":11, \"Index\":\"Xiaoming\"}";
            var userDo2 = JsonUtils.Deserialize<UserDo>(otherStr2);
            Assert.NotNull(userDo2);
            Assert.Equal(0, userDo2.Id);
            Assert.Null(userDo2.Name);
        }

        [Fact]
        public void DeserializeTest2()
        {
            string userStr = "{\"Id\":11, \"Name\":\"Xiaoming\"}";
            var jsonDo = JsonUtils.Deserialize(userStr, typeof(UserDo));
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
            Assert.Equal("{\"Id\":20,\"Name\":\"Po\"}", JsonUtils.Serialize(userDo));
        }

        [Fact]
        public void SerializeTest2()
        {
            var userDo = new UserDo() { Id = 20, Name = "Po" };
            Assert.Equal("{\"Id\":20,\"Name\":\"Po\"}", JsonUtils.Serialize(userDo, typeof(UserDo)));
        }

        [Fact]
        public void ToJsonTest()
        {
            var userDo = new UserDo() { Id = 20, Name = "Po" };
            Assert.Equal("{\"Id\":20,\"Name\":\"Po\"}", userDo.ToJson());
        }

        public class UserDo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
