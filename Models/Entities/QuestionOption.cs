namespace BlazorTailwindApp.Models.Entities;

public class QuestionOption
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public Question Question { get; set; } = null!;
}
