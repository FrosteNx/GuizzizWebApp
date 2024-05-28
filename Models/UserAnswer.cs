using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Q.Data;

namespace Q.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredOn { get; set; }

        public int QuizResultId { get; set; }
        public QuizResult QuizResult { get; set; }
    }


public static class UserAnswerEndpoints
{
	public static void MapUserAnswerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/UserAnswer").WithTags(nameof(UserAnswer));

        group.MapGet("/", async (QuizDbContext db) =>
        {
            return await db.UserAnswers.ToListAsync();
        })
        .WithName("GetAllUserAnswers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<UserAnswer>, NotFound>> (int id, QuizDbContext db) =>
        {
            return await db.UserAnswers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is UserAnswer model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserAnswerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, UserAnswer userAnswer, QuizDbContext db) =>
        {
            var affected = await db.UserAnswers
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, userAnswer.Id)
                  .SetProperty(m => m.QuizId, userAnswer.QuizId)
                  .SetProperty(m => m.QuestionId, userAnswer.QuestionId)
                  .SetProperty(m => m.AnswerId, userAnswer.AnswerId)
                  .SetProperty(m => m.UserId, userAnswer.UserId)
                  .SetProperty(m => m.IsCorrect, userAnswer.IsCorrect)
                  .SetProperty(m => m.AnsweredOn, userAnswer.AnsweredOn)
                  .SetProperty(m => m.QuizResultId, userAnswer.QuizResultId)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUserAnswer")
        .WithOpenApi();

        group.MapPost("/", async (UserAnswer userAnswer, QuizDbContext db) =>
        {
            db.UserAnswers.Add(userAnswer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/UserAnswer/{userAnswer.Id}",userAnswer);
        })
        .WithName("CreateUserAnswer")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, QuizDbContext db) =>
        {
            var affected = await db.UserAnswers
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUserAnswer")
        .WithOpenApi();
    }
}}