namespace StatsCounter.Models
{
    public class HttpClientOptions
    {
        public static string Section = "GitHubSettings";

        public string BaseApiUrl { get; set; }

        public string AccessToken { get; set; }

        public string User { get; set; }
    }
}
