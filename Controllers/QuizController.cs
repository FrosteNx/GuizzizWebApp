using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q.Models;
using Q.ViewModels;
using System.Security.Claims;
using Q.Data;
public class QuizController : Controller
{
    private readonly QuizDbContext _context;
    public QuizController(QuizDbContext context)
    {
        _context = context;
    }
    // GET: Quiz/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }
    // POST: Quiz/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    // Details action
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (quiz == null)
        {
            return NotFound();
        }
        return View(quiz);
    }
    // GET: Quiz/Delete/5
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    // GET: Quiz/CreateQuestion
    public IActionResult CreateQuestion(int quizId)
    {
        var questionViewModel = new QuestionViewModel
        {
            QuizId = quizId
        };
        return View(questionViewModel);
    }
    // POST: Quiz/CreateQuestion
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateQuestion(QuestionViewModel questionViewModel)
    {
        if (ModelState.IsValid)
        {
            var question = new Question
            {
                Text = questionViewModel.Text,
                QuizId = questionViewModel.QuizId
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = question.QuizId });
        }
        return View(questionViewModel);
    }
    // GET: Quiz/EditQuestion/5
    public async Task<IActionResult> EditQuestion(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var question = await _context.Questions.FindAsync(id);
        if (question == null)
        {
            return NotFound();
        }
        var questionViewModel = new QuestionViewModel
        {
            Id = question.Id,
            Text = question.Text,
            QuizId = question.QuizId
        };
        return View(questionViewModel);
    }
    // POST: Quiz/EditQuestion/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditQuestion(int id, [Bind("Id, Text, QuizId")] QuestionViewModel questionViewModel)
    {
        if (id != questionViewModel.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                var question = await _context.Questions.FindAsync(id);
                if (question == null)
                {
                    return NotFound();
                }
                question.Text = questionViewModel.Text;
                _context.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(questionViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", new { id = questionViewModel.QuizId });
        }
        return View(questionViewModel);
    }
    private bool QuestionExists(int id)
    {
        return _context.Questions.Any(e => e.Id == id);
    }
    // GET: Quiz/DeleteQuestion/5
    public async Task<IActionResult> DeleteQuestion(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var question = await _context.Questions
            .Include(q => q.Quiz)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (question == null)
        {
            return NotFound();
        }
        return View(question);
    }
    // POST: Quiz/DeleteQuestion/5
    [HttpPost, ActionName("DeleteQuestion")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuestionConfirmed(int id)
    {
        var question = await _context.Questions
            .Include(q => q.Quiz)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (question == null)
        {
            return NotFound();
        }
        var quizId = question.QuizId;
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = quizId });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAnswerCorrect(int questionId, int answerId)
    {
        var question = await _context.Questions
            .Include(q => q.Quiz)
            .FirstOrDefaultAsync(q => q.Id == questionId);
        if (question == null)
        {
            return NotFound();
        }
        var answers = await _context.Answers
            .Where(a => a.QuestionId == questionId)
            .ToListAsync();
        foreach (var answer in answers)
        {
            answer.IsCorrect = answer.Id == answerId;
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = question.QuizId });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        var answer = await _context.Answers
            .Include(a => a.Question)
            .ThenInclude(q => q.Quiz)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (answer == null)
        {
            return NotFound();
        }
        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = answer.Question.QuizId });
    }
    public async Task<IActionResult> EditAnswer(int id)
    {
        var answer = await _context.Answers.Include(a => a.Question).FirstOrDefaultAsync(a => a.Id == id);
        if (answer == null)
        {
            return NotFound();
        }
        var viewModel = new AnswerViewModel
        {
            Id = answer.Id,
            Text = answer.Text,
            IsCorrect = answer.IsCorrect,
            QuestionId = answer.QuestionId,
            QuizId = answer.Question.QuizId // Ensure this is set
        };
        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAnswer(int id, AnswerViewModel answerViewModel)
    {
        if (id != answerViewModel.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                var answer = await _context.Answers.FindAsync(id);
                if (answer == null)
                {
                    return NotFound();
                }
                answer.Text = answerViewModel.Text;
                answer.IsCorrect = answerViewModel.IsCorrect;
                _context.Update(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = answerViewModel.QuizId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(answerViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(answerViewModel);
    }
    private bool AnswerExists(int id)
    {
        return _context.Answers.Any(e => e.Id == id);
    }
    [Authorize]
    public async Task<IActionResult> TakeQuiz(int id)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id);
        if (quiz == null)
        {
            return NotFound();
        }
        var viewModel = new TakeQuizViewModel
        {
            Quiz = quiz
        };
        return View(viewModel);
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SubmitQuiz(int quizId, Dictionary<string, int> answers)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == quizId);
        if (quiz == null)
        {
            return NotFound();
        }
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var totalQuestions = quiz.Questions.Count;
        var correctAnswers = 0;
        foreach (var question in quiz.Questions)
        {
            if (answers.TryGetValue($"answers_{question.Id}", out var selectedAnswerId))
            {
                var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                if (selectedAnswer != null && selectedAnswer.IsCorrect)
                {
                    correctAnswers++;
                }
            }
        }

        var result = new QuizResult
        {
            QuizId = quizId,
            UserId = userId,
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers,
            TakenOn = DateTime.UtcNow
        };

        _context.QuizResults.Add(result);
        await _context.SaveChangesAsync();


        foreach (var question in quiz.Questions)
        {
            if (answers.TryGetValue($"answers_{question.Id}", out var selectedAnswerId))
            {
                var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                var userAnswer = new UserAnswer
                {
                    QuizResultId = result.Id,
                    QuizId = quizId,
                    QuestionId = question.Id,
                    AnswerId = selectedAnswerId,
                    UserId = userId,
                    IsCorrect = selectedAnswer != null && selectedAnswer.IsCorrect,
                    AnsweredOn = DateTime.UtcNow
                };
                _context.UserAnswers.Add(userAnswer);
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("QuizResult", new { id = result.Id });
    }


    [Authorize]
    public async Task<IActionResult> QuizResult(int id)
    {
        var quizResult = await _context.QuizResults
            .Include(qr => qr.Quiz)
            .Include(qr => qr.UserAnswers)
                .ThenInclude(ua => ua.Question)
            .Include(qr => qr.UserAnswers)
                .ThenInclude(ua => ua.Answer)
            .FirstOrDefaultAsync(qr => qr.Id == id);

        if (quizResult == null)
        {
            return NotFound();
        }

        return View(quizResult);
    }
}