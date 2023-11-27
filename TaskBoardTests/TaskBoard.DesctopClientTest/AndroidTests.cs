using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace TaskBoard.DesctopClientTest
{
    public class DesktopTest
    {
        private const string AppiumUrl = "http://localhost:4723/wd/hub";
        private const string TasksBoardUrl = "https://taskboard.nakov.repl.co/api";
        private const string appLocation = @"C:\TaskBoard\TaskBoard.DesktopApp\TaskBoard.DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_SearchTask()
        {
            // Arrange
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(TasksBoardUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var editTextField = driver.FindElementByAccessibilityId("textBoxSearchText");
            editTextField.SendKeys("Project skeleton");

            //Act
            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();

            //Case1:
            //Thread.Sleep(2000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));       
            var searchLabel = driver.FindElementByXPath("/ Pane[@ClassName =\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]").Text;
 
            // Assert
            Assert.That(searchLabel.Contains("tasks loaded"));
        }
    }
}