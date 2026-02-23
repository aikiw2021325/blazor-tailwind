using BlazorTailwindApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorTailwindApp.Data;

public class SurveyDbContext : DbContext
{
    public SurveyDbContext(DbContextOptions<SurveyDbContext> options) : base(options) { }

    public DbSet<Survey> Surveys => Set<Survey>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();
    public DbSet<MatrixRow> MatrixRows => Set<MatrixRow>();
    public DbSet<MatrixColumn> MatrixColumns => Set<MatrixColumn>();
    public DbSet<SurveyResponse> SurveyResponses => Set<SurveyResponse>();
    public DbSet<ResponseAnswer> ResponseAnswers => Set<ResponseAnswer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Survey>(e =>
        {
            e.HasMany(s => s.Questions)
                .WithOne(q => q.Survey)
                .HasForeignKey(q => q.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(s => s.Responses)
                .WithOne(r => r.Survey)
                .HasForeignKey(r => r.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Question>(e =>
        {
            e.HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(q => q.MatrixRows)
                .WithOne(r => r.Question)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(q => q.MatrixColumns)
                .WithOne(c => c.Question)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(q => q.QuestionType)
                .HasConversion<string>();
        });

        modelBuilder.Entity<SurveyResponse>(e =>
        {
            e.HasMany(r => r.Answers)
                .WithOne(a => a.SurveyResponse)
                .HasForeignKey(a => a.SurveyResponseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ResponseAnswer>(e =>
        {
            e.HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(a => a.SelectedOption)
                .WithMany()
                .HasForeignKey(a => a.SelectedOptionId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(a => a.MatrixRow)
                .WithMany()
                .HasForeignKey(a => a.MatrixRowId)
                .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(a => a.MatrixColumn)
                .WithMany()
                .HasForeignKey(a => a.MatrixColumnId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
