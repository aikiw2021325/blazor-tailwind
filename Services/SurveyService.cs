using BlazorTailwindApp.Data;
using BlazorTailwindApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorTailwindApp.Services;

public class SurveyService : ISurveyService
{
    private readonly SurveyDbContext _db;

    public SurveyService(SurveyDbContext db)
    {
        _db = db;
    }

    public async Task<Survey?> GetSurveyAsync(int surveyId)
    {
        return await _db.Surveys
            .Include(s => s.Questions.OrderBy(q => q.Step).ThenBy(q => q.DisplayOrder))
                .ThenInclude(q => q.Options.OrderBy(o => o.DisplayOrder))
            .Include(s => s.Questions)
                .ThenInclude(q => q.MatrixRows.OrderBy(r => r.DisplayOrder))
            .Include(s => s.Questions)
                .ThenInclude(q => q.MatrixColumns.OrderBy(c => c.DisplayOrder))
            .FirstOrDefaultAsync(s => s.Id == surveyId && s.IsActive);
    }

    public async Task<List<Question>> GetQuestionsForStepAsync(int surveyId, int step)
    {
        return await _db.Questions
            .Where(q => q.SurveyId == surveyId && q.Step == step)
            .OrderBy(q => q.DisplayOrder)
            .Include(q => q.Options.OrderBy(o => o.DisplayOrder))
            .Include(q => q.MatrixRows.OrderBy(r => r.DisplayOrder))
            .Include(q => q.MatrixColumns.OrderBy(c => c.DisplayOrder))
            .ToListAsync();
    }

    public async Task<int> GetTotalStepsAsync(int surveyId)
    {
        var maxStep = await _db.Questions
            .Where(q => q.SurveyId == surveyId)
            .MaxAsync(q => (int?)q.Step);
        return maxStep ?? 0;
    }

    public async Task SubmitResponseAsync(int surveyId, List<ResponseAnswer> answers, string? respondentId = null)
    {
        var response = new SurveyResponse
        {
            SurveyId = surveyId,
            RespondentIdentifier = respondentId,
            SubmittedAt = DateTime.UtcNow,
            Answers = answers
        };

        _db.SurveyResponses.Add(response);
        await _db.SaveChangesAsync();
    }
}
