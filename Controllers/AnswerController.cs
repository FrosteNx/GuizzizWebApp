using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q.Data;
using Q.Models;
using System.Linq;
using System.Threading.Tasks;

public class AnswerController : Controller
{
    private readonly QuizDbContext _context;

    public AnswerController(QuizDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int questionId)
    {
        ViewBag.QuestionId = questionId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int questionId, [Bind("Text")] Answer answer)
    {
        if (ModelState.IsValid)
        {
            answer.QuestionId = questionId;
            _context.Add(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Quiz", new { id = answer.Question.QuizId });
        }
        ViewBag.QuestionId = questionId;
        return View(answer);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var answer = await _context.Answers.FindAsync(id);
        if (answer == null)
        {
            return NotFound();
        }
        return View(answer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Text")] Answer answer)
    {
        if (id != answer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(answer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(answer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", "Quiz", new { id = answer.Question.QuizId });
        }
        return View(answer);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var answer = await _context.Answers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (answer == null)
        {
            return NotFound();
        }

        return View(answer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var answer = await _context.Answers.FindAsync(id);
        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Quiz", new { id = answer.Question.QuizId });
    }

    private bool AnswerExists(int id)
    {
        return _context.Answers.Any(e => e.Id == id);
    }
}