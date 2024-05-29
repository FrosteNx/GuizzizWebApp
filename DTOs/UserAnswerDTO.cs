namespace Q.DTOs
{
    public class UserAnswerDTO
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string UserId { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredOn { get; set; }
        public int QuizResultId { get; set; }
    }
}
