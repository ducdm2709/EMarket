using EMarket.Areas.Client.Helpers;
using System;
using System.Linq;
using System.Text;

namespace EMarket.Areas.Client.Services
{
    public class HelperService
    {

        private StringBuilder sb = new StringBuilder();

        public string InjectMetaInfo(string urlSegment)
        {
            var metaInfo = SEOPageMetaHelper.Collections.Where(s => s.Item1.Equals(urlSegment, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

            //If no match found  
            if (metaInfo == null)
                return string.Empty;

            sb.Append("<title>" + metaInfo.Item2 + "</title>");
            sb.Append(Environment.NewLine);
            sb.Append($"<meta name='description' content='{metaInfo.Item3}'/>");
            sb.Append(Environment.NewLine);
            sb.Append($"<meta name='keywords' content ='{metaInfo.Item4}'/>");
            string metaTag = sb.ToString();
            sb = null;
            return metaTag;
        }
    }
}
