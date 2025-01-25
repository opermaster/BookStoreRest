using BookStoreRest.Models;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public ActionResult CreateBook(Book book) {
            _context.Books.Add(book);
            _context.SaveChanges();
            return Ok();
        }
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


            _context.SaveChanges();
            return book;
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteBook(int id) {
            Book? book = _context.Books.Find(id);
            if (book == null) {
                return NotFound();
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
