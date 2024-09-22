using System.Security.Claims;
using ForumDeDiscussion.Data.Context;
using ForumDeDiscussion.Models;
using ForumDeDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumDeDiscussion.Controllers;

public class MessagesController : Controller
{
    private readonly ForumDeDiscussionDbContext _context;
        
    private readonly ILogger<MessagesController> _logger;
    
    public MessagesController(ForumDeDiscussionDbContext context, ILogger<MessagesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Messages(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var messages = await _context.Messages
            .Where(m => m.SubjectId == id)
            .Select(message => new MessageViewModel
            {
                MessageId = message.Id,
                Content = message.Content,
                Author = message.Member.UserName,
                Date = message.Date
            }).ToListAsync();

        ViewBag.SujetId = id;
        ViewBag.CurrentUserId = userId;

        return View(messages);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(int subjectId, MessageViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Content))
        {
            TempData["Error"] = "Veuillez entrer un message.";
            return RedirectToAction(nameof(Messages), new { id = subjectId });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var message = new Message
            {
                Content = model.Content,
                SubjectId = subjectId,
                MemberId = int.Parse(userId),
                Date = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Message envoyé avec succès!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'envoi d'un message.");
            TempData["Error"] = "Une erreur s'est produite lors de l'envoi du message.";
        }

        return RedirectToAction(nameof(Messages), new { id = subjectId });
    }


    [HttpGet]
    public async Task<IActionResult> GetMessageDetails(int id)
    {
        var message = await _context.Messages
            .Where(m => m.Id == id)
            .Select(m => new { MessageId = m.Id, Content = m.Content })
            .FirstOrDefaultAsync();

        if (message == null)
        {
            return NotFound();
        }

        return Json(message);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditMessage(int messageId, string content)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null || message.MemberId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
        {
            TempData["Error"] = "Vous n'avez pas l'autorisation de modifier ce message.";
            return RedirectToAction(nameof(Messages), new { id = message.SubjectId });
        }

        message.Content = content;
        _context.Update(message);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Message modifié avec succès.";
        return RedirectToAction(nameof(Messages), new { id = message.SubjectId });
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null || message.MemberId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
        {
            TempData["Error"] = "Vous n'avez pas l'autorisation de supprimer ce message.";
            return RedirectToAction(nameof(Messages), new { id = message.SubjectId });
        }

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Message supprimé avec succès.";
        return RedirectToAction(nameof(Messages), new { id = message.SubjectId });
    }
}