using Microsoft.AspNetCore.Mvc;
using BookStoreRest.Models;
using System;
namespace BookStoreRest.Controllers
{
    [Route("/api/{controller}")]
    [ApiController]
    public class AuthorsController: ControllerBase
    {
        private readonly DatabaseContext _context;
        public AuthorsController(DatabaseContext context) {
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Author>> GetAuthors() {
            return _context.Authors.ToList();
        }
        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(int id) {
            Author? author = _context.Authors.Find(id);

            return author == null ? NotFound(): author;
        }
        [HttpPost]
        public ActionResult<Author> CreateAuthor(Author author) {
            author.BirthDate = DateTime.SpecifyKind(author.BirthDate, DateTimeKind.Utc);
            _context.Authors.Add(author);
            _context.SaveChanges();
            return Created();
        }
        [HttpPut("{id}")]
        public ActionResult<Author> UpdateAuthor(int id, Author newAuthor) {
            Author? author = _context.Authors.Find(id);
            if (author == null || newAuthor.Id != id) {
                return NotFound();
            }

            author.FirstName = newAuthor.FirstName;
            author.SecondName = newAuthor.SecondName;
            author.Nationality = newAuthor.Nationality;
            author.BirthDate = newAuthor.BirthDate;

            _context.SaveChanges();
            return author;
        }
        [HttpDelete("{id}")]
        public ActionResult<Author> DeleteAuthor(int id) {
            Author? author = _context.Authors.Find(id);
            if (author == null) {
                return NotFound();
            }
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
