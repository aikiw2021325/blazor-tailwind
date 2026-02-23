using BlazorTailwindApp.Models.Enums;

namespace BlazorTailwindApp.Models.Entities;

public class Question
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public QuestionType QuestionType { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Step { get; set; } = 1;
    public int DisplayOrder { get; set; }
    public bool IsRequired { get; set; } = true;
    public bool AllowOtherOption { get; set; }
    public string? NpsLowLabel { get; set; }
    public string? NpsHighLabel { get; set; }

    public Survey Survey { get; set; } = null!;
    public List<QuestionOption> Options { get; set; } = [];
    public List<MatrixRow> MatrixRows { get; set; } = [];
    public List<MatrixColumn> MatrixColumns { get; set; } = [];
}
