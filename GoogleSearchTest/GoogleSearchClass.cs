using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GoogleSearchTest
{
    public class GoogleSearchClass
    {
        [Test]
        public void GoogleSearch()
        {
            ChromeOptions options = setOptions();
            IWebDriver driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl("https://google.com");
                driver.Manage().Window.Maximize();

                IWebElement txtSearch = driver.FindElement(By.Name("q"));
                txtSearch.SendKeys("youtube");
                txtSearch.SendKeys(Keys.Enter);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement buttonFirstLink = wait.Until(d => d.FindElement(By.XPath("(//a/h3)[1]")));
                buttonFirstLink.Click();

                IWebElement txtYoutubeSearch = wait.Until(d => d.FindElement(By.Name("search_query")));
                txtYoutubeSearch.SendKeys("Rihanna");
                txtYoutubeSearch.SendKeys(Keys.Enter);

                IWebElement buttonFirstVideo = wait.Until(d => d.FindElement(By.XPath("(//*[@id='video-title'])[1]")));
                buttonFirstVideo.Click();

                wait.Until(d => d.FindElement(By.Id("movie_player")));

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver; 

                while (true)
                {
                    try
                    {
                        var currentTimeObj = js.ExecuteScript("return document.querySelector('video').currentTime");
                        double currentTime = Convert.ToDouble(currentTimeObj);

                        System.Diagnostics.Debug.WriteLine($"Seconds: {currentTime}");

                        if (currentTime >= 23)
                        {
                            driver.FindElement(By.Id("movie_player")).Click();
                            break;
                        }
                    }
                    catch
                    {
                    }

                    Thread.Sleep(100);
                }

                Thread.Sleep(3000);
            }
            finally
            {
                driver.Quit();
            }
        }

        public static ChromeOptions setOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");

            return options;
        }
    }
}