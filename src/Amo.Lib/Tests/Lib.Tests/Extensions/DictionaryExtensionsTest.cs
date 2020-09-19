using Amo.Lib.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Amo.Lib.Tests.Extensions
{
    public class DictionaryExtensionsTest
    {
        [Fact]
        public void RenameKeyTest()
        {
            Dictionary<int, int> attr = null;
            attr.RenameKey(6, 66);
            Assert.Null(attr);

            attr = GetBaseDic();

            attr.RenameKey(2, 12);
            Assert.Equal(22, attr[12]);

            attr.RenameKey(6, 16);
            Assert.False(attr.ContainsKey(16));
        }

        [Fact]
        public void CloneTest()
        {
            Dictionary<int, int> attr = null;

            var attrNew = attr.Clone();
            Assert.Null(attrNew);

            attr = GetBaseDic();
            attrNew = attr.Clone();
            Assert.NotNull(attrNew);
            Assert.NotNull(attr);
            Assert.Equal(attr.Count, attrNew.Count);
            Assert.Equal(attr[1], attrNew[1]);
            Assert.Equal(attr[3], attrNew[3]);
        }

        [Fact]
        public void InsertBeforeAfterTest()
        {
            Dictionary<int, int> attr = null;

            var b0 = attr.Insert(2, 22, 22);
            Assert.False(b0);
            Assert.Null(attr);

            attr = GetBaseDic();
            var b1 = attr.Insert(6, 22, 22);
            Assert.False(b1);

            attr = GetBaseDic();
            var b2 = attr.Insert(2, 22, 22);
            Assert.True(b2);
            Assert.Equal(22, attr[22]);
            Assert.Equal(new List<int> { 1, 2, 22, 3 }, attr.Keys.ToList());

            attr = GetBaseDic();
            var b3 = attr.Insert(2, 12, 12, true);
            Assert.True(b3);
            Assert.Equal(12, attr[12]);
            Assert.Equal(new List<int> { 1, 12, 2, 3 }, attr.Keys.ToList());
        }

        [Fact]
        public void InsertBeforeTest()
        {
            var attr = GetBaseDic();
            var b3 = attr.InsertBefore(2, 12, 12);
            Assert.True(b3);
            Assert.Equal(12, attr[12]);
            Assert.Equal(new List<int> { 1, 12, 2, 3 }, attr.Keys.ToList());
        }

        [Fact]
        public void InsertAfterTest()
        {
            var attr = GetBaseDic();
            var b2 = attr.Insert(2, 22, 22);
            Assert.True(b2);
            Assert.Equal(22, attr[22]);
            Assert.Equal(new List<int> { 1, 2, 22, 3 }, attr.Keys.ToList());
        }

        [Fact]
        public void InsertIndex()
        {
            Dictionary<int, int> attr = null;

            var b0 = attr.Insert(2, 11, 11);
            Assert.False(b0);
            Assert.Null(attr);

            attr = GetBaseDic();
            var b1 = attr.Insert(2, 11, 11);
            Assert.True(b1);
            Assert.Equal(11, attr[11]);
            Assert.Equal(new List<int> { 1, 2, 11, 3 }, attr.Keys.ToList());

            attr = GetBaseDic();
            var b2 = attr.Insert(3, 11, 11);
            Assert.True(b2);
            Assert.Equal(11, attr[11]);
            Assert.Equal(new List<int> { 1, 2, 3, 11 }, attr.Keys.ToList());

            attr = GetBaseDic();
            var b3 = attr.Insert(5, 11, 11);
            Assert.False(b3);
            Assert.Equal(3, attr.Count);
            Assert.False(attr.ContainsKey(11));
        }

        private Dictionary<int, int> GetBaseDic()
        {
            return new Dictionary<int, int>
            {
                [1] = 11,
                [2] = 22,
                [3] = 33
            };
        }
    }
}
