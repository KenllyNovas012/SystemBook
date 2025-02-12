using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Services.Modelo;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Services
{
    public class  BookService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://fakerestapi.azurewebsites.net/api/v1/Books";

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener todos los libros
        public async Task<List<Book>> GetAllBooksAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Book>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Obtener un libro por ID
        public async Task<Book> GetBookByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Book>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Crear un nuevo libro
        public async Task<Book> CreateBookAsync(Book book)
        {
            var bookJson = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, bookJson);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Book>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Actualizar un libro
        public async Task<bool> UpdateBookAsync(int id, Book book)
        {
            var bookJson = new StringContent(JsonSerializer.Serialize(book), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/{id}", bookJson);
            return response.IsSuccessStatusCode;
        }

        // Eliminar un libro
        public async Task<bool> DeleteBookAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
