using System.ComponentModel.DataAnnotations;

namespace Q.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}