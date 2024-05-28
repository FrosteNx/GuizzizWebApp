using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q.Data;
using Q.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class ResultsController : Controller
{
    private readonly QuizDbContext _context;
    private readonly UserManager<User> _userManager;

    public ResultsController(QuizDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Results
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var isAdmin = User.IsInRole("Admin");

        var results = isAdmin ?
            await _context.QuizResults.Include(r => r.Quiz).Include(r => r.User).ToListAsync() :
            await _context.QuizResults.Where(r => r.UserId == userId).Include(r => r.Quiz).Include(r => r.User).ToListAsync();

        return View(results);
    }

    // GET: Results/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await _context.QuizResults
            .Include(r => r.Quiz)
            .Include(r => r.User)
            .Include(r => r.UserAnswers)
            .ThenInclude(ua => ua.Question)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }
}