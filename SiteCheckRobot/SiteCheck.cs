using System.Diagnostics;

namespace SiteCheckRobot;

public class SiteCheck
{
    public int ResponseTimeMs { get; set; }
    public int HttpStatusCode { get; set; }

    public async Task LoadSite(string url)
    {
        var watch = new Stopwatch();
        var client = new HttpClient();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        watch.Start();

        try
        {
            HttpResponseMessage response = await client.GetAsync(url, cts.Token);

            if (response.IsSuccessStatusCode)
            {
                HttpStatusCode = (int)response.StatusCode;
            }
            else
            {
                HttpStatusCode = (int)response.StatusCode;
                // Handle non-200 status codes here
                // For example, you can throw an exception or set a flag to indicate an error
                // throw new Exception($"HTTP request failed with status code {response.StatusCode}");
                // or
                // HasError = true;
            }
        }
        catch (HttpRequestException)
        {
            // Handle general HTTP errors
            HttpStatusCode = -2; // or any other error code you want to use
        }
        catch (TaskCanceledException)
        {
            // Handle timeouts
            HttpStatusCode = -3; // or any other error code you want to use
        }
        finally
        {
            watch.Stop();
            ResponseTimeMs = (int)watch.ElapsedMilliseconds;
        }
    }

}