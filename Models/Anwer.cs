using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Q.DTOs;

namespace Q.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; } // Add this property
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }


public static class AnswerEndpoints
{
	public static void MapAnswerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Answer").WithTags(nameof(Answer));

        group.MapGet("/", async (QuizDbContext db) =>
        {
            var answers = await db.Answers.ToListAsync();

            var answersDTOs = answers.Select(a => new AnswerDTO
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList();

            return answersDTOs;
        })
        .WithName("GetAllAnswers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Answer>, NotFound>> (int id, QuizDbContext db) =>
        {
            var answer = await db.Answers.FirstOrDefaultAsync(model => model.Id == id);

            if (answer != null)
            {
                var answerDTO = new AnswerDTO
                {
                    Id = answer.Id,
                    Text = answer.Text,
                    IsCorrect = answer.IsCorrect
                };

                return TypedResults.Ok(answerDTO);
            }
            else
            {
                return TypedResults.NotFound();
            }
        })
        .WithName("GetAnswerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Answer answer, QuizDbContext db) =>
        {
            var affected = await db.Answers
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, answer.Id)
                  .SetProperty(m => m.Text, answer.Text)
                  .SetProperty(m => m.IsCorrect, answer.IsCorrect)
                  .SetProperty(m => m.QuestionId, answer.QuestionId)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAnswer")
        .WithOpenApi();

        group.MapPost("/", async (Answer answer, QuizDbContext db) =>
        {
            db.Answers.Add(answer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Answer/{answer.Id}",answer);
        })
        .WithName("CreateAnswer")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, QuizDbContext db) =>
        {
            var affected = await db.Answers
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAnswer")
        .WithOpenApi();
    }
}}