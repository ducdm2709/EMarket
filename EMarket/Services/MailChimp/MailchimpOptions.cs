using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.Services.MailChimp
{
    public class MailchimpOptions
    {
        public string APIKey { get; set; }
        public string AudienceId { get; set; }
        public int TemplateId { get; set; }
    }
}
