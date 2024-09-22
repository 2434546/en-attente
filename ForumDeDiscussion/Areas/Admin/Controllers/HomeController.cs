using ForumDeDiscussion.Data.Context;
using ForumDeDiscussion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ForumDeDiscussion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Member.ROLE_ADMIN)]
    public class HomeController : Controller
    {
        private readonly ForumDeDiscussionDbContext _context;

        public HomeController(ForumDeDiscussionDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
