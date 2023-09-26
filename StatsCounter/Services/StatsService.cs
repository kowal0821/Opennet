using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatsCounter.Models;

namespace StatsCounter.Services
{
    public interface IStatsService
    {
        Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner);
    }
    
    public class StatsService : IStatsService
    {
        private readonly IGitHubService _gitHubService;

        public StatsService(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        public async Task<RepositoryStats> GetRepositoryStatsByOwnerAsync(string owner)
        {
            var repos = await _gitHubService.GetRepositoryInfosByOwnerAsync(owner);

            if (repos == null)
            {
                return null;
            }


            char[] alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            var repoStats = new RepositoryStats
            {
                AvgForks = repos.Sum(x => x.ForksCount),
                AvgSize = repos.Sum(x => x.Size),
                AvgStargazers = repos.Sum(x => x.StargazersCount),
                AvgWatchers = repos.Sum(x => x.WatchersCount),
                Owner = owner,
                Letters = alphabet.Select(x => new Dictionary<char, int>
                {
                    [x] = repos.Sum(s => s.Name.Count(c => c == x))
                }).FirstOrDefault()
            };

            return repoStats;
        }
    }
}