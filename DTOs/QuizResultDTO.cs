using Q.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Q.Data;

namespace Q.DTOs
{
    public class QuizResultDTO
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public float score { get; set; }    
        public DateTime TakenOn { get; set; }
        public ICollection<UserAnswerDTO> UserAnswers { get; set; } = new List<UserAnswerDTO>();
    }


}