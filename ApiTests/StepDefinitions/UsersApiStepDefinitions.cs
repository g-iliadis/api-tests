using Reqnroll;
using ApiTests.Context;
using NUnit.Framework;
using Newtonsoft.Json;
using ApiTests.Models;

namespace ApiTests.StepDefinitions
{
    [Binding]
    [Parallelizable(ParallelScope.All)]
    public class UsersApiStepDefinitions(ApiTestContext context)
    {

        [Given("the Users API is available")]
        public async Task GivenTheUsersApiIsAvailable()
        {
            await context.Users.GetUsersPage(1);
            context.Users.ValidateSuccess();
        }

        [When("I request users for page (.*)")]
        public async Task WhenIRequestUsersForPage(int page)
            => await context.Users.GetUsersPage(page);

        [Then("the response should be successful")]
        public void ThenTheResponseShouldBeSuccessful()
            => context.Users.ValidateSuccess();

        [Then("the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int code)
            => context.Users.ValidateStatusCode(code);

        [Then("the response should contain the user from (.*)")]
        public void ThenTheResponseShouldContainUser(string fileName)
        {
            var full = Path.Combine(
                Directory.GetCurrentDirectory(), "Fixtures/User", fileName);
            var json = File.ReadAllText(full);
            var expected = JsonConvert.DeserializeObject<User>(json)!;
            context.Users.ValidateContains(expected);
        }

        [Then("the users list should contain (.*) users")]
        public void ThenTheUsersListShouldContainUsers(int expectedCount)
            => context.Users.ValidateUserCount(expectedCount);

        [Then("the page number should be (.*)")]
        public void ThenThePageNumberShouldBe(int expectedPage)
            => context.Users.ValidatePageNumber(expectedPage);

        [Then("the response should contain a user with:")]
        public void ThenTheResponseShouldContainAUserWith(Table table)
        {
            Assert.That(table.Rows, Is.Not.Empty, "No test data provided in table");

            var row = table.Rows[0];
            var expected = new User
            {
                FirstName = row["First_Name"],
                LastName  = row["Last_Name"],
                Email      = row["Email"],
                Avatar     = row["Avatar"]
            };

            context.Users.ValidateContains(expected);
        }
    }
}