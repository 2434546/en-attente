using ForumDeDiscussion.Data.Context;
using ForumDeDiscussion.Helpers;
using ForumDeDiscussion.Models;
using ForumDeDiscussion.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace ForumDeDiscussion.Controllers
{
    public class AuthController : Controller
    {
        private readonly ForumDeDiscussionDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public AuthController(ILogger<HomeController> logger, ForumDeDiscussionDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Members.AnyAsync(m => m.UserName == viewModel.UserName || m.Email == viewModel.Email);
                if (existingUser)
                {
                    ModelState.AddModelError("", "Un utilisateur avec ce nom ou cet email existe déjà.");
                    return View(viewModel);
                }

                string hashedPassword = CryptographyHelper.HashPassword(viewModel.Password);

                var newMember = new Member
                {
                    Name = viewModel.Name,
                    Firstname = viewModel.Firstname,
                    UserName = viewModel.UserName,
                    Email = viewModel.Email,
                    Password = hashedPassword,
                };

                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel viewModel = new();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel, string? returnurl)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Members.FirstOrDefaultAsync(m => m.Email == viewModel.Email);

                if (user != null)
                {
                    bool isValid = CryptographyHelper.ValidateHashedPassword(viewModel.Password, user.Password);

                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, viewModel.Email),
                            new Claim(ClaimTypes.Role, user.Role),
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity));

                        if (!string.IsNullOrWhiteSpace(returnurl) && Url.IsLocalUrl(returnurl))
                        {
                            return LocalRedirect(returnurl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("Password", "Invalid login attempt.");
                _logger.LogWarning("User not found.");
            }

            return View(viewModel);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
        
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Members.FirstOrDefaultAsync(m => m.Id == int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                FirstName = user.Firstname
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.Members.FirstOrDefaultAsync(m => m.Id == int.Parse(userId));

                if (user != null)
                {
                    user.UserName = viewModel.UserName;
                    user.Email = viewModel.Email;
                    user.Name = viewModel.Name;
                    user.Firstname = viewModel.FirstName;
                    
                    if (!string.IsNullOrWhiteSpace(viewModel.NewPassword))
                    {
                        user.Password = CryptographyHelper.HashPassword(viewModel.NewPassword);
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Profil mis à jour avec succès.";
                    return RedirectToAction(nameof(EditProfile));
                }

                ModelState.AddModelError("", "Une erreur est survenue lors de la mise à jour du profil.");
            }

            return View(viewModel);
        }

    }
}
