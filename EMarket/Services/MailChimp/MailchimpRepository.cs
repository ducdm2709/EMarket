using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EMarket.Services.MailChimp
{
  
    class MailchimpRepository
    {
        private MailchimpOptions _mailChimpOption;
        private MailChimpManager _mailChimpManager;

        public MailchimpRepository(IOptions<MailchimpOptions> option)
        {
            _mailChimpOption = option.Value;
            _mailChimpManager = new MailChimpManager(_mailChimpOption.APIKey);
        }

        private Setting _campaignSettings = new Setting
        {
            ReplyTo = "damnhatphong671998@gmail.com",
            FromName = "Monkey Kong",
            Title = "Mailchimp API Test",
            SubjectLine = "nothing go here.",
        };

        // `html` contains the content of your email using html notation
        public void CreateAndSendCampaign(string html)
        {
            var campaign = _mailChimpManager.Campaigns.AddAsync(new Campaign
            {
                Settings = _campaignSettings,
                Recipients = new Recipient { ListId = _mailChimpOption.AudienceId },
                Type = CampaignType.Regular
            }).Result;
            var timeStr = DateTime.Now.ToString();
            var content = _mailChimpManager.Content.AddOrUpdateAsync(
             campaign.Id,
             new ContentRequest()
             {
                 Template = new ContentTemplate
                 {
                     Id = _mailChimpOption.TemplateId,
                     Sections = new Dictionary<string, object>()
                        {
                            { "body_content", html },
                            { "preheader_leftcol_content", $"<p>{timeStr}</p>" }
                        }
                 }
             }).Result;
            _mailChimpManager.Campaigns.SendAsync(campaign.Id).Wait();
        }
        public List<Template> GetAllTemplates()
          => _mailChimpManager.Templates.GetAllAsync().Result.ToList();
        public List<List> GetAllMailingLists()
          => _mailChimpManager.Lists.GetAllAsync().Result.ToList();
        public Content GetTemplateDefaultContent(string templateId)
          => (Content)_mailChimpManager.Templates.GetDefaultContentAsync(templateId).Result;
    }
}
