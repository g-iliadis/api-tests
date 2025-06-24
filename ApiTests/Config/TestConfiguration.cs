using Microsoft.Extensions.Configuration;

namespace ApiTests.Config
{
    public class TestConfiguration
    {
        public TestSettings TestSettings { get; } = new();

        public static TestConfiguration Load()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var testConfig = new TestConfiguration();
            configuration.Bind(testConfig);
            return testConfig;
        }

        public string GetBaseUrl() => TestSettings.BaseUrl;
    }

    public class TestSettings
    {
        public string BaseUrl { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
        
        public int DefaultTimeout { get; set; } = 30;
        public int RetryAttempts { get; set; } = 3;
        public int WaitBetweenRequests { get; set; } = 1000;
    }
}