using System.IO;
using System;
using Microsoft.EntityFrameworkCore;

namespace EmojiQuiz;
class QuizContext : DbContext
{
    public DbSet<Question> Questions => Set<Question>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string projectFolder=AppDomain.CurrentDomain.BaseDirectory;
        options.UseSqlite($"Data Source={Path.Combine(projectFolder,"quiz.db")}");
    }
}

