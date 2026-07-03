namespace EmojiQuiz;

class Question
{
    public int Id { get; set; }
    public string Emoji { get; set; } = "";
    public string Answer { get; set; } = "";
    public string Category { get; set; } = "";
}
