using System.ComponentModel.DataAnnotations;

namespace Q.Models
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuizId { get; set; }
    }
}