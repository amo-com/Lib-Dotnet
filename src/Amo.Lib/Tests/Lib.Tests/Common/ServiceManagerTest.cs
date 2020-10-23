using System;
using System.Collections.Generic;
using Xunit;

namespace Amo.Lib.Tests.Common
{
#pragma warning disable CS0612 // 类型或成员已过时
    public class ServiceManagerTest
    {
        [Fact]
        public void GetImplementationTypesTest()
        {
            var types = ServiceManager.GetImplementationTypes(null, null);
            Assert.Empty(types);

            var scopeFacType = typeof(Impls.ScopedFac);
            var rootDemo2Type = typeof(ServiceManagerMock.RootDemo.Demo2);
            var rootDemo3Type = typeof(ServiceManagerMock.RootDemo.Demo3);

            var types1 = ServiceManager.GetImplementationTypes(new List<string>() { "Amo.Lib.Tests" }, null);
            Assert.NotEmpty(types1);
            Assert.NotNull(types1.Find(q => q == scopeFacType));
            Assert.Null(types1.Find(q => q == rootDemo2Type));
            Assert.NotNull(types1.Find(q => q == rootDemo3Type));

            var types2 = ServiceManager.GetImplementationTypes(null, new List<string>() { "Amo.Lib.Tests" });
            Assert.NotEmpty(types2);
            Assert.NotNull(types2.Find(q => q == scopeFacType));
            Assert.Null(types2.Find(q => q == rootDemo2Type));
            Assert.NotNull(types2.Find(q => q == rootDemo3Type));
        }

        [Fact]
        public void StartWithTest()
        {
            var prefixs = new List<string>() { "test", "amo", "lib" };
            Assert.True(ServiceManager.StartWith(null, null));
            Assert.True(ServiceManager.StartWith(null, prefixs));
            Assert.True(ServiceManager.StartWith(string.Empty, prefixs));
            Assert.True(ServiceManager.StartWith("abc", null));
            Assert.False(ServiceManager.StartWith("abc", prefixs));
            Assert.False(ServiceManager.StartWith("user.test", prefixs));
            Assert.True(ServiceManager.StartWith("lib.common", prefixs));
            Assert.True(ServiceManager.StartWith("test.user", prefixs));
        }

        [Fact]
        public void RemoveOverRideTypesTest()
        {
            // Root
            // Demo1,正常接口实现,无OverRide应用,不做操作
            List<Type> rootTypes = new List<Type>() { typeof(ServiceManagerMock.RootDemo.Demo1) };
            var resultRootTypes1 = ServiceManager.RemoveOverRideTypes(rootTypes, null);
            Assert.Single(resultRootTypes1);
            Assert.Equal(rootTypes, resultRootTypes1);
            Assert.Equal(typeof(ServiceManagerMock.RootDemo.Demo1).FullName, resultRootTypes1[0].FullName);

            // Demo2
            rootTypes = new List<Type>() { typeof(ServiceManagerMock.RootDemo.Demo1), typeof(ServiceManagerMock.RootDemo.Demo2) };
            var resultRootTypes2 = ServiceManager.RemoveOverRideTypes(rootTypes, null);
            Assert.Equal(2, resultRootTypes2.Count);
            Assert.Equal(rootTypes, resultRootTypes2);

            // Demo3,有OverRide标记,移除基类,保留本类
            rootTypes = new List<Type>() { typeof(ServiceManagerMock.RootDemo.Demo1), typeof(ServiceManagerMock.RootDemo.Demo3) };
            var resultRootTypes3 = ServiceManager.RemoveOverRideTypes(rootTypes, null);
            Assert.Single(resultRootTypes3);
            Assert.Equal(typeof(ServiceManagerMock.RootDemo.Demo3).FullName, resultRootTypes3[0].FullName);

            // Scoped
            // Demo1
            var scopedTypes = new List<Type>() { typeof(ServiceManagerMock.ScopedDemo.Demo1) };
            var resultScopedTypes1 = ServiceManager.RemoveOverRideTypes(scopedTypes, Data.Sites.APW);
            Assert.Single(resultScopedTypes1);
            Assert.Equal(scopedTypes, resultScopedTypes1);
            Assert.Equal(typeof(ServiceManagerMock.ScopedDemo.Demo1).FullName, resultScopedTypes1[0].FullName);

            // Demo2
            scopedTypes = new List<Type>() { typeof(ServiceManagerMock.ScopedDemo.Demo1), typeof(ServiceManagerMock.ScopedDemo.Demo2) };
            var resultScopedTypes2 = ServiceManager.RemoveOverRideTypes(scopedTypes, Data.Sites.APW);
            Assert.Equal(2, resultScopedTypes2.Count);
            Assert.Equal(scopedTypes, resultScopedTypes2);

            // Demo3
            scopedTypes = new List<Type>() { typeof(ServiceManagerMock.ScopedDemo.Demo1), typeof(ServiceManagerMock.ScopedDemo.Demo3) };
            var resultScopedTypes3 = ServiceManager.RemoveOverRideTypes(scopedTypes, Data.Sites.APW);
            Assert.Single(resultScopedTypes3);
            Assert.Equal(typeof(ServiceManagerMock.ScopedDemo.Demo3).FullName, resultScopedTypes3[0].FullName);

            // Demo4
            scopedTypes = new List<Type>() { typeof(ServiceManagerMock.ScopedDemo.Demo1), typeof(ServiceManagerMock.ScopedDemo.Demo4) };
            var resultScopedTypes4 = ServiceManager.RemoveOverRideTypes(scopedTypes, Data.Sites.APW);
            Assert.Single(resultScopedTypes4);
            Assert.Equal(typeof(ServiceManagerMock.ScopedDemo.Demo1).FullName, resultScopedTypes4[0].FullName);

            var resultScopedTypes5 = ServiceManager.RemoveOverRideTypes(scopedTypes, Data.Sites.HPN);
            Assert.Single(resultScopedTypes5);
            Assert.Equal(typeof(ServiceManagerMock.ScopedDemo.Demo4).FullName, resultScopedTypes5[0].FullName);
        }
    }
#pragma warning restore CS0612 // 类型或成员已过时
}
