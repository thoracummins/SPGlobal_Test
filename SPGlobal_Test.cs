using System;
using System.Threading;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

public class SPGlobal_Test
{

    private string _url = "";
    private IWebDriver _driver = null;
        
    WeatherPage wp = null;

    public SPGlobal_Test()
    {
        _url = (@"http://weather.com/");
        TestStartup();
    }

    [Fact]
    [Trait("S&P Global", "Test")]
    public void SPGlobalCSharpSeleniumTest()
    {
        string searchText = "Raleigh, NC";

        wp = new WeatherPage(_driver);

        Thread.Sleep(3000);  // give page a chance to render

        try
        {
            // Test
            // Enter text in search field and click Enter
            wp.EnterTextInSearchField(_driver, searchText);
            

            // Assert
            // Verify top location name is the same as search text
            Assert.True(wp.VerifyLocationNAme(_driver, searchText), $"Expected Location Name was not {searchText}");
            Console.WriteLine($"Expected Location Name was {searchText}");

            // Verify Location Snapshot location is the same as search text
            Assert.True(wp.VerifyLocationAsOfSnapshot(_driver, searchText), $"Expected Location As Of was not {searchText}");
            Console.WriteLine($"Expected Location As Of was {searchText}");
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error Message: {e.Message}");
        }

        // Cleanup
        CloseBrowser();
    }

    // ---------------------------- private methods--------------------------

   /// <summary>
   /// Close browser and Quit the driver
   /// </summary>
    private void CloseBrowser()
    {
        _driver.Close();
        _driver.Quit();
    }

    /// <summary>
    /// Setup Chromedriver and open weather.com
    /// </summary>
    private void TestStartup()
    {
        _driver = new ChromeDriver();

        _driver.Navigate().GoToUrl(_url);
        _driver.Manage().Window.Maximize();
    }
}

// -------------------------------- WeatherPage Class ----------------------
public class WeatherPage
{
    /// <summary>
    /// WeatherPage defines web page elements and test functions
    /// </summary>
    /// <param name="driver"></param> 
    public WeatherPage(IWebDriver driver)
    {
    }

    private string locationName = "/html/body/div[1]/div[3]/div[2]/div/div/div/div[1]/div/div/div/a[2]/span[1]";
    private string locationNameAsOf = "/html/body/div[1]/main/div[2]/main/div[1]/div/section/div/div[1]/h1";
    private string searchTextBox = "LocationSearch_input"; // ID

    /// <summary>
    /// Enters text into Weather Page search field
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="text"></param>
    public void EnterTextInSearchField(IWebDriver driver, string text)
    {
        IWebElement ele = null;

        ele = driver.FindElement(By.Id(searchTextBox));
        ele.Clear();
        ele.SendKeys(text);
        Thread.Sleep(3000);
        ele.SendKeys(Keys.Enter);
    }

    /// <summary>
    /// Verifies current location name
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="text"></param>
    /// <returns>Boolean</returns>
    public bool VerifyLocationNAme(IWebDriver driver, string text)
    {
        IWebElement ele = null;
        string value = "";

        try
        {
            ele = driver.FindElement(By.XPath(locationName));
            value = ele.GetAttribute("innerText");

            if (text.ToLower() == value.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Verifies current value in search field
    /// </summary>
    /// <param name="driver"></param>
    /// <returns>Boolean</returns>
    public bool VerifyLocationAsOfSnapshot(IWebDriver driver, string text)
    {
        IWebElement ele = null;
        string value = "";

        try
        {
            ele = driver.FindElement(By.Id(locationNameAsOf));
            value = ele.GetAttribute("innerText");

            if (text.ToLower() == value.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error Message: {e.Message}");
            return false;
        }
    }
}

