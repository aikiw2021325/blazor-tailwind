namespace BlazorTailwindApp.Models.Entities;

public class SurveyResponse
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string? RespondentIdentifier { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public Survey Survey { get; set; } = null!;
    public List<ResponseAnswer> Answers { get; set; } = [];
}
