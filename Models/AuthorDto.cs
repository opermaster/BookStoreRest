namespace BookStoreRest.Models
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string SecondName { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        public static Author DtoToAuthor(AuthorDto dto) {
            return new Author {
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,
                BirthDate = DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc)
            };
        }
    }
}
