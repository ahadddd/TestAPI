using System.ComponentModel.DataAnnotations;

namespace TestAPI.Dto
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
