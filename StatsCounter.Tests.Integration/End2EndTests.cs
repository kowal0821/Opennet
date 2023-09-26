using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using StatsCounter.Models;
using StatsCounter.Services;
using Xunit;

namespace StatsCounter.Tests.Integration
{
    public class End2EndTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private const string Owner = "octocat";

        public End2EndTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CanReachGitHub()
        {
            // Arrange
            var client = _factory.CreateClient();
            var mockStatsService = new Mock<IStatsService>();
            var mockGitHubService = new Mock<IGitHubService>();

            mockStatsService.Setup(c => c.GetRepositoryStatsByOwnerAsync(Owner)).ReturnsAsync(new RepositoryStats
            {
                AvgForks = 1,
                Owner = Owner,
                AvgSize = 1,
                AvgStargazers = 1,
                AvgWatchers = 1,
                Letters = null
            });

            // Act
            var response = await client.GetAsync($"repositories/{Owner}");

            // Assert
            var responseJson = await response.Content.ReadAsStringAsync();
            var states = JsonConvert.DeserializeObject<RepositoryStats>(responseJson);

            response.IsSuccessStatusCode.Should().BeTrue();
            states.Owner.Should().Be(Owner);
            states.AvgForks.Should().Be(1);
        }
    }
}