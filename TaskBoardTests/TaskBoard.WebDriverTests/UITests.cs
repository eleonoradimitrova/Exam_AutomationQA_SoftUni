using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace TaskBoard.WebDriverTests
{
    public class UITests
    {
        private const string url = "https://taskboard.nakov.repl.co/";
        // private const string url = "http://localhost:8080";
        private WebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ListTasks_CheckFirstTask()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var tasksLink = driver.FindElement(By.LinkText("Task Board"));

            // Act
            tasksLink.Click();

            // Assert
            var title = driver.FindElement(By.XPath("/html/body/main/div/div[3]/table/tbody/tr[1]/td")).Text;
            Assert.That(title, Is.EqualTo("Project skeleton"));
        }
        [Test]
        public void Test_Listtasks_SearchHomePage()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            // Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("Home page");
            driver.FindElement(By.Id("search")).Click();


            // Assert
            var task = driver.FindElement(By.XPath("//*/tbody/tr[1]/td")).Text;
            Assert.That(task, Is.EqualTo("Home page"));           
        }
        [Test]
        public void Test_Listtasks_EmptySearchResult()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            // Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("missing12345");
            driver.FindElement(By.Id("search")).Click();


            // Assert
            var task = driver.FindElement(By.XPath("//*[@id=\"searchResult\"]")).Text;
            Assert.That(task, Is.EqualTo("No tasks found."));
        }
        [Test]
        public void Test_CreateTask_InvalidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            // Act
            var title = driver.FindElement(By.Id("title"));
            title.SendKeys("");
            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();

            // Assert
            var errorMessage = driver.FindElement(By.XPath("/html/body/main/div")).Text;
            Assert.That(errorMessage, Is.EqualTo("Error: Title cannot be empty!"));
        }
        [Test]
        public void Test_CreateTask_ValidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var title = "Title" + DateTime.Now.Ticks;
            var description = "Description" + DateTime.Now.Ticks;
            var board = "Open";    

            // Act
            driver.FindElement(By.Id("title")).SendKeys(title);
            driver.FindElement(By.Id("description")).SendKeys(description);
            driver.FindElement(By.Id("boardName")).SendKeys(board);

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();

            // Assert
            var allTasks = driver.FindElements(By.CssSelector("table.task-entry"));
            var lastTask = allTasks.Last();

            var titleLabel = lastTask.FindElement(By.CssSelector("tr.title > td")).Text;
            var descriptionLabel = lastTask.FindElement(By.CssSelector("tr.description > td")).Text;

            Assert.That(titleLabel, Is.EqualTo(title));
            Assert.That(descriptionLabel, Is.EqualTo(description));
        }
    }
}