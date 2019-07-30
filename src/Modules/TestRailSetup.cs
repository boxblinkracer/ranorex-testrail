using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ranorex.Core.Testing;

namespace boxblinkracer.Ranorex.TestRail.Modules
{

    /// <summary>
    /// Description of TestRailSetup.
    /// </summary>
    [TestModule("5C063D78-5E51-4C56-B8E6-2FA412E85055", ModuleType.UserCode, 1)]
    public class TestRailSetup : ITestModule
    {

        /// <summary>
        /// The endpoint URL to your TestRail instance
        /// </summary>
        [TestVariable("2436c39d-0705-4ec3-85fb-5e9835a1ab29")]
        public string TestRailURL { get; set; }

        /// <summary>
        /// The TestRail username
        /// </summary>
        [TestVariable("2436c39d-0705-4ec3-85fb-5e9865a1ab19")]
        public string TestRailUsername { get; set; }

        /// <summary>
        /// The TestRail password
        /// </summary>
        [TestVariable("2436c39d-0705-2e33-85fb-5e9865a1ab19")]
        public string TestRailPassword { get; set; }

        /// <summary>
        /// The TestRun ID that should be used for
        /// reporting all test results.
        /// </summary>
        [TestVariable("2321c39d-0705-4ec3-85fb-5e9835a1ab29")]
        public string TestRailTestRunID { get; set; }


        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public TestRailSetup()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// 
        /// </summary>
        void ITestModule.Run()
        {
        }
    }

}
