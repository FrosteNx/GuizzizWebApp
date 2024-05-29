namespace Q.DTOs
{
    public class QuizDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
    }
}