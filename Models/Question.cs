using System.ComponentModel.DataAnnotations;

namespace Q.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int CorrectAnswerId { get; set; }

        [Required]
        public int QuizId { get; set; } 
        public Quiz Quiz { get; set; } 

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}