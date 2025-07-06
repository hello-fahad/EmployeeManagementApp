using System.ComponentModel.DataAnnotations;

namespace WebApp.Model
{
    public class CredentialViewModel
    {
        [Required]
        public string? ClientId { get; set; }

        [Required]
        public string? ClientSecret { get; set; }

    }
}
