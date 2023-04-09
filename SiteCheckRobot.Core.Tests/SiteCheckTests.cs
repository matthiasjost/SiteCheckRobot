namespace SiteCheckRobot.Core.Tests
{
    public class SiteCheckTests
    {
        [Fact]
        public async Task SiteResponseTimeTest()
        {
            // Arrange
            var siteCheck = new SiteCheck();
            var url = "https://www.google.com";
            var expectedResponseTimeMs = 1000;
            // Act
            siteCheck.LoadSite(url);
            // Assert
            Assert.True(siteCheck.ResponseTimeMs < expectedResponseTimeMs);
        }
        [Fact]
        public async Task SiteResponsCodeTest()
        {
            // Arrange
            var siteCheck = new SiteCheck();
            var url = "https://www.google.com";
            var expectedResponseCode = 200;
            // Act
            await siteCheck.LoadSite(url);
            // Assert
            Assert.Equal(expectedResponseCode, siteCheck.HttpStatusCode);
 
        }
    }
}