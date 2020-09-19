using Amo.Lib.RestClient.Contexts;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Contexts
{
    public class ApiParameterDescriptorTest
    {
        [Fact]
        public void NewTest()
        {
            var p1 = typeof(IAubTestApi).GetMethod("PostAsync").GetParameters()[0];
            var m1 = new ApiParameterDescriptor(p1);
            Assert.True(m1.Attributes.Count == 1);
            Assert.True(m1.Name == p1.Name);
            Assert.True(m1.Index == 0);
            Assert.True(m1.Value == null);

            var p2 = typeof(IAubTestApi).GetMethod("PostAsync").GetParameters()[1];
            var m2 = new ApiParameterDescriptor(p2);
            Assert.True(m2.Attributes.Count == 1);
            Assert.True(m2.Name == p2.Name);
            Assert.True(m2.Index == 1);
            Assert.True(m2.Value == null);
        }

        [Fact]
        public void CloneTest()
        {
            var p1 = typeof(IAubTestApi).GetMethod("PostAsync").GetParameters()[0];
            var m1 = new ApiParameterDescriptor(p1);
            var m2 = m1.Clone("m0");

            Assert.True(m1.Attributes == m2.Attributes);
            Assert.True(m1.Name == m2.Name);
            Assert.True(m1.Index == m2.Index);
            Assert.True((string)m2.Value == "m0");
        }
    }
}
