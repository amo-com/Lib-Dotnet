namespace Demo.Lib.Models
{
    public class InterceptLogEntity : Amo.Lib.Model.LogEntity
    {
        public long Latency { get; set; }
        public string Version { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
    }
}
