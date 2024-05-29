using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q.Data;
using Q.Models;

public class QuestionController : Controller
{
    private readonly QuizDbContext _context;

    public QuestionController(QuizDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int quizId)
    {
        ViewBag.QuizId = quizId;
        return View(new QuestionViewModel { QuizId = quizId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Text, QuizId")] QuestionViewModel questionViewModel)
    {
        Console.WriteLine("Received POST request to create Question");

        if (ModelState.IsValid)
        {
            Console.WriteLine("ModelState is valid");
            Console.WriteLine($"QuizId: {questionViewModel.QuizId}, Text: {questionViewModel.Text}");

            var question = new Question
            {
                Text = questionViewModel.Text,
                QuizId = questionViewModel.QuizId
            };

            _context.Add(question);
            await _context.SaveChangesAsync();
            Console.WriteLine("Question saved successfully");

            return RedirectToAction("Details", "Quiz", new { id = question.QuizId });
        }
        else
        {
            Console.WriteLine("ModelState is invalid");

            foreach (var state in ModelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                var errors = state.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }
        }

        ViewBag.QuizId = questionViewModel.QuizId;
        return View(questionViewModel);
    }

    public IActionResult CreateAnswer(int questionId)
    {
        var question = _context.Questions.Include(q => q.Quiz).FirstOrDefault(q => q.Id == questionId);
        if (question == null)
        {
            return NotFound();
        }

        var answerViewModel = new AnswerViewModel
        {
            QuestionId = questionId,
            QuizId = question.QuizId 
        };
        return View(answerViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAnswer(AnswerViewModel answerViewModel)
    {
        if (ModelState.IsValid)
        {
            var answer = new Answer
            {
                Text = answerViewModel.Text,
                QuestionId = answerViewModel.QuestionId
            };

            _context.Add(answer);
            await _context.SaveChangesAsync();

            var question = await _context.Questions
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == answerViewModel.QuestionId);

            if (question == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Quiz", new { id = question.QuizId });
        }
        return View(answerViewModel);
    }

    public async Task<IActionResult> DeleteAnswer(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var answer = await _context.Answers
            .Include(a => a.Question)
            .ThenInclude(q => q.Quiz)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (answer == null)
        {
            return NotFound();
        }

        return View(answer);
    }

    [HttpPost, ActionName("DeleteAnswer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAnswerConfirmed(int id)
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

        return RedirectToAction("Details", "Quiz", new { id = answer.Question.QuizId });
    }

    public async Task<IActionResult> Edit(int? id)
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
            Text = question.Text,
            QuizId = question.QuizId
        };

        return View(questionViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Text, QuizId")] QuestionViewModel questionViewModel)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            question.Text = questionViewModel.Text;
            question.QuizId = questionViewModel.QuizId;

            try
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(question.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", "Quiz", new { id = question.QuizId });
        }

        return View(questionViewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var question = await _context.Questions.FindAsync(id);
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Quiz", new { id = question.QuizId });
    }

    private bool QuestionExists(int id)
    {
        return _context.Questions.Any(e => e.Id == id);
    }

    public async Task<IActionResult> EditAnswer(int? id)
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

        var answerViewModel = new AnswerViewModel
        {
            Text = answer.Text,
            QuestionId = answer.QuestionId
        };

        return View(answerViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAnswer(int id, [Bind("Text, QuestionId")] AnswerViewModel answerViewModel)
    {
        var answer = await _context.Answers.FindAsync(id);
        if (answer == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            answer.Text = answerViewModel.Text;
            answer.QuestionId = answerViewModel.QuestionId;

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

        return View(answerViewModel);
    }

    private bool AnswerExists(int id)
    {
        return _context.Answers.Any(e => e.Id == id);
    }
}