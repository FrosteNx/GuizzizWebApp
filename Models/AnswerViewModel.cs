using System.ComponentModel.DataAnnotations;

namespace Q.Models
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public int QuizId { get; set; }  // Ensure this property is included
    }
}