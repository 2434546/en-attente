using System.Security.Claims;
using ForumDeDiscussion.Data.Context;
using ForumDeDiscussion.Models;
using ForumDeDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumDeDiscussion.Controllers;

public class SubjectsController : Controller
{
    private readonly ForumDeDiscussionDbContext _context;
        
    private readonly ILogger<SubjectsController> _logger;

    public SubjectsController(ForumDeDiscussionDbContext context, ILogger<SubjectsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Subjects(int sectionId)
    {
        var subjects = await _context.Subjects
            .Where(s => s.SectionId == sectionId)
            .Select(subject => new SubjectViewModel
            {
                SubjectId = subject.Id,
                Name = subject.Title,
            }).ToListAsync();
        
        ViewBag.SectionId = sectionId;
        
        return View(subjects);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddSubjectViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subject = new Subject
            {
                SectionId = viewModel.SectionId,
                Title = viewModel.Name,
                UserId = userId
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Subjects), new { sectionId = viewModel.SectionId });
        }

        TempData["Error"] = "Une erreur est survenue lors de la création du sujet.";
        return RedirectToAction(nameof(Subjects), new { sectionId = viewModel.SectionId });
    }

    [HttpGet]
    public async Task<IActionResult> GetSubjectDetails(int id)
    {
        var subject = await _context.Subjects
            .Where(s => s.Id == id)
            .Select(s => new EditSubjectViewModel
            {
                SubjectId = s.Id,
                SectionId = s.SectionId,
                Title = s.Title
            }).FirstOrDefaultAsync();

        if (subject == null)
        {
            return NotFound();
        }

        return Json(subject);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditSubjectViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var subject = await _context.Subjects.FindAsync(viewModel.SubjectId);
            if (subject != null)
            {
                subject.Title = viewModel.Title;
                subject.SectionId = viewModel.SectionId;
                _context.Update(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Subjects), new { sectionId = viewModel.SectionId });
            }
        }

        TempData["Error"] = "Erreur lors de la modification du sujet.";
        return RedirectToAction(nameof(Subjects), new { sectionId = viewModel.SectionId });
    }


    public async Task<IActionResult> Delete(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
        {
            return NotFound();
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (subject.UserId != userId && !User.IsInRole("Admin"))
        {
            TempData["Error"] = "Vous n'avez pas l'autorisation de supprimer ce sujet.";
            return RedirectToAction(nameof(Subjects), new { sectionId = subject.SectionId });
        }

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Sujet supprimé avec succès.";
        return RedirectToAction(nameof(Subjects), new { sectionId = subject.SectionId });
    }

}