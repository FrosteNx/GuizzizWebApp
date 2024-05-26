using System.ComponentModel.DataAnnotations;

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
}