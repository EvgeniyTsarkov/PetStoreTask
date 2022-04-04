using NUnit.Framework.Internal;
using Serilog;
using TechTalk.SpecFlow;

namespace PetStoreTask.Tests.Steps
{
    [Binding]
    class BaseStepsDefinitions
    {
        private const string LogFileNamePattern = "F:\\log-{0}.txt";

        protected readonly ScenarioContext _scenarioContext;

        public BaseStepsDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        protected void Setup() => InitializeLogger();

        protected void InitializeLogger()
        {
            var testName = TestExecutionContext.CurrentContext.CurrentTest.ClassName;

            _scenarioContext["logger"] = new LoggerConfiguration()
                .WriteTo.File(string.Format(LogFileNamePattern, testName),
                rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
