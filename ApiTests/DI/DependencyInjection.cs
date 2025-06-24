using Microsoft.Extensions.DependencyInjection;
using ApiTests.ApiObjects;
using ApiTests.Clients;
using ApiTests.Config;
using ApiTests.Context;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace ApiTests.DI;

public static class DependencyInjection
{
    [ScenarioDependencies]
    public static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        var config = TestConfiguration.Load();
        services.AddSingleton(config);
        services.AddScoped<UsersApiClient>();
        services.AddScoped<UsersApi>();
        services.AddScoped<ApiTestContext>();

        return services;
    }
}