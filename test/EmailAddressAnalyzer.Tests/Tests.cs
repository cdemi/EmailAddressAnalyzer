using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public async Task TestOutlook()
        {
            string emailAddress = "christopher.demicoli@outlook.com";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Outlook);
        }

        [Fact]
        public async Task TestGmail()
        {
            string emailAddress = "christopher.demicoli@gmail.com";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Gmail);
        }

        [Fact]
        public async Task TestGoogleApps()
        {
            string emailAddress = "test@google.com";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Gmail);
        }

        [Fact]
        public async Task TestOffice365()
        {
            string emailAddress = "test@microsoft.com";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Office365);
        }

        [Fact]
        public async Task TestTestDomain()
        {
            string emailAddress = "test@test.com";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Unknown);
        }

        [Fact]
        public async Task TestDomainWithoutMailServer()
        {
            string emailAddress = "test@99bits.net";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(!analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Unknown && analysis.Reason == EmailAddressAnalyzer.FailReason.NoMailServerExists);
        }

        [Fact]
        public async Task TestInexistentDomain()
        {
            string emailAddress = "test@sdfgsdfgdf3355.net";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(!analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Unknown && analysis.Reason == EmailAddressAnalyzer.FailReason.NonExistentDomain);
        }

        [Fact]
        public async Task TestNoDomain()
        {
            string emailAddress = "christopher.demicoli";
            var analysis = await EmailAddressAnalyzer.Analyzer.Analyze(emailAddress);
            Assert.True(!analysis.LooksGood && analysis.WellKnownMailServer == EmailAddressAnalyzer.WellKnownMailServer.Unknown && analysis.Reason == EmailAddressAnalyzer.FailReason.InvalidEmail);
        }
    }
}
