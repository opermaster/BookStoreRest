namespace BookStoreRest.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        public List<Book> Books { get; set; } = new();
    }

}
