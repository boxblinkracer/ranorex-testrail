using Ranorex;
using Ranorex.Core.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxblinkracer.Ranorex.TestRail.Services.Ranorex
{

    /// <summary>
    /// 
    /// </summary>
    public class TestResultReader
    {

        /// <summary>
        /// 
        /// </summary>
        private ITestContainerActivity rootContainer;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootContainer"></param>
        public TestResultReader(ITestContainerActivity rootContainer)
        {
            this.rootContainer = rootContainer;
        }


        /// <summary>
        /// Extracts the first relevant error message from the
        /// test activity container of Ranorex
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return this.RecSearchContainer(this.rootContainer);
        }


        /// <summary>
        /// Searches recursively in the test containers until a
        /// real ModuleActivity is found that includes error messages.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private string RecSearchContainer(ITestContainerActivity container)
        {
            string error = "";

            foreach (var containerChild in container.Children)
            {
                var subContainer = containerChild as ITestContainerActivity;
                if (subContainer != null)
                {
                    error = this.RecSearchContainer(subContainer);
                }

                var module = containerChild as ITestModuleActivity;
                if (module != null)
                {
                    error = this.GetFirstModuleError(module.Children);
                }

                // if we have found a result, then return it
                if (!string.IsNullOrEmpty(error))
                {
                    break;
                }
            }

            return error;
        }

        /// <summary>
        /// Searches the first error message in all executed modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        private string GetFirstModuleError(IList<IReportItem> modules)
        {
            foreach (var module in modules)
            {
                var reportItem = module as ReportItem;

                if (reportItem != null && (reportItem.Level == ReportLevel.Error || reportItem.Level == ReportLevel.Failure))
                {
                    return reportItem.Message;
                }
            }

            return "";
        }

    }

}
