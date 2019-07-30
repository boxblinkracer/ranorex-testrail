using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace boxblinkracer.Ranorex.TestRail.Services.Client
{

    /// <summary>
    /// 
    /// </summary>
    public class TestRailResponse
    {

        /// <summary>
        /// 
        /// </summary>
        private HttpStatusCode statusCode = 0;

        /// <summary>
        /// 
        /// </summary>
        private string content = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="content"></param>
        public TestRailResponse(HttpStatusCode statusCode, string content)
        {
            this.statusCode = statusCode;
            this.content = content;
        }

        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get
            {
                return this.content;
            }
        }

    }
}
