using System.ComponentModel.DataAnnotations;

namespace Q.Models
{
    public class AnswerViewModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public int QuestionId { get; set; }
    }
}