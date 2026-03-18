using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurement.Repository.Util
{
    public class AppConfig
    {
        private static AppConfig? _instance;
        private static readonly object _lock = new();

        private readonly IConfiguration _config;

        private AppConfig()
        {
            string basePath = FindAppSettingsDirectory()
                ?? throw new FileNotFoundException(
                    "appsettings.json was not found. " +
                    "Make sure QuantityMeasurement.ConsoleApp/appsettings.json has " +
                    "<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory> in its .csproj.");

            _config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        public static AppConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new AppConfig();
                    return _instance;
                }
            }
        }

        public string ConnectionString =>
            _config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in appsettings.json");

        // "cache" or "database" – controls which repo gets created in Menu.cs
        public string RepositoryType =>
            _config["App:RepositoryType"] ?? "cache";

        public int MaxPoolSize =>
            int.TryParse(_config["App:ConnectionPool:MaxSize"], out int v) ? v : 10;

        public int MinPoolSize =>
            int.TryParse(_config["App:ConnectionPool:MinSize"], out int v) ? v : 2;

        public int ConnectionTimeoutSeconds =>
            int.TryParse(_config["App:ConnectionPool:TimeoutSeconds"], out int v) ? v : 30;

        private static string? FindAppSettingsDirectory()
        {
            // first check next to the running exe 
            string? assemblyDir = Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (assemblyDir != null && File.Exists(Path.Combine(assemblyDir, "appsettings.json")))
                return assemblyDir;

            // fallback: walk up from the working directory 
            string? dir = Directory.GetCurrentDirectory();
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir, "appsettings.json")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }

            return null;
        }
    }
}
