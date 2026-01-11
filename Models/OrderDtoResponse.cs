namespace BookStoreRest.Models
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Done { get; set; }
        public DateTime? SoldDate { get; set; }

        public BookDto Book { get; set; } = null!;
    }
}
