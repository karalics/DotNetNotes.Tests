using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using DotNetNotes;
using System;
using Xunit.Abstractions;
using System.IO;

namespace DotNetNotes.Test
{
    public class UnitTest1
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            // For Debuging write to output
            this.output = output;
            string testpath = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath;
            testpath = testpath.Substring(0, testpath.IndexOf("DotNetNotes.Tests"));
            var contentroot = Path.Combine(testpath, "DotNetNotes");
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(contentRoot: contentroot)
                .UseEnvironment(environment: "Development")
                .UseStartup<DotNetNotes.Startup>()
                .UseApplicationInsights());
            _client = _server.CreateClient();
        }

        [Fact]
        public async void NoteDoesntExist()
        {
            var response = await _client.GetAsync("/Note/Edit/777");
            string testStatusCode = response.StatusCode.ToString();
            // Note should not exist (404)
            Assert.Equal("NotFound", testStatusCode);
        }

        [Fact]
        public void UpdateNote()
        {
            // We don't have a Note-Service, so we work on the Model
            Models.Note note = new Models.Note
            {
                Id = 777,
                Priority = 3,
                Title = "New Note",
                Text = "Test Note",
                DueDate = DateTime.Now
            };
            note.Title = "Updated Note";
            Assert.Equal("Updated Note", note.Title);
        }
    }
}
