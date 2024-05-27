using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Q.Models;

public class QuizDbContext : IdentityDbContext<User>
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<QuizResult> QuizResults { get; set; }

    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
    {
    }
}