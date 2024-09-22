using ForumDeDiscussion.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ForumDeDiscussion.Data.Context;
using ForumDeDiscussion.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForumDeDiscussion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ForumDeDiscussionDbContext _context;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ForumDeDiscussionDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var sections = await _context.Sections
                .Select(section => new SectionViewModel
                {
                    SectionId = section.Id,
                    SectionName = section.Title,
                    TopicCount = section.Subjects.Count,
                    MessageCount = section.Subjects.Sum(sujet => sujet.Messages.Count),
                    LastMessageDate = section.Subjects
                        .SelectMany(s => s.Messages)
                        .OrderByDescending(m => m.Date)
                        .FirstOrDefault().Date,
                    LastMessageId = section.Subjects
                        .SelectMany(s => s.Messages)
                        .OrderByDescending(m => m.Date)
                        .FirstOrDefault().Id
                })
                .ToListAsync();
            return View(sections);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}