using System.ComponentModel.DataAnnotations;
namespace Findcas.Web.Client.Models
{
    public class RegisterDto
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string? Token { get; set; }
        public string? Mensaje { get; set; }
    }
}
