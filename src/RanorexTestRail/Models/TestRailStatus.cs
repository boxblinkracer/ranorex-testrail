using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxblinkracer.Ranorex.TestRail.Models
{

    /// <summary>
    /// 
    /// </summary>
    public enum TestRailStatus
    {
        STATUS_PASSED = 1,
        STATUS_BLOCKED = 2,
        STATUS_NOT_TESTED = 3,
        STATUS_RETEST = 4,
        STATUS_FAILED = 5

    }
}
