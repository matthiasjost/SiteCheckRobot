using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCheckRobot.Core
{
    // make it a cosmos db sitehealth item with id
    public class SiteHealthItem
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public int ResponseTimeMs { get; set; }
        public int HttpStatusCode { get; set; }
    }
}
