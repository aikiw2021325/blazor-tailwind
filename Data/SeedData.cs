using BlazorTailwindApp.Models.Entities;
using BlazorTailwindApp.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BlazorTailwindApp.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SurveyDbContext>();

        await db.Database.MigrateAsync();

        if (await db.Surveys.AnyAsync()) return;

        var survey = new Survey
        {
            Title = "イベントアンケート",
            Description = "本日はイベントにご参加いただきありがとうございます。今後の改善のため、以下のアンケートにご協力ください。",
            IsActive = true,
            Questions =
            [
                // Step 1: Q1 - 単一選択
                new Question
                {
                    QuestionType = QuestionType.SingleChoice,
                    Text = "Q1. 本イベントをどこで知りましたか？",
                    Step = 1,
                    DisplayOrder = 1,
                    IsRequired = true,
                    Options =
                    [
                        new QuestionOption { Text = "会社からの案内", DisplayOrder = 1 },
                        new QuestionOption { Text = "メールマガジン", DisplayOrder = 2 },
                        new QuestionOption { Text = "SNS（Twitter / Facebook等）", DisplayOrder = 3 },
                        new QuestionOption { Text = "知人の紹介", DisplayOrder = 4 },
                        new QuestionOption { Text = "Webサイト", DisplayOrder = 5 },
                    ]
                },
                // Step 2: Q2 - 複数選択
                new Question
                {
                    QuestionType = QuestionType.MultipleChoice,
                    Text = "Q2. 本イベントに参加した目的をお選びください（複数選択可）",
                    Step = 2,
                    DisplayOrder = 1,
                    IsRequired = true,
                    AllowOtherOption = true,
                    Options =
                    [
                        new QuestionOption { Text = "最新技術の情報収集", DisplayOrder = 1 },
                        new QuestionOption { Text = "製品・サービスの比較検討", DisplayOrder = 2 },
                        new QuestionOption { Text = "業界動向の把握", DisplayOrder = 3 },
                        new QuestionOption { Text = "ネットワーキング", DisplayOrder = 4 },
                        new QuestionOption { Text = "スキルアップ・学習", DisplayOrder = 5 },
                    ]
                },
                // Step 3: Q3 - マトリクス評価
                new Question
                {
                    QuestionType = QuestionType.MatrixRating,
                    Text = "Q3. 各セッションの満足度を教えてください",
                    Step = 3,
                    DisplayOrder = 1,
                    IsRequired = true,
                    MatrixRows =
                    [
                        new MatrixRow { Text = "基調講演", DisplayOrder = 1 },
                        new MatrixRow { Text = "テクニカルセッションA", DisplayOrder = 2 },
                        new MatrixRow { Text = "テクニカルセッションB", DisplayOrder = 3 },
                        new MatrixRow { Text = "パネルディスカッション", DisplayOrder = 4 },
                        new MatrixRow { Text = "ハンズオンワークショップ", DisplayOrder = 5 },
                    ],
                    MatrixColumns =
                    [
                        new MatrixColumn { Text = "満足", DisplayOrder = 1 },
                        new MatrixColumn { Text = "やや満足", DisplayOrder = 2 },
                        new MatrixColumn { Text = "普通", DisplayOrder = 3 },
                        new MatrixColumn { Text = "やや不満", DisplayOrder = 4 },
                        new MatrixColumn { Text = "不満", DisplayOrder = 5 },
                    ]
                },
                // Step 4: Q4 - NPS
                new Question
                {
                    QuestionType = QuestionType.NpsScale,
                    Text = "Q4. 本イベントを友人や同僚に薦める可能性はどのくらいですか？",
                    Step = 4,
                    DisplayOrder = 1,
                    IsRequired = true,
                    NpsLowLabel = "全く薦めない",
                    NpsHighLabel = "強く薦める"
                },
                // Step 5: Q5 - ブース評価
                new Question
                {
                    QuestionType = QuestionType.BoothEvaluation,
                    Text = "Q5. 訪問したブースの評価をお願いします",
                    Step = 5,
                    DisplayOrder = 1,
                    IsRequired = false,
                    MatrixRows =
                    [
                        new MatrixRow { Text = "企業Aブース", DisplayOrder = 1 },
                        new MatrixRow { Text = "企業Bブース", DisplayOrder = 2 },
                        new MatrixRow { Text = "企業Cブース", DisplayOrder = 3 },
                    ],
                    MatrixColumns =
                    [
                        new MatrixColumn { Text = "参考になった", DisplayOrder = 1 },
                        new MatrixColumn { Text = "やや参考になった", DisplayOrder = 2 },
                        new MatrixColumn { Text = "普通", DisplayOrder = 3 },
                        new MatrixColumn { Text = "あまり参考にならなかった", DisplayOrder = 4 },
                        new MatrixColumn { Text = "早急に検討したい", DisplayOrder = 5 },
                    ]
                },
                // Step 6: Q6 - 自由記述
                new Question
                {
                    QuestionType = QuestionType.FreeText,
                    Text = "Q6. ご意見・ご要望がございましたらご自由にお書きください",
                    Step = 6,
                    DisplayOrder = 1,
                    IsRequired = false
                }
            ]
        };

        db.Surveys.Add(survey);
        await db.SaveChangesAsync();
    }
}
