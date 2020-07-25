namespace Amo.Lib.CoreApi.Model
{
    public class VersionVo
    {
        public string Version1 { get; set; }
        public string Version2 { get; set; }
        public string Version => $"{Version1}-{Version2}";
    }
}
