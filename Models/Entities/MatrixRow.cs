namespace BlazorTailwindApp.Models.Entities;

public class MatrixRow
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsDynamic { get; set; }

    public Question Question { get; set; } = null!;
}
