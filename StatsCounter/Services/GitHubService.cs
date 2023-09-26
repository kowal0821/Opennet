using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StatsCounter.Models;

namespace StatsCounter.Services
{
    public interface IGitHubService
    {
        Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner);
    }
    
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GitHubService> _logger;

        public GitHubService(HttpClient httpClient, ILogger<GitHubService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/users/{owner}/repos");

            if (!response.IsSuccessStatusCode) 
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogError(responseBody);
                throw new HttpRequestException(responseBody, null, response.StatusCode);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<RepositoryInfo>>(responseContent) ?? throw new InvalidOperationException("Could't deserialize object");
        }
    }
}
