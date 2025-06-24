using Reqnroll;
using ApiTests.Config;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace ApiTests.Hooks;

[Binding]
public sealed class TestHooks
{
    private readonly TestConfiguration _config;

    public TestHooks(TestConfiguration config) => _config = config;

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        var cfg = TestConfiguration.Load();
        TestContext.WriteLine($"[{DateTime.UtcNow:O}] Test run started. Base URL: {cfg.GetBaseUrl()}");
    }

    [BeforeScenario]
    public void BeforeScenario(ScenarioContext scenarioContext)
    {
        TestContext.WriteLine(
            $"[{DateTime.UtcNow:O}] Starting scenario: «{scenarioContext.ScenarioInfo.Title}» " +
            $"| Base URL: {_config.GetBaseUrl()}");
    }

    [AfterScenario]
    public void AfterScenario(ScenarioContext scenarioContext)
    {
        var outcome = scenarioContext.ScenarioExecutionStatus;
        if (scenarioContext.TestError is not null)
        {
            TestContext.WriteLine($"Scenario failed: {scenarioContext.TestError.Message}");
        }

        TestContext.WriteLine(
            $"[{DateTime.UtcNow:O}] Completed scenario: «{scenarioContext.ScenarioInfo.Title}» → {outcome}");
    }
}