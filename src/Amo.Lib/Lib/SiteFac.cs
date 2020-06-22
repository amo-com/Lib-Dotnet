namespace Amo.Lib
{
    public class SiteFac : ISite
    {
        private readonly string site;
        public SiteFac(string site)
        {
            this.site = site;
        }

        public string GetSite()
        {
            return site;
        }
    }
}
