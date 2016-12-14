using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailAddressAnalyzer
{
    public enum FailReason
    {
        NoMailServerExists,
        InvalidEmail,
        NonExistentDomain
    }
    public enum WellKnownMailServer
    {
        Unknown,
        Outlook,
        Gmail,
        Office365
    }
    public class EmailAddressAnalysis
    {
        public bool LooksGood;
        public WellKnownMailServer WellKnownMailServer;
        public FailReason Reason;
    }
    public static class Analyzer
    {
        const string emailAddressRegex = "(.+?)(@)(.+)";

        private static LookupClient lookupClient = new LookupClient();

        public async static Task<EmailAddressAnalysis> Analyze(string emailAddress)
        {
            var regexMatch = Regex.Match(emailAddress, emailAddressRegex);

            if (regexMatch.Groups.Count == 4)
            {
                var domain = regexMatch.Groups[3].Value;

                var dnsResponse = await lookupClient.QueryAsync(domain, QueryType.MX);

                if (dnsResponse.Header.ResponseCode == DnsResponseCode.NotExistentDomain)
                {
                    return new EmailAddressAnalysis()
                    {
                        LooksGood = false,
                        WellKnownMailServer = WellKnownMailServer.Unknown,
                        Reason = FailReason.NonExistentDomain
                    };
                }

                var mailHosts = dnsResponse.Answers.Select(a => a.RecordToString());

                if (mailHosts.Count() > 0)
                {
                    var wellKnownMailServer = deduceMailServer(mailHosts);

                    return new EmailAddressAnalysis()
                    {
                        LooksGood = true,
                        WellKnownMailServer = wellKnownMailServer
                    };
                }
                else
                {
                    return new EmailAddressAnalysis()
                    {
                        LooksGood = false,
                        WellKnownMailServer = WellKnownMailServer.Unknown,
                        Reason = FailReason.NoMailServerExists
                    };
                }
            }
            else
            {
                return new EmailAddressAnalysis()
                {
                    LooksGood = false,
                    WellKnownMailServer = WellKnownMailServer.Unknown,
                    Reason = FailReason.InvalidEmail
                };
            }
        }

        private static WellKnownMailServer deduceMailServer(IEnumerable<string> mailHosts)
        {

            if (mailHosts.Where(mh => mh.Contains("hotmail.com")).Any())
                return WellKnownMailServer.Outlook;
            else if (mailHosts.Where(mh => mh.Contains("outlook.com")).Any())
                return WellKnownMailServer.Office365;
            else if (mailHosts.Where(mh => mh.Contains("google.com")).Any())
                return WellKnownMailServer.Gmail;
            else
                return WellKnownMailServer.Unknown;
        }
    }
}
