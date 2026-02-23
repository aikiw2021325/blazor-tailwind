using BlazorTailwindApp.Models.Entities;

namespace BlazorTailwindApp.Services;

public interface ISurveyService
{
    Task<Survey?> GetSurveyAsync(int surveyId);
    Task<List<Question>> GetQuestionsForStepAsync(int surveyId, int step);
    Task<int> GetTotalStepsAsync(int surveyId);
    Task SubmitResponseAsync(int surveyId, List<ResponseAnswer> answers, string? respondentId = null);
}
