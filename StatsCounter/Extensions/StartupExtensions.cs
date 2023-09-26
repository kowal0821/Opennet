using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StatsCounter.Models;
using StatsCounter.Services;

namespace StatsCounter.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddGitHubService(
            this IServiceCollection services,
            Uri baseApiUrl,
            IConfiguration configuration)
        {
            

            services.AddTransient<IGitHubService, GitHubService>();
            AddHttpClient(services, configuration, baseApiUrl);

            return services; // TODO: add your code here
        }

        private static IServiceCollection AddHttpClient(IServiceCollection services, IConfiguration configuration, Uri baseUrl)
        {
            services.Configure<HttpClientOptions>(options => configuration.GetSection(HttpClientOptions.Section));

            services.AddHttpClient<IGitHubService, GitHubService>((servieProvider, httpClient) =>
            {
                httpClient.BaseAddress = baseUrl;
                httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic", $"{configuration["GitHubSettings:AccessToken"]}");
            });

            return services;
        }
    }
}