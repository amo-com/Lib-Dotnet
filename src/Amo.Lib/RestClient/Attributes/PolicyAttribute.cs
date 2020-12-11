using System;

namespace Amo.Lib.RestClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class PolicyAttribute : Attribute, IPolicyAttribute
    {
        public abstract IPolicyConfig CreatePolicy();
    }
}
