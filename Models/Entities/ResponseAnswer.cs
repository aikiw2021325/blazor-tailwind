namespace BlazorTailwindApp.Models.Entities;

public class ResponseAnswer
{
    public int Id { get; set; }
    public int SurveyResponseId { get; set; }
    public int QuestionId { get; set; }
    public int? SelectedOptionId { get; set; }
    public int? MatrixRowId { get; set; }
    public int? MatrixColumnId { get; set; }
    public int? NumericValue { get; set; }
    public string? TextValue { get; set; }

    public SurveyResponse SurveyResponse { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public QuestionOption? SelectedOption { get; set; }
    public MatrixRow? MatrixRow { get; set; }
    public MatrixColumn? MatrixColumn { get; set; }
}
