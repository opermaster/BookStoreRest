namespace BookStoreRest.Controllers
{
    using BookStoreRest.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    [Route("/api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OrdersController(DatabaseContext context) {
            _context = context;
        }

        [Authorize]
        [HttpGet("purchases/by-user")]
        public ActionResult<List<OrderResponseDto>> GetPurchasesByUser() {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderResponseDto {
                    Id = o.Id,
                    CreationDate = o.CreationDate,
                    Done = o.Done,
                    SoldDate = o.SoldDate,
                    Book = new BookDto {
                        Id = o.Book.Id,
                        Title = o.Book.Title,
                        Cost = o.Book.Cost,
                        Img = o.Book.Img,
                        AuthorName = o.Book.Author.FirstName + " " + o.Book.Author.SecondName,
                        AuthorId = o.Book.AuthorId,
                        UserLogin = o.Book.User.Login
                    }
                })
                .ToList();

            return Ok(orders);
        }
        [Authorize]
        [HttpGet("by-user")]
        public ActionResult<List<OrderResponseDto>> GetOrdersByUser() {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = _context.Orders
                .Where(o => o.Book.UserId == userId)
                .Select(o => new OrderResponseDto {
                    Id = o.Id,
                    CreationDate = o.CreationDate,
                    Done = o.Done,
                    SoldDate = o.SoldDate,
                    Book = new BookDto {
                        Id = o.Book.Id,
                        Title = o.Book.Title,
                        Cost = o.Book.Cost,
                        Img = o.Book.Img,
                        AuthorName = o.Book.Author.FirstName + " " + o.Book.Author.SecondName,
                        AuthorId = o.Book.AuthorId,
                        UserLogin = o.Book.User.Login
                    }
                })
                .ToList(); 

            return Ok(orders);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateOrder(OrderDto dto) {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var book = _context.Books.Find(dto.BookId);
            
            if (book == null)
                return NotFound("Book not found");
            if (book.UserId == userId)
                return Conflict();

            if (book.Count <= 0)
                return BadRequest("Book out of stock");

            var order = OrderDto.DtoToOrder(dto);
            order.UserId = userId;
            order.CreationDate = DateTime.UtcNow;

            book.Count--;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Created();
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<Order> UpdateOrder(int id, OrderDto dto) {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var order = _context.Orders.Find(id);
            if (order == null)
                return NotFound();

            if (order.UserId != userId)
                return Forbid();

            order.Done = dto.Done;
            order.SoldDate = dto.SoldDate;

            _context.SaveChanges();
            return Ok(order);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id) {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var order = _context.Orders
                .Include(o => o.Book)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            if (order.UserId != userId)
                return Forbid();

            order.Book.Count++;

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return NoContent();
        }
    }

}
