using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using DotNetNotes;

namespace DotNetNotes.Test
{
    public class UnitTest1
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public UnitTest1()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(contentRoot: "/home/slavi/dotnet/DotNetNotes")
                .UseEnvironment(environment: "Development")
                .UseStartup<DotNetNotes.Startup>()
                .UseApplicationInsights());
            _client = _server.CreateClient();
        }

        [Fact]
        public async void Test1()
        {
            var response = await _client.GetAsync("/");
            //response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(responseString.Contains("Note"));
        }
    }
}
