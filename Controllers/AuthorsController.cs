using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpPost]
        public ActionResult<Author> CreateAuthor(AuthorDto author) {
            author.BirthDate = DateTime.SpecifyKind(author.BirthDate, DateTimeKind.Utc);
            _context.Authors.Add(AuthorDto.DtoToAuthor(author));
            _context.SaveChanges();
            return Created();
        }
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<Author> UpdateAuthor(int id, Author newAuthor) {
            Author? author = _context.Authors.Find(id);
            if (author == null || newAuthor.Id != id) {
                return NotFound();
            }

            author.FirstName = newAuthor.FirstName;
            author.SecondName = newAuthor.SecondName;
            author.BirthDate = newAuthor.BirthDate;

            _context.SaveChanges();
            return author;
        }
    }
}
