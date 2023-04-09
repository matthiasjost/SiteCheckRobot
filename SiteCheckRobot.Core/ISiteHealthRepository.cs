using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCheckRobot.Core
{
    public interface ISiteHealthRepository
    {
        public Task Connect();
    }
}