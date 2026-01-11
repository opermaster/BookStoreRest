namespace BookStoreRest.Models
{
    public class OrderDto
    {
        public DateTime CreationDate { get; set; }
        public bool Done { get; set; }
        public DateTime? SoldDate { get; set; } 
        public int UserId { get; set; }
        public int BookId { get; set; }
        public static Order DtoToOrder(OrderDto dto) {
            return new Order {
                CreationDate = DateTime.SpecifyKind(dto.CreationDate, DateTimeKind.Utc),
                Done = dto.Done,
                SoldDate = dto.SoldDate.HasValue
                    ? DateTime.SpecifyKind(dto.SoldDate.Value, DateTimeKind.Utc)
                    : null,

                UserId = dto.UserId,
                BookId = dto.BookId
            };
        }
    }

}
