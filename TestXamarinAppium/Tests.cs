// dependency required for URi and Exception objects
using System;
// dependency required for Eyes objec
using Applitools.Selenium;
// dependency required for FixedCutProvider object
using Applitools.Cropping;
// dependency required for NUnit
using NUnit.Framework;
// dependency required for By object
using OpenQA.Selenium;
// dependency requierd for RemoteWebDriver object
using OpenQA.Selenium.Remote;
// dependency required for BatchInfo object
using Applitools;

namespace TestXamarinAppium
{
    [TestFixture]
    public class Tests
    {
        //Declaring the Eyes object
        Eyes eyes;
        //Declaring the RemoteWebDriver object
        RemoteWebDriver driver;
        //Creating a BatchInfo object to group all the tests together ender the same batch
        BatchInfo mybatch = new BatchInfo("Xamarin");
        //Using the same test name to create a bug in the TestWithDiff method
        string TestName = "Test_Appium";
        //Declaring the DesiredCapabilities object
        DesiredCapabilities dc;

        [TestFixtureSetUp]
        public void BeforeAllTests()
        {

            // Initialize the eyes SDK and set your private API key.
            eyes = new Eyes();
            //eyes.ApiKey = "ioRRQOF5YBYU6NIwbe3tuDFCX8H109mmenarZo8arSlbA110";
            eyes.ApiKey = Environment.GetEnvironmentVariable("APPLITOOLS_API_KEY");

            //Hides the scroll bar
            eyes.HideScrollbars = true;
            //Take fullpage screenshot
            eyes.ForceFullPageScreenshot = true;
            //when taking a full page screenshot use css stitching to handle fixed positoin elements like headers/floating bars
            eyes.StitchMode = StitchModes.CSS;

            //Creat a batch to group all the tests together
            eyes.Batch = mybatch;

            // We remove the header automatically for some devices, sometimes there are a few extra pixels that are needed to be removed
            //FixedCutProvider provider = new FixedCutProvider(7, 0, 0, 0);
            //eyes.CutProvider = provider;

            //using OpenQA.Selenium.Remote
            //Setup appium - Ensure the capabilities meets your environment.
            dc = new DesiredCapabilities();
            dc.SetCapability("platformName", "iOS");
            dc.SetCapability("platformVersion", "11.2");
            dc.SetCapability("browserName", "safari");
            dc.SetCapability("deviceName", "iPhone 7");

        }

        [SetUp]
        public void BeforeEachTest()
        {
            try
            {
                //creating the driver with the appium server url
                driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4723/wd/hub"), dc);

                // Start the test
                eyes.Open(driver, "Xamarin", TestName);

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        [TearDown]
        public void Cleanup()
        {
            // End the test.
            driver.Quit();
            eyes.Close();
           
        }

      
        [Test]
        public void RunTest()
        {

            try
            {

                // Navigate the browser to the "hello world!" web-site.
                driver.Url = "https://applitools.com/helloworld";

                // Visual checkpoint #1
                eyes.Check("Hello!", Applitools.Selenium.Target.Window());

                // Click the "Click me!" button.
                driver.FindElement(By.TagName("button")).Click();

                // Visual checkpoint #2
                eyes.Check("Click!", Applitools.Selenium.Target.Window());
            
                // End the test.
                eyes.Close();
            }
            finally
            {
                // If the test was aborted before eyes.close was called, ends the test as aborted.
                eyes.AbortIfNotClosed();
                // Close the browser
                driver.Quit();
            }
        }

        [Test]
        public void TestWithDiff()
        {

            // Navigate the browser to the "hello world!" web-site.
            driver.Url = "https://applitools.com/helloworld";

            //Generate diffs
            driver.FindElement(By.CssSelector("body > div > div:nth-child(2) > p:nth-child(3) > a")).Click();

            // Visual checkpoint #1
            eyes.Check("Hello!", Applitools.Selenium.Target.Window());

            // Click the "Click me!" button.
            driver.FindElement(By.TagName("button")).Click();

            // Visual checkpoint #2
            eyes.Check("Click!", Applitools.Selenium.Target.Window());

        }

    }
}
