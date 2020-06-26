using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.Areas.Client.Helpers
{
    public static class SEOPageMetaHelper
    {
        public static IEnumerable<Tuple<string, string, string, string>> Collections
        {
            get
            {
                return new List<Tuple<string, string, string, string>>
                {
                   new Tuple<string, string, string, string>("HangHoa/Index", "Dynamic Index", "Index Description","Hàng Hóa, Emarket Page"),
                   new Tuple<string, string, string, string>("HangHoa/Detail", "Dynamic About",  "About Description","keyword3,keyword4"),
                   new Tuple<string, string, string, string>("HangHoa/Contact", "Dynamic Contact", "Contact Description","keyword5,keyword6")
                };
            }
        }
    }
}
