using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace EmojiQuiz;
static class Db
{
    private static readonly Random rng=new Random();
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
public static List<string> GetCategories()
{
using var ctx=new QuizContext();
return ctx.Questions.Select(q=>q.Category)
.Distinct()
.OrderBy(c=>c)
.ToList();
} 

public static bool Exists(string answer)
{
using var ctx = new QuizContext();
return ctx.Questions.Any(q=>q.Answer.Trim().ToLower()==answer.Trim().ToLower());
}

    public static Question? GetRandom(string category = "")
{
    using var ctx = new QuizContext();
    var q = ctx.Questions.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(category))
    {
        var c=category.Trim().ToLower();
        q = q.Where(x => x.Category != null && x.Category.Trim().ToLower()==c);
    }
    return q.AsEnumerable()
.OrderBy(x=>rng.Next())
.FirstOrDefault();
}

    public static List<string> GetWrongAnswers(string correct, int count, string category = "")
    {
        using var ctx = new QuizContext();
        var q=ctx.Questions.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(category)&& category !="Все")
        {
        
            var c =category.Trim().ToLower();
q=q.Where(x=>x.Category!=null && x.Category.Trim().ToLower()==c);
        }

        return q.Select(x => x.Answer)
.Distinct()
.Where(a=>a != null && a.Trim().ToLower() != correct.Trim().ToLower())
.OrderBy(x=>rng.Next())
.Take(count)
.ToList();
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
