using BlazorTailwindApp.Models.Entities;

namespace BlazorTailwindApp.Services;

public class SurveySessionState
{
    public int CurrentStep { get; set; } = 1;
    public List<ResponseAnswer> Answers { get; set; } = [];

    public void SetAnswer(int questionId, int? optionId = null, int? matrixRowId = null,
        int? matrixColumnId = null, int? numericValue = null, string? textValue = null)
    {
        // For matrix questions, match on questionId + matrixRowId
        if (matrixRowId.HasValue)
        {
            var existing = Answers.FirstOrDefault(a =>
                a.QuestionId == questionId && a.MatrixRowId == matrixRowId);
            if (existing != null)
            {
                existing.MatrixColumnId = matrixColumnId;
                existing.NumericValue = numericValue;
                existing.TextValue = textValue;
                return;
            }
        }
        // For single choice / NPS / free text, match on questionId only (no matrixRowId)
        else if (optionId.HasValue || numericValue.HasValue || textValue != null)
        {
            var existing = Answers.FirstOrDefault(a =>
                a.QuestionId == questionId && !a.MatrixRowId.HasValue && a.SelectedOptionId == null);
            if (existing != null && !optionId.HasValue)
            {
                existing.NumericValue = numericValue;
                existing.TextValue = textValue;
                return;
            }
            // For single choice, replace existing
            if (optionId.HasValue)
            {
                Answers.RemoveAll(a => a.QuestionId == questionId && !a.MatrixRowId.HasValue);
            }
        }

        Answers.Add(new ResponseAnswer
        {
            QuestionId = questionId,
            SelectedOptionId = optionId,
            MatrixRowId = matrixRowId,
            MatrixColumnId = matrixColumnId,
            NumericValue = numericValue,
            TextValue = textValue
        });
    }

    public void SetMultipleChoiceAnswers(int questionId, List<int> selectedOptionIds, string? otherText = null)
    {
        Answers.RemoveAll(a => a.QuestionId == questionId);
        foreach (var optionId in selectedOptionIds)
        {
            Answers.Add(new ResponseAnswer
            {
                QuestionId = questionId,
                SelectedOptionId = optionId
            });
        }
        if (!string.IsNullOrWhiteSpace(otherText))
        {
            Answers.Add(new ResponseAnswer
            {
                QuestionId = questionId,
                TextValue = otherText
            });
        }
    }

    public List<ResponseAnswer> GetAnswersForQuestion(int questionId)
    {
        return Answers.Where(a => a.QuestionId == questionId).ToList();
    }

    public void Reset()
    {
        CurrentStep = 1;
        Answers.Clear();
    }
}
