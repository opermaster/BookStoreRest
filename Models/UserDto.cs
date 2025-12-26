namespace BookStoreRest.Models
{
    public class UserDto
    {
        public required string Login { get; set; }
        public required string Password { get; set; }

        public static User DtoToUser(UserDto dto) {
            return new User() {
                Login = dto.Login,
                PasswordHash = HashPassword(dto.Password),
            };
        }

        private static string HashPassword(string password) {

            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash) {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
