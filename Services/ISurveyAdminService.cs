using BlazorTailwindApp.Models.Entities;

namespace BlazorTailwindApp.Services;

public interface ISurveyAdminService
{
    // Survey CRUD
    Task<List<Survey>> GetAllSurveysAsync();
    Task<Survey?> GetSurveyWithQuestionsAsync(int surveyId);
    Task<Survey> CreateSurveyAsync(Survey survey);
    Task UpdateSurveyAsync(Survey survey);
    Task DeleteSurveyAsync(int surveyId);

    // Question CRUD
    Task<Question?> GetQuestionAsync(int questionId);
    Task<Question> CreateQuestionAsync(Question question);
    Task UpdateQuestionAsync(Question question);
    Task DeleteQuestionAsync(int questionId);

    // Options
    Task AddOptionAsync(QuestionOption option);
    Task UpdateOptionAsync(QuestionOption option);
    Task DeleteOptionAsync(int optionId);

    // Matrix rows/columns
    Task AddMatrixRowAsync(MatrixRow row);
    Task UpdateMatrixRowAsync(MatrixRow row);
    Task DeleteMatrixRowAsync(int rowId);
    Task AddMatrixColumnAsync(MatrixColumn column);
    Task UpdateMatrixColumnAsync(MatrixColumn column);
    Task DeleteMatrixColumnAsync(int columnId);

    // Responses
    Task<List<SurveyResponse>> GetResponsesAsync(int surveyId);
    Task<SurveyResponse?> GetResponseDetailAsync(int responseId);
}
