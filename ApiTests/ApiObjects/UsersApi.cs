using ApiTests.Models;
using ApiTests.Services;
using NUnit.Framework;
using ApiTests.Clients;


namespace ApiTests.ApiObjects
{
    public class UsersApi
    {
        private readonly UsersApiClient _client;
        public ApiResponse<UsersResponse>? LastResponse { get; private set; }

        public UsersApi(UsersApiClient client)
        {
            _client = client;
        }

        public async Task GetUsersPage(int page)
        {
            LastResponse = await _client.GetUsersPageAsync(page);
            Log(LastResponse, $"GET /users?page={page}");
        }

        public void ValidateSuccess()
        {
            Assert.That(LastResponse, Is.Not.Null, "No response");
            Assert.That(LastResponse!.IsSuccess, Is.True, $"Request failed: {LastResponse.ErrorMessage}");
            Assert.That(LastResponse.StatusCode, Is.InRange(200, 299), $"Unexpected status code: {LastResponse.StatusCode}");
        }

        public void ValidateStatusCode(int expected)
        {
            Assert.That(LastResponse, Is.Not.Null, "No response");
            Assert.That(LastResponse!.StatusCode, Is.EqualTo(expected), $"Expected {expected} but got {LastResponse.StatusCode}");
        }

        public void ValidateContains(User expected)
        {
            Assert.Multiple(() =>
            {
                Assert.That(LastResponse?.Data?.Data, Is.Not.Null.Or.Empty, "list is empty");
                var match = LastResponse!.Data!.Data
                    .FirstOrDefault(u => u.FirstName == expected.FirstName &&
                                         u.LastName  == expected.LastName  &&
                                         u.Email      == expected.Email      &&
                                         u.Avatar     == expected.Avatar);
                Assert.That(match, Is.Not.Null, $"User {expected.FirstName} {expected.LastName} not found");
            });
        }

        private static void Log<T>(ApiResponse<T> resp, string op)
        {
            TestContext.WriteLine($"[{op}] {resp.StatusCode} ({resp.ResponseTime.TotalMilliseconds} ms)");
            if (!resp.IsSuccess)
                TestContext.WriteLine($"[{op}] Error: {resp.ErrorMessage}");
        }
        
        public void ValidateUserCount(int expectedCount)
        {
            Assert.Multiple(() =>
            {
                Assert.That(LastResponse, Is.Not.Null, "No response");
                Assert.That(LastResponse!.Data, Is.Not.Null, "No data");
                Assert.That(LastResponse.Data!.Data, Is.Not.Null.Or.Empty, "list is empty");
                Assert.That(LastResponse.Data.Data.Count, Is.EqualTo(expectedCount),
                    $"Expected {expectedCount} users but found {LastResponse.Data.Data.Count}");
            });
        }

        public void ValidatePageNumber(int expectedPage)
        {
            Assert.Multiple(() =>
            {
                Assert.That(LastResponse, Is.Not.Null, "No response");
                Assert.That(LastResponse!.Data, Is.Not.Null, "No data in response");
                Assert.That(LastResponse.Data!.Page, Is.EqualTo(expectedPage),
                    $"Expected page {expectedPage} but got {LastResponse.Data.Page}");
            });
        }
    }
} 