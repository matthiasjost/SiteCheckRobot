using System.Diagnostics;

namespace SiteCheckRobot.Core
{
    public class SiteCheck
    {
        public int ResponseTimeMs { get; set; }
        public int HttpStatusCode { get; set; }


        public SiteCheck()
        {
            
        }
        public async Task LoadSite(string url)
        {

            var watch = new Stopwatch();
            HttpClient client = new HttpClient();
            watch.Start();
            HttpResponseMessage response = await client.GetAsync(url);
            HttpStatusCode = (int)response.StatusCode;
            watch.Stop();
            ResponseTimeMs = (int)watch.ElapsedMilliseconds;
        }
    }
}