using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCheckRobot.Core
{
    public class SiteHealthItem
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public int ResponseTimeMs { get; set; }
        public int HttpStatusCode { get; set; }

        public string State => HttpStatusCode >= 200 && HttpStatusCode < 400 ? "good" : "bad";
    }

}
