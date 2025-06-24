using RestSharp;
using ApiTests.Services;
using ApiTests.Models;
using ApiTests.Config;

namespace ApiTests.Clients
{
    public class UsersApiClient : BaseApiClient<User>
    {
        public UsersApiClient(TestConfiguration config)
            : base(() => new RestClient(new RestClientOptions(config.GetBaseUrl())), 
                config.TestSettings.ApiKey,
                   UriConstants.Users, 
                   config.TestSettings.RetryAttempts, 
                   config.TestSettings.WaitBetweenRequests)
        {
        }

        public async Task<ApiResponse<UsersResponse>> GetUsersPageAsync(int page)
        {
            var request = new RestRequest($"{_endpoint}", Method.Get);
            request.AddQueryParameter("page", page);
            return await ExecuteWithRetryAsync<UsersResponse>(request);
        }
    }
} 