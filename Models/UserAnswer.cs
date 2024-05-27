namespace Q.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string UserId { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime AnsweredOn { get; set; }

        public Quiz Quiz { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
        public User User { get; set; }
    }
}
