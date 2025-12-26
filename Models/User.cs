namespace BookStoreRest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public required string PasswordHash { get; set; }

        public List<Book> Books { get; set; } = new();   
        public List<Order> Orders { get; set; } = new(); 
    }

}
