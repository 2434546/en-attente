using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ForumDeDiscussion.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Mot de passe")]
        public string Password { get; set; } = string.Empty;
    }
}
