using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Q.DTOs;
using Q.Data;

namespace Q.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();
    }

public static class QuizEndpoints
{
	public static void MapQuizEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Quiz").WithTags(nameof(Quiz));

        group.MapGet("/", async (QuizDbContext db) =>
        {
            var quizzes = await db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .ToListAsync();

            var quizDTOs = quizzes.Select(q => new QuizDTO
            {
                Id = q.Id,
                Title = q.Title,
                Questions = q.Questions.Select(qt => new QuestionDTO
                {
                    Id = qt.Id,
                    Text = qt.Text,
                    Answers = qt.Answers.Select(a => new AnswerDTO
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            }).ToList();

            return quizDTOs;
        })
        .WithName("GetAllQuizzes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<QuizDTO>, NotFound>> (int id, QuizDbContext db) =>
        {
            var quiz = await db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(model => model.Id == id);

            if (quiz != null)
            {
                var quizDTO = new QuizDTO
                {
                    Id = quiz.Id,
                    Title = quiz.Title,
                    Questions = quiz.Questions.Select(qt => new QuestionDTO
                    {
                        Id = qt.Id,
                        Text = qt.Text,
                        Answers = qt.Answers.Select(a => new AnswerDTO
                        {
                            Id = a.Id,
                            Text = a.Text,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    }).ToList()
                };

                return TypedResults.Ok(quizDTO);
            }
            else
            {
                return TypedResults.NotFound();
            }
        })
        .WithName("GetQuizById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Quiz quiz, QuizDbContext db) =>
        {
            var affected = await db.Quizzes
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Id, quiz.Id)
                  .SetProperty(m => m.Title, quiz.Title)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateQuiz")
        .WithOpenApi();

        group.MapPost("/", async (QuizDTO quizDto, QuizDbContext db) =>
        {
            var quiz = new Quiz
            {
                Title = quizDto.Title,
                Questions = quizDto.Questions.Select(q => new Question
                {
                    Text = q.Text,
                    CorrectAnswerId = q.CorrectAnswerId,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            db.Quizzes.Add(quiz);
            await db.SaveChangesAsync();

            var createdQuizDto = new QuizDTO
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Questions = quiz.Questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    Text = q.Text,
                    CorrectAnswerId = q.CorrectAnswerId,
                    Answers = q.Answers.Select(a => new AnswerDTO
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return TypedResults.Created($"/api/Quiz/{quiz.Id}", createdQuizDto);
        })
        .WithName("CreateQuiz")
        .WithOpenApi();

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, QuizDbContext db) =>
        {
            var affected = await db.Quizzes
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteQuiz")
        .WithOpenApi();
    }
}}