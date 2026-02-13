using BookStoreRest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
namespace BookStoreRest.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class BooksController : ControllerBase {
        private readonly DatabaseContext _context;
        public BooksController(DatabaseContext context) {
            _context = context;
        }
        [HttpGet("all-books")]
        public ActionResult<List<BookDto>> GetBooks() {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.User)
                .Select(b => new BookDto {
                    Title = b.Title,
                    Id = b.Id,
                    Synapsis = b.Synapsis,
                    PublishDate = b.PublishDate,
                    Cost = b.Cost,
                    Count = b.Count,
                    Img = b.Img,
                    AuthorName = b.Author.FirstName + " " + b.Author.SecondName,
                    AuthorId = b.AuthorId,
                    UserLogin = b.User.Login
                })
                .ToList();
            return books;
        }
        [Authorize]
        [HttpGet("by-user")]
        public ActionResult<List<BookDto>> GetBooksByUserId() {
            var books = _context.Books
                .Where(b=> b.UserId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .Select(b => new BookDto {
                    Title = b.Title,
                    Synapsis = b.Synapsis,
                    PublishDate = b.PublishDate,
                    Id = b.Id,
                    Cost = b.Cost,
                    Count = b.Count,
                    Img = b.Img,
                    AuthorName = b.Author.FirstName + " " + b.Author.SecondName,
                    AuthorId = b.AuthorId,
                    UserLogin = b.User.Login
                })
                .ToList();
            return books;
        }
        [HttpGet("by-bookid/{id}")]
        public ActionResult<Book> GetBookById(int id) {
            Book? book = _context.Books.Find(id);
            return book == null? NotFound() : book;
        }
        [HttpGet("search/{name}")]
        public ActionResult<List<Book>> SearchBooks(string name) {
            var books = _context.Books
                .Where(b => b.Title.Contains(name))
                .Select(b => new BookDto {
                    Title = b.Title,
                    Synapsis = b.Synapsis,
                    PublishDate = b.PublishDate,
                    Id = b.Id,
                    Cost = b.Cost,
                    Count = b.Count,
                    Img = b.Img,
                    AuthorName = b.Author.FirstName + " " + b.Author.SecondName,
                    AuthorId = b.AuthorId,
                    UserLogin = b.User.Login
                })
                .ToList();

            if (!books.Any())
                return NotFound();

            return Ok(books);
        }
        [HttpGet("by-name")]
        public ActionResult<Book> GetBookByName(string name) {
            var book = _context.Books
                .FirstOrDefault(b => b.Title == name);

            return book == null ? NotFound() : Ok(book);
        }
        [Authorize]
        [HttpPost("new-book")]
        public ActionResult CreateBook(BookDto _book) {
            Book book = BookDto.DtoToBook(_book);
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            book.UserId = userId;
            _context.Books.Add(book);
            _context.SaveChanges();
            return Ok();
        }
        [Authorize]
        [HttpPut("by-bookid/{id}")]
        public ActionResult<Book> UpdateBook(int id ,BookDto newBook) {
            Book? book = _context.Books.Find(id);
            if(book == null || newBook.Id != id) {
                return NotFound();
            }
            book.Title= newBook.Title;
            book.AuthorId = newBook.AuthorId;
            book.PublishDate = newBook.PublishDate;
            book.Synapsis = newBook.Synapsis;
            book.Cost = newBook.Cost;
            book.Img = newBook.Img;
            book.Count = newBook.Count;
            book.AuthorId = newBook.AuthorId;

            _context.SaveChanges();
            return book;
        }
        [Authorize]
        [HttpDelete("by-bookid/{id}")]
        public ActionResult DeleteBook(int id) {
            Book? book = _context.Books.Find(id);
            var userId = int.Parse(User.FindFirst("id").Value);

            if (book == null) {
                return NotFound();
            }
            if(book.UserId != userId) { return Forbid(); };
            _context.Books.Remove(book);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
