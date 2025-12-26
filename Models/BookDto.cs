namespace BookStoreRest.Models
{
    public class BookDto
    {
        public string Title { get; set; } = null!;
        public string Synapsis { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }

        public int AuthorId { get; set; }

        public static Book DtoToBook(BookDto dto) {
            return new Book {
                Title = dto.Title,
                Synapsis = dto.Synapsis,
                PublishDate = DateTime.SpecifyKind( dto.PublishDate,DateTimeKind.Utc),
                Cost = dto.Cost,
                Count = dto.Count,
                AuthorId = dto.AuthorId,
            };
        }
    }
}
