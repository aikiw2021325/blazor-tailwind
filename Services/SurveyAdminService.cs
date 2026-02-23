using BlazorTailwindApp.Data;
using BlazorTailwindApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorTailwindApp.Services;

public class SurveyAdminService : ISurveyAdminService
{
    private readonly SurveyDbContext _db;

    public SurveyAdminService(SurveyDbContext db)
    {
        _db = db;
    }

    // Survey CRUD
    public async Task<List<Survey>> GetAllSurveysAsync()
    {
        return await _db.Surveys
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Survey?> GetSurveyWithQuestionsAsync(int surveyId)
    {
        return await _db.Surveys
            .Include(s => s.Questions.OrderBy(q => q.Step).ThenBy(q => q.DisplayOrder))
                .ThenInclude(q => q.Options.OrderBy(o => o.DisplayOrder))
            .Include(s => s.Questions)
                .ThenInclude(q => q.MatrixRows.OrderBy(r => r.DisplayOrder))
            .Include(s => s.Questions)
                .ThenInclude(q => q.MatrixColumns.OrderBy(c => c.DisplayOrder))
            .FirstOrDefaultAsync(s => s.Id == surveyId);
    }

    public async Task<Survey> CreateSurveyAsync(Survey survey)
    {
        _db.Surveys.Add(survey);
        await _db.SaveChangesAsync();
        return survey;
    }

    public async Task UpdateSurveyAsync(Survey survey)
    {
        _db.Surveys.Update(survey);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteSurveyAsync(int surveyId)
    {
        var survey = await _db.Surveys.FindAsync(surveyId);
        if (survey != null)
        {
            _db.Surveys.Remove(survey);
            await _db.SaveChangesAsync();
        }
    }

    // Question CRUD
    public async Task<Question?> GetQuestionAsync(int questionId)
    {
        return await _db.Questions
            .Include(q => q.Options.OrderBy(o => o.DisplayOrder))
            .Include(q => q.MatrixRows.OrderBy(r => r.DisplayOrder))
            .Include(q => q.MatrixColumns.OrderBy(c => c.DisplayOrder))
            .FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task<Question> CreateQuestionAsync(Question question)
    {
        _db.Questions.Add(question);
        await _db.SaveChangesAsync();
        return question;
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        _db.Questions.Update(question);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _db.Questions.FindAsync(questionId);
        if (question != null)
        {
            _db.Questions.Remove(question);
            await _db.SaveChangesAsync();
        }
    }

    // Options
    public async Task AddOptionAsync(QuestionOption option)
    {
        _db.QuestionOptions.Add(option);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateOptionAsync(QuestionOption option)
    {
        _db.QuestionOptions.Update(option);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteOptionAsync(int optionId)
    {
        var option = await _db.QuestionOptions.FindAsync(optionId);
        if (option != null)
        {
            _db.QuestionOptions.Remove(option);
            await _db.SaveChangesAsync();
        }
    }

    // Matrix rows
    public async Task AddMatrixRowAsync(MatrixRow row)
    {
        _db.MatrixRows.Add(row);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateMatrixRowAsync(MatrixRow row)
    {
        _db.MatrixRows.Update(row);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteMatrixRowAsync(int rowId)
    {
        var row = await _db.MatrixRows.FindAsync(rowId);
        if (row != null)
        {
            _db.MatrixRows.Remove(row);
            await _db.SaveChangesAsync();
        }
    }

    // Matrix columns
    public async Task AddMatrixColumnAsync(MatrixColumn column)
    {
        _db.MatrixColumns.Add(column);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateMatrixColumnAsync(MatrixColumn column)
    {
        _db.MatrixColumns.Update(column);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteMatrixColumnAsync(int columnId)
    {
        var column = await _db.MatrixColumns.FindAsync(columnId);
        if (column != null)
        {
            _db.MatrixColumns.Remove(column);
            await _db.SaveChangesAsync();
        }
    }

    // Responses
    public async Task<List<SurveyResponse>> GetResponsesAsync(int surveyId)
    {
        return await _db.SurveyResponses
            .Where(r => r.SurveyId == surveyId)
            .OrderByDescending(r => r.SubmittedAt)
            .ToListAsync();
    }

    public async Task<SurveyResponse?> GetResponseDetailAsync(int responseId)
    {
        return await _db.SurveyResponses
            .Include(r => r.Answers)
                .ThenInclude(a => a.Question)
            .Include(r => r.Answers)
                .ThenInclude(a => a.SelectedOption)
            .Include(r => r.Answers)
                .ThenInclude(a => a.MatrixRow)
            .Include(r => r.Answers)
                .ThenInclude(a => a.MatrixColumn)
            .FirstOrDefaultAsync(r => r.Id == responseId);
    }
}
