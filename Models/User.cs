using CSharpApi.Constants;

namespace CSharpApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public string Role { get; set; } = RoleConstants.User;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}