﻿namespace Q.Models
{
    public class QuizResult
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime TakenOn { get; set; }

        public Quiz Quiz { get; set; }
        public User User { get; set; }
    }
}
