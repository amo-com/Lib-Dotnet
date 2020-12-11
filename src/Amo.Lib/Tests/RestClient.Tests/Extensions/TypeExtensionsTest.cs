using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Extensions
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void IsAllowMultipleTest()
        {
            Assert.True(typeof(TestAttribute).IsAllowMultiple());
        }

        [Fact]
        public void IsInheritFromTest()
        {
            Assert.True(typeof(PollyPolicyTestApi).IsInheritFrom<IPollyPolicyTestApi>());
            Assert.False(typeof(PollyPolicyTestApi).IsInheritFrom<IAubTestApi>());
        }

        [Fact]
        public void ToReadOnlyListTest()
        {
            List<int> list1 = null;
            Assert.Throws<ArgumentNullException>(() => list1.ToReadOnlyList());

            List<int> list2 = new List<int>();
            var l2 = list2.ToReadOnlyList();
            Assert.NotNull(l2);
        }

        [Fact]
        public void UrlEncodeTest()
        {
            var str1 = "abc".UrlEncode(Encoding.ASCII);
            Assert.Equal("abc", str1);

            var str2 = "ab+c".UrlEncode(Encoding.ASCII);
            Assert.Equal("ab%2bc", str2);

            var str3 = "ab c".UrlEncode(Encoding.ASCII);
            Assert.Equal("ab%20c", str3);

            string s1 = null;
            var str4 = s1.UrlEncode(Encoding.ASCII);
            Assert.Null(str4);
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        private class TestAttribute : Attribute
        {
        }
    }
}
