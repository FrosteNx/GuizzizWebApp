namespace Q.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<AnswerDTO> Answers { get; set; } = new List<AnswerDTO>();
    }
}
