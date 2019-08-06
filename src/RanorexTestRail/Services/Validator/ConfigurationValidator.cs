using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxblinkracer.Ranorex.TestRail.Services.Validator
{

    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsValidCredentialData(string url, string username, string password)
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
