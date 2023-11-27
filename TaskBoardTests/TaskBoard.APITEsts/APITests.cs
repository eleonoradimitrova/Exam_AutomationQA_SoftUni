using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace TaskBoard.APITEsts
{
    public class APITests
    {
        private const string url = "https://taskboard.nakov.repl.co/api/tasks";
        private RestClient client;
        private RestRequest request;
        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_ListAllTasks_CheckFirstDoneTask()
        {
            // Arrange
            this.request = new RestRequest(url);

            // Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            foreach (var task in tasks)
            {
                if (task.board.name == "Done")
                {
                    Assert.That(task.title, Is.EqualTo("Project skeleton"));
                    break;
                }
            }
        }
        [Test]
        public void Test_ListAllTasks_AndFindTaskHomepage()
        {
            // Arrange
            this.request = new RestRequest(url+ "/search/[home]");

            // Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));
            Assert.That(tasks[0].title, Is.EqualTo("Home page"));
        }
        [Test]
        public void Test_ListAllTasks_AndCheckMissingTask()
        {
            // Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "missin462378462378");

            // Act
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(response.Content);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.EqualTo(0));
        }
        [Test]
        public void Test_CreateInvalidTask()
        {
            // Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                title = " ",
                description = " ",
                board = " "
            };
            request.AddJsonBody(body);

            // Act
            var response = this.client.Execute(request, Method.Post);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public void Test_CreateValidTask()
        {
            // Arrange
            this.request = new RestRequest(url);
            var body = new
            {
                title = "Test Valid Data",
                description = "Test to create valid task",
                board = "Open"
            };
            request.AddJsonBody(body);

            // Act
            var response = this.client.Execute(request, Method.Post);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var allTasks = this.client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Tasks>>(allTasks.Content);

            var lastTask = tasks.Last();


            Assert.That(lastTask.title, Is.EqualTo(body.title));
            Assert.That(lastTask.description, Is.EqualTo(body.description));
            Assert.That(lastTask.board.name, Is.EqualTo("Open"));
        }
    }
}