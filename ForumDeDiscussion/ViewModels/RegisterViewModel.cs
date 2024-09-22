using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ForumDeDiscussion.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; } = string.Empty;

        [Required] [DisplayName("Firstname")] public string Firstname { get; set; } = string.Empty;

        [Required]
        [DisplayName("Nom d'utilisateur")]
        public string UserName { get; set; } = string.Empty;

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
