using System.ComponentModel.DataAnnotations;

namespace CSharpApi.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string Email { get; set; } = string.Empty;

        [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres")]
        public required string Password { get; set; } = string.Empty;
    }
}