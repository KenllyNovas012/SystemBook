using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Services;
using System.Services.Modelo;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Test
{
    [TestClass]

    public class BookServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private BookService _bookService;


        [TestInitialize]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://fakerestapi.azurewebsites.net/api/v1/")
            };

            _bookService = new BookService(_httpClient);
        }

        [TestMethod]
        public async Task GetListOfBooks()
        {
            
            var fakeBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Prueba libro 1" },
                new Book { Id = 2, Title = "Prueba libro 2" }
            };

            var responseJson = JsonConvert.SerializeObject(fakeBooks);
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            
            var result = await _bookService.GetAllBooksAsync();

            
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Prueba libro 1", result[0].Title);
        }

        [TestMethod]
        public async Task GetBookById()
        {
            
            var fakeBook = new Book { Id = 1, Title = "Prueba libro 1" };
            var responseJson = JsonConvert.SerializeObject(fakeBook);
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            
            var result = await _bookService.GetBookByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Prueba libro 1", result.Title);
        }

        [TestMethod]
        public async Task CreateBook()
        {
            
            var newBook = new Book { Title = "Nuevo libro" };
            var createdBook = new Book { Id = 400, Title = "Nuevo Libro" };
            var responseJson = JsonConvert.SerializeObject(createdBook);
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

           
            var result = await _bookService.CreateBookAsync(newBook);

            Assert.IsNotNull(result);
            Assert.AreEqual(99, result.Id);
            Assert.AreEqual("Nuevo Libro", result.Title);
        }

        [TestMethod]
        public async Task UpdateBookTrueOnSuccess()
        {
            var updatedBook = new Book { Id = 1, Title = "Actualizar Librok" };
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var result = await _bookService.UpdateBookAsync(1, updatedBook);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteBookTrueOnSuccess()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var result = await _bookService.DeleteBookAsync(1);

            Assert.IsTrue(result);
        }
    }

}

