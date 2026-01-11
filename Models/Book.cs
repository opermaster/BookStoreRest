namespace BookStoreRest.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Synapsis { get; set; } = null!;
        public string Img { get; set; } = null!;
        public DateTime PublishDate { get; set; }

        public decimal Cost { get; set; }
        public int Count { get; set; } 

        
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Order> Orders { get; set; } = new();
    }

}
