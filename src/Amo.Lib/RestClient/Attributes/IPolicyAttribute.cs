namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// Polly策略属性
    /// </summary>
    public interface IPolicyAttribute
    {
        /// <summary>
        /// 转换为PolicyConfig(通过Attribute配置PolicyConfig)
        /// </summary>
        /// <returns>PolicyConfig</returns>
        IPolicyConfig CreatePolicy();
    }
}
