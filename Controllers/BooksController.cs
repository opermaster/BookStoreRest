using BookStoreRest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
namespace BookStoreRest.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public BooksController(DatabaseContext context) {
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Book>> GetBooks() {
            return _context.Books.ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id) {
            Book? book = _context.Books.Find(id);
            return book == null? NotFound() : book;
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateBook(BookDto _book) {
            Book book = BookDto.DtoToBook(_book);
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            book.UserId = userId;
            _context.Books.Add(book);
            _context.SaveChanges();
            return Ok();
        }
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<Book> UpdateBook(int id ,Book newBook) {
            Book? book = _context.Books.Find(id);
            if(book == null || newBook.Id != id) {
                return NotFound();
            }
            book.Author = newBook.Author;
            book.Title= newBook.Title;
            book.AuthorId = newBook.AuthorId;
            book.PublishDate = newBook.PublishDate;
            book.Synapsis = newBook.Synapsis;
            book.Cost = newBook.Cost;
            book.Count = newBook.Count;
            book.AuthorId = newBook.AuthorId;

            _context.SaveChanges();
            return book;
        }
        [Authorize]
        [HttpDelete("{id}")]
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
