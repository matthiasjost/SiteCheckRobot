namespace SiteCheckRobot.Core.Tests
{
    public class SiteCheckTests
    {
        [Fact]
        public async Task SiteResponseTimeTest()
        {
            // Arrange
            SiteCheck siteCheck = new SiteCheck();
            string url = "https://www.google.com";
            int expectedResponseTimeMs = 1000;
            // Act
            await siteCheck.LoadSite(url);
            // Assert
            Assert.True(siteCheck.ResponseTimeMs < expectedResponseTimeMs);
        }
        [Fact]
        public async Task SiteResponsCodeTest()
        {
            // Arrange
            SiteCheck siteCheck = new SiteCheck();
            string url = "https://www.google.com";
            int expectedResponseCode = 200;
            // Act
            await siteCheck.LoadSite(url);
            // Assert
            Assert.Equal(expectedResponseCode, siteCheck.HttpStatusCode);
 
        }
    }
}