using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q.Data;
using Q.Models;
using System.Linq;
using System.Threading.Tasks;

public class QuizController : Controller
{
    private readonly QuizDbContext _context;

    public QuizController(QuizDbContext context)
    {
        _context = context;
    }

    // GET: Quiz/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Quiz/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title")] Quiz quiz)
    {
        if (ModelState.IsValid)
        {
            _context.Add(quiz);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(quiz);
    }

    // GET: Quiz/Index
    public async Task<IActionResult> Index()
    {
        return View(await _context.Quizzes.ToListAsync());
    }

    // GET: Quiz/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quiz = await _context.Quizzes.FindAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        return View(quiz);
    }

    // POST: Quiz/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Quiz quiz)
    {
        if (id != quiz.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(quiz);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(quiz.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(quiz);
    }

    // GET: Quiz/Details/5
    // GET: Quiz/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quiz = await _context.Quizzes
            .Include(q => q.Questions) // Ensure questions are included
            .ThenInclude(q => q.Answers) // Include answers if needed
            .FirstOrDefaultAsync(m => m.Id == id);

        if (quiz == null)
        {
            return NotFound();
        }

        return View(quiz);
    }

    // GET: Quiz/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quiz = await _context.Quizzes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (quiz == null)
        {
            return NotFound();
        }

        return View(quiz);
    }

    // POST: Quiz/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var quiz = await _context.Quizzes.FindAsync(id);
        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool QuizExists(int id)
    {
        return _context.Quizzes.Any(e => e.Id == id);
    }
}