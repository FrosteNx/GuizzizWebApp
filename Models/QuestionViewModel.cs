using System.ComponentModel.DataAnnotations;

namespace Q.Models
{
    public class QuestionViewModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public int QuizId { get; set; } // Foreign key to Quiz
    }
}