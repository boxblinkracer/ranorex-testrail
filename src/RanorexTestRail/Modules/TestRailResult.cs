using boxblinkracer.Ranorex.TestRail.Models;
using boxblinkracer.Ranorex.TestRail.Services;
using boxblinkracer.Ranorex.TestRail.Services.Client;
using boxblinkracer.Ranorex.TestRail.Services.Ranorex;
using boxblinkracer.Ranorex.TestRail.Services.Validator;
using Ranorex;
using Ranorex.Core.Reporting;
using Ranorex.Core.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxblinkracer.Ranorex.TestRail.Modules
{

    /// <summary>
    /// Description of RanorexIntegration.
    /// </summary>
    [TestModule("6CBDB9F7-5712-4AE7-9750-16269211CE71", ModuleType.UserCode, 1)]
    public class TestRailResult : ITestModule
    {

        /// <summary>
        /// 
        /// </summary>
        private ConfigurationValidator configValidator;

        /// <summary>
        /// 
        /// </summary>
        private int testRailTimeoutSeconds;

        /// <summary>
        /// Gets or sets the value of the test case in TestRail
        /// </summary>
        [TestVariable("2436c39d-0705-4ec3-85fb-5e9835a1ab19")]
        public string TestCaseID { get; set; }


        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public TestRailResult()
        {
            // Do not delete - a parameterless constructor is required!

            this.configValidator = new ConfigurationValidator();
            this.testRailTimeoutSeconds = 5;
        }


        /// <summary>
        /// 
        /// </summary>
        void ITestModule.Run()
        {
            var test = TestSuite.CurrentTestContainer;
            var parameters = TestSuite.Current.Parameters;
            var container = TestReport.CurrentTestContainerActivity;

            string testRailURL = parameters.Keys.Contains("TestRailURL") ? parameters["TestRailURL"] : "";
            string testRailUsername = parameters.Keys.Contains("TestRailUsername") ? parameters["TestRailUsername"] : "";
            string testRailPassword = parameters.Keys.Contains("TestRailPassword") ? parameters["TestRailPassword"] : "";
            string testRunID = parameters.Keys.Contains("TestRailTestRunID") ? parameters["TestRailTestRunID"] : "";

            if (!this.configValidator.IsValidCredentialData(testRailURL, testRailUsername, testRailPassword))
            {
                Report.Error("No TestRail API credentials provided. Please configure your credentials in the TestRailSetup action!");
                return;
            }

            if (string.IsNullOrEmpty(testRunID))
            {
                Report.Error("No TestRail Tesst Run ID provided for this Test!");
                return;
            }

            if (string.IsNullOrEmpty(this.TestCaseID))
            {
                Report.Error("No TestRail TestCaseID provided for this Test!");
                return;
            }


            // build our actual testrail api client
            // from the configured credentials
            var client = new TestRailAPIClient(testRailURL, testRailUsername, testRailPassword, this.testRailTimeoutSeconds);

            // create our reader to extract data like error messages
            // from our current test activity container
            var resultReader = new TestResultReader(container);


            string comment = "";

            Task<TestRailResponse> task = null;


            switch (test.Status)
            {
                case ActivityStatus.Success:
                    comment = "Automatic Ranorex Test Execution";
                    task = client.SendResult(testRunID, this.TestCaseID, (int)TestRailStatus.STATUS_PASSED, comment);
                    break;

                case ActivityStatus.Failed:
                    comment = resultReader.GetErrorMessage();
                    task = client.SendResult(testRunID, this.TestCaseID, (int)TestRailStatus.STATUS_FAILED, comment);
                    break;

                default:
                    comment = "Automatic Ranorex Test Execution";
                    task = client.SendResult(testRunID, this.TestCaseID, (int)TestRailStatus.STATUS_NOT_TESTED, comment);
                    break;
            }


            if (task == null)
            {
                Report.Error("Error when verifying Test Result. No Result sent to TestRail!");
                return;
            }

            // run our testrail api client and
            // wait for the result before continuing
            task.Wait();


            TestRailResponse response = task.Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // build our testrail deep link to our provided test run
                string testRunLink = this.BuildTestRunDeepLink(testRailURL, testRunID);

                Report.Success("Successfully sent Test Result for Test " + this.TestCaseID + " to TestRail!");
                Report.Link("Open Test Run", testRunLink);
            }
            else
            {
                Report.Warn("TestRail API Response: " + response.Content);
                Report.Error("Error when sending Result for Test " + this.TestCaseID + " to TestRail!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testRailURL"></param>
        /// <param name="testRunID"></param>
        /// <returns></returns>
        private string BuildTestRunDeepLink(string testRailURL, string testRunID)
        {
            return testRailURL + "/index.php?/runs/view/" + testRunID.ToUpper().Replace("R", "");
        }

    }
}
