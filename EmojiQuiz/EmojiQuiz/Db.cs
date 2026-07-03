using Microsoft.EntityFrameworkCore;
namespace EmojiQuiz;
static class Db
{
    static readonly Random rng=new();
    public static void EnsureCreated()
    {
        using var ctx = new QuizContext();
        ctx.Database.EnsureCreated();
    }

    public static int Count()
    {
        using var ctx = new QuizContext();
        return ctx.Questions.Count();
    }

    public static void Add(string emoji, string answer, string category)
    {
        using var ctx = new QuizContext();
        ctx.Questions.Add(new Question { Emoji = emoji, Answer = answer, Category = category ?? "" });
        ctx.SaveChanges();
    }

    public static Question? GetRandom(string category = "")
{
    using var ctx = new QuizContext();
    var all=ctx.Questions.ToList();
    var q = all.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(category))
    {
        
        q = q.Where(x => x.Category != null && x.Category.Trim().ToLower()==category.Trim().ToLower());
    }
    var list =q.ToList();
    if (list.Count == 0) return null;
    return list[rng.Next(list.Count)];
}

    public static List<string> GetWrongAnswers(string correct, int count, string category = "")
    {
        using var ctx = new QuizContext();
        var all=ctx.Questions.ToList();
        var q = all.Where(x => x.Answer != correct);
        if (!string.IsNullOrWhiteSpace(category))
        {
        
            q = q.Where(x => x.Category != null && x.Category.Trim().ToLower()==category.Trim().ToLower());
        }

        return q.OrderBy(x => Guid.NewGuid()).Select(x => x.Answer).Take(count).ToList();
    }

    public static void SeedFromFile(string path)
    {
        if (Count() > 0) return;
        using var ctx = new QuizContext();
        foreach (var line in File.ReadLines(path).Skip(1))
        {
            var p = line.Split('\t');
            if (p.Length < 4)
            {
                Console.WriteLine($"only {p.Length} el {line}");
                continue;
            }
            
            ctx.Questions.Add(new Question { Emoji = p[2], Answer = p[1], Category = p[3].Trim() });
        }
        ctx.SaveChanges();
    }
}
