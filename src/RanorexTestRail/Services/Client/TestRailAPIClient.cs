using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using System.Net.Http.Headers;

namespace boxblinkracer.Ranorex.TestRail.Services.Client
{

    /// <summary>
    /// 
    /// </summary>
    public class TestRailAPIClient
    {

        /// <summary>
        /// 
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// 
        /// </summary>
        private string baseURL = "";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="timeoutSeconds"></param>
        public TestRailAPIClient(string url, string username, string password, int timeoutSeconds)
        {
            // build the testrail url
            this.baseURL = url.Trim('/') + "/index.php?/api/v2";

            this.client = new HttpClient();

            // set our timeout
            this.client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            // add basic auth header
            this.client.DefaultRequestHeaders.Add("Authorization", "Basic " + this.Base64Encode(username + ":" + password));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="runID"></param>
        /// <param name="caseID"></param>
        /// <param name="statusID"></param>
        /// <param name="comment"></param>
        public async Task<TestRailResponse> SendResult(string runID, string caseID, int statusID, string comment)
        {
            // remove the leading R and C 
            // identifiers. we only need the digits!
            runID = runID.ToUpper().Replace("R", "");
            caseID = caseID.ToUpper().Replace("C", "");

            // build our json content
            var requestBody = new StringContent(this.BuildJSON(statusID, comment), Encoding.UTF8, "application/json");

            // send our result to testrail
            // and wait for the response
            var response = await this.client.PostAsync(this.baseURL + "/add_result_for_case/" + runID + "/" + caseID, requestBody);
            var responseBody = await response.Content.ReadAsStringAsync();

            // build our response object
            return new TestRailResponse(response.StatusCode, responseBody);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusID"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        private string BuildJSON(int statusID, string comment)
        {
            string json = "{ \"status_id\" : \"" + statusID + "\",  \"comment\" : \"" + comment + "\" }";

            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

}

