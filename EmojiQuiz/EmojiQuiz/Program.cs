namespace EmojiQuiz;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Db.EnsureCreated();
        Db.SeedFromFile("movies_ru_emoji.tsv");
        Application.Run(new MainForm());
    }
}