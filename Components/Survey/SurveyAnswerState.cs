using BlazorTailwindApp.Components.Survey.Questions;
using BlazorTailwindApp.Models.Entities;

namespace BlazorTailwindApp.Components.Survey;

public class SurveyAnswerState
{
    private readonly Dictionary<int, int?> _singleChoices = new();
    private readonly Dictionary<int, List<int>> _multipleChoices = new();
    private readonly Dictionary<int, string?> _otherTexts = new();
    private readonly Dictionary<int, Dictionary<int, int>> _matrixSelections = new();
    private readonly Dictionary<int, int?> _npsValues = new();
    private readonly Dictionary<int, string?> _freeTexts = new();
    private readonly Dictionary<int, List<BoothEvaluationQuestion.DynamicRowData>> _dynamicRows = new();

    // Single Choice
    public int? GetSingleChoice(int questionId) =>
        _singleChoices.TryGetValue(questionId, out var v) ? v : null;
    public void SetSingleChoice(int questionId, int? optionId) =>
        _singleChoices[questionId] = optionId;

    // Multiple Choice
    public List<int> GetMultipleChoice(int questionId) =>
        _multipleChoices.TryGetValue(questionId, out var v) ? v : [];
    public void SetMultipleChoice(int questionId, List<int> optionIds) =>
        _multipleChoices[questionId] = optionIds;

    // Other text
    public string? GetOtherText(int questionId) =>
        _otherTexts.TryGetValue(questionId, out var v) ? v : null;
    public void SetOtherText(int questionId, string? text) =>
        _otherTexts[questionId] = text;

    // Matrix
    public Dictionary<int, int> GetMatrixSelections(int questionId) =>
        _matrixSelections.TryGetValue(questionId, out var v) ? v : new();
    public void SetMatrixSelections(int questionId, Dictionary<int, int> selections) =>
        _matrixSelections[questionId] = selections;

    // NPS
    public int? GetNpsValue(int questionId) =>
        _npsValues.TryGetValue(questionId, out var v) ? v : null;
    public void SetNpsValue(int questionId, int? value) =>
        _npsValues[questionId] = value;

    // Free Text
    public string? GetFreeText(int questionId) =>
        _freeTexts.TryGetValue(questionId, out var v) ? v : null;
    public void SetFreeText(int questionId, string? text) =>
        _freeTexts[questionId] = text;

    // Dynamic Rows
    public List<BoothEvaluationQuestion.DynamicRowData> GetDynamicRows(int questionId) =>
        _dynamicRows.TryGetValue(questionId, out var v) ? v : [];
    public void SetDynamicRows(int questionId, List<BoothEvaluationQuestion.DynamicRowData> rows) =>
        _dynamicRows[questionId] = rows;

    public List<ResponseAnswer> ToResponseAnswers()
    {
        var answers = new List<ResponseAnswer>();

        foreach (var (qId, optionId) in _singleChoices)
        {
            if (optionId.HasValue)
                answers.Add(new ResponseAnswer { QuestionId = qId, SelectedOptionId = optionId });
        }

        foreach (var (qId, optionIds) in _multipleChoices)
        {
            foreach (var optionId in optionIds)
                answers.Add(new ResponseAnswer { QuestionId = qId, SelectedOptionId = optionId });

            if (_otherTexts.TryGetValue(qId, out var otherText) && !string.IsNullOrWhiteSpace(otherText))
                answers.Add(new ResponseAnswer { QuestionId = qId, TextValue = otherText });
        }

        foreach (var (qId, selections) in _matrixSelections)
        {
            foreach (var (rowId, colId) in selections)
                answers.Add(new ResponseAnswer { QuestionId = qId, MatrixRowId = rowId, MatrixColumnId = colId });
        }

        // Dynamic rows for booth evaluation
        foreach (var (qId, rows) in _dynamicRows)
        {
            foreach (var row in rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Text) && row.SelectedColumnId.HasValue)
                {
                    answers.Add(new ResponseAnswer
                    {
                        QuestionId = qId,
                        TextValue = row.Text,
                        MatrixColumnId = row.SelectedColumnId
                    });
                }
            }
        }

        foreach (var (qId, value) in _npsValues)
        {
            if (value.HasValue)
                answers.Add(new ResponseAnswer { QuestionId = qId, NumericValue = value });
        }

        foreach (var (qId, text) in _freeTexts)
        {
            if (!string.IsNullOrWhiteSpace(text))
                answers.Add(new ResponseAnswer { QuestionId = qId, TextValue = text });
        }

        return answers;
    }

    public bool ValidateQuestion(Question question)
    {
        if (!question.IsRequired) return true;

        return question.QuestionType switch
        {
            Models.Enums.QuestionType.SingleChoice => GetSingleChoice(question.Id).HasValue,
            Models.Enums.QuestionType.MultipleChoice => GetMultipleChoice(question.Id).Count > 0,
            Models.Enums.QuestionType.MatrixRating =>
                GetMatrixSelections(question.Id).Count == question.MatrixRows.Count,
            Models.Enums.QuestionType.NpsScale => GetNpsValue(question.Id).HasValue,
            Models.Enums.QuestionType.BoothEvaluation =>
                GetMatrixSelections(question.Id).Count == question.MatrixRows.Count,
            Models.Enums.QuestionType.FreeText => !string.IsNullOrWhiteSpace(GetFreeText(question.Id)),
            _ => true
        };
    }
}
