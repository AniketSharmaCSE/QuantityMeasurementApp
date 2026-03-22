using Microsoft.Extensions.Configuration;

namespace QuantityMeasurement.Repository.Util
{
    // singleton that reads appsettings.json once and exposes typed properties
    public class AppConfig
    {
        private static AppConfig? _instance;
        private readonly IConfiguration _config;

        private AppConfig()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }

        public static AppConfig Instance => _instance ??= new AppConfig();

        public string RepositoryType =>
            _config["App:RepositoryType"] ?? "cache";

        public string ConnectionString =>
            _config.GetConnectionString("DefaultConnection") ?? string.Empty;

        public int PoolMaxSize =>
            int.Parse(_config["App:ConnectionPool:MaxSize"] ?? "10");

        public int PoolMinSize =>
            int.Parse(_config["App:ConnectionPool:MinSize"] ?? "2");

        public int PoolTimeoutSeconds =>
            int.Parse(_config["App:ConnectionPool:TimeoutSeconds"] ?? "30");
    }
}
