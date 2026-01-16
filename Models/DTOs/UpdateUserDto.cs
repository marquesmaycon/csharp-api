using System.ComponentModel.DataAnnotations;

namespace CSharpApi.Models.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public required string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string Email { get; set; } = string.Empty;

        public string? Role { get; set; }
    }
}