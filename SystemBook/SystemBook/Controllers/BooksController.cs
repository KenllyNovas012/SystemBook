using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Services.Modelo;
using System.Services;
using System.Threading.Tasks;
using System;

namespace SystemBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        /// <summary>
        /// Metodo para Obtener el listado de los libros.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(new { data = books });
            }
            catch (Exception)
            {
                return BadRequest("Hubo  un error al cargar los datos");
            }

        }
        // GET: api/Books/
        /// <summary>
        /// Metodo para Obtener un libros.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {

            try
            {
                var result = await _bookService.GetBookByIdAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Hubo  un error al cargar");
            }
        }

        // Post: api/Books
        /// <summary>
        /// Metodo para Guarda un libro.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdBook = await _bookService.CreateBookAsync(book);
                if (createdBook != null)
                    return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);

                return BadRequest("No se pudo crear el libro.");
            }
            catch (Exception e)
            {
                return BadRequest("Hubo un error al guardar: " + e.Message);
            }
        }
        // Put: api/Books/id
        /// <summary>
        /// Metodo para actualizar un libro.
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await _bookService.UpdateBookAsync(id, book);
                if (!success)
                    return NotFound("El libro no fue actualizado.");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("Hubo un error al actualizar: " + e.Message);
            }
        }
        // GET: api/Books/id
        /// <summary>
        /// Metodo para eliminar un libros.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var success = await _bookService.DeleteBookAsync(id);
                if (!success)
                    return NotFound("El libro no fue encontrado.");

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("Hubo un error al eliminar: " + e.Message);
            }
        }
    }
}
