using System.ComponentModel.DataAnnotations;

namespace ForumDeDiscussion.Models
{
    public class Member
    {
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_MEMBER = "Membre";
        
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        public string Firstname { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [MinLength(5, ErrorMessage = "Le mot de passe doit avoir au moins 5 caractères.")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le nom d'utilisateur doit avoir au moins 3 caractères.")]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = ROLE_MEMBER;
        public List<Message> Messages { get; set; }

        public Member()
        {
        }
    }
}
