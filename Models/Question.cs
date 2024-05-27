using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Q.DTOs;

namespace Q.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int CorrectAnswerId { get; set; }
        
        [Required]
        public int QuizId { get; set; } // Foreign key to Quiz
        public Quiz Quiz { get; set; } // Navigation property

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }


public static class QuestionEndpoints
{
	public static void MapQuestionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Question").WithTags(nameof(Question));

        group.MapGet("/", async (QuizDbContext db) =>
        {
            var questions = await db.Questions.Include(q => q.Answers).ToListAsync();

            var questionsDTOs = questions.Select(q => new QuestionDTO
            {
                Id = q.Id,
                Text = q.Text,
                Answers = q.Answers.Select(a => new AnswerDTO
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            }).ToList();

            return questionsDTOs;
        })
        .WithName("GetAllQuestions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<QuestionDTO>, NotFound>> (int id, QuizDbContext db) =>
        {
            var question = await db.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(model => model.Id == id);

            if (question != null)
            {
                var questionDTO = new QuestionDTO
                {
                    Id = question.Id,
                    Text = question.Text,
                    Answers = question.Answers.Select(a => new AnswerDTO
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                };

                return TypedResults.Ok(questionDTO);
            }
            else
            {
                return TypedResults.NotFound();
            }
        })
        .WithName("GetQuestionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Question question, QuizDbContext db) =>
        {
            var affected = await db.Questions
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, question.Id)
                  .SetProperty(m => m.Text, question.Text)
                  .SetProperty(m => m.CorrectAnswerId, question.CorrectAnswerId)
                  .SetProperty(m => m.QuizId, question.QuizId)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateQuestion")
        .WithOpenApi();

        group.MapPost("/", async (Question question, QuizDbContext db) =>
        {
            db.Questions.Add(question);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Question/{question.Id}",question);
        })
        .WithName("CreateQuestion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, QuizDbContext db) =>
        {
            var affected = await db.Questions
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteQuestion")
        .WithOpenApi();
    }
}}