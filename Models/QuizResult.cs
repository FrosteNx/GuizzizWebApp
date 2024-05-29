using Microsoft.AspNetCore.Http.HttpResults;
using Q.Data;
using Q.Models;
using Microsoft.EntityFrameworkCore;
using Q.DTOs;

namespace Q.Models
{
    public class QuizResult
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime TakenOn { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
    }
}

public static class QuizResultEndpoints
{
    public static void MapQuizResultEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/QuizResult").WithTags(nameof(QuizResult));

        group.MapGet("/", async (QuizDbContext db) =>
        {
            var quizResults = await db.QuizResults.ToListAsync();

            var quizResultsDTOs = quizResults.Select(q => new QuizResultDTO
            {
                Id = q.Id,
                QuizId = q.QuizId,
                UserId = q.UserId,
                TotalQuestions = q.TotalQuestions,
                CorrectAnswers = q.CorrectAnswers,
                score = q.CorrectAnswers / q.TotalQuestions,
                TakenOn = q.TakenOn,
                UserAnswers = q.UserAnswers.Select(ua => new UserAnswerDTO
                {
                    Id = ua.Id,
                    QuestionId = ua.QuestionId,
                    IsCorrect = ua.IsCorrect
                }).ToList()

            }).ToList();

            return quizResultsDTOs;
        })
        .WithName("GetAllQuizResults")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<QuizResult>, NotFound>> (int id, QuizDbContext db) =>
        {
            return await db.QuizResults.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is QuizResult model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetQuizResultById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, QuizResult quizResult, QuizDbContext db) =>
        {
            var affected = await db.QuizResults
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, quizResult.Id)
                  .SetProperty(m => m.QuizId, quizResult.QuizId)
                  .SetProperty(m => m.UserId, quizResult.UserId)
                  .SetProperty(m => m.TotalQuestions, quizResult.TotalQuestions)
                  .SetProperty(m => m.CorrectAnswers, quizResult.CorrectAnswers)
                  .SetProperty(m => m.TakenOn, quizResult.TakenOn)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateQuizResult")
        .WithOpenApi();

        group.MapPost("/", async (QuizResult quizResult, QuizDbContext db) =>
        {
            db.QuizResults.Add(quizResult);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/QuizResult/{quizResult.Id}", quizResult);
        })
        .WithName("CreateQuizResult")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, QuizDbContext db) =>
        {
            var affected = await db.QuizResults
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteQuizResult")
        .WithOpenApi();
    }
}
