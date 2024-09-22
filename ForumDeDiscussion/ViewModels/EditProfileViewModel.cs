using System.ComponentModel.DataAnnotations;

namespace ForumDeDiscussion.ViewModels;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'adresse email est requise.")]
    [EmailAddress(ErrorMessage = "Adresse email non valide.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom est requis.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prénom est requis.")]
    public string FirstName { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}