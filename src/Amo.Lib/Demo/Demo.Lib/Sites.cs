using System.Collections.Generic;
using System.Linq;

namespace Demo.Lib
{
    public class Sites : Amo.Lib.SiteConst
    {
        public const string AAA = "AAA";
        public const string ABC = "ABC";
        public const string AFK = "AFK";

        public static List<string> GetSites()
        {
            return typeof(Sites).GetFields().Select(q => q.Name).Where(q => q != Sites.NNN).ToList();
        }
    }
}
