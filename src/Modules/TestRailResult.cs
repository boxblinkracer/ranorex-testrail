using boxblinkracer.Ranorex.TestRail.Models;
using boxblinkracer.Ranorex.TestRail.Services;
using boxblinkracer.Ranorex.TestRail.Services.Client;
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
        }


        /// <summary>
        /// 
        /// </summary>
        void ITestModule.Run()
        {
            var test = TestSuite.CurrentTestContainer;
            var parameters = TestSuite.Current.Parameters;

            string testRailURL = parameters.Keys.Contains("TestRailURL") ? parameters["TestRailURL"] : "";
            string testRailUsername = parameters.Keys.Contains("TestRailUsername") ? parameters["TestRailUsername"] : "";
            string testRailPassword = parameters.Keys.Contains("TestRailPassword") ? parameters["TestRailPassword"] : "";
            string testRunID = parameters.Keys.Contains("TestRailTestRunID") ? parameters["TestRailTestRunID"] : "";

            if (!this.isValidCredentialData(testRailURL, testRailUsername, testRailPassword))
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

         


            int timeoutSeconds = 5;

            var client = new TestRailAPIClient(testRailURL, testRailUsername, testRailPassword, timeoutSeconds);


            string comment = "Automatic Ranorex Test Execution";

            Task<TestRailResponse> task = null;

            switch (test.Status)
            {
                case ActivityStatus.Success:
                    task = client.SendResult(testRunID, this.TestCaseID, (int)TestRailStatus.STATUS_PASSED, comment);
                    break;

                case ActivityStatus.Failed:
                    task = client.SendResult(testRunID, this.TestCaseID, (int)TestRailStatus.STATUS_FAILED, comment);
                    break;

                default:
                    task = client.SendResult(testRunID, this.TestCaseID, (int)TestRailStatus.STATUS_NOT_TESTED, comment);
                    break;
            }


            if (task == null)
            {
                Report.Error("Error when verifying Test Result. No Result sent to TestRail!");
                return;
            }


            task.Wait();

            TestRailResponse response = task.Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string testRunLink = testRailURL + "/index.php?/runs/view/" + testRunID.ToUpper().Replace("R", "");

                Report.Success("Successfully sent Test Result for Test " + this.TestCaseID + " to TestRail!");
                Report.Link(testRunLink);
            }
            else
            {
                Report.Info("TestRail API Response: " + response.Content);
                Report.Error("Error when sending Result for Test " + this.TestCaseID + " to TestRail!");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool isValidCredentialData(string url, string username, string password)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            return true;
        }

    }
}
