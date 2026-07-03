namespace EmojiQuiz;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
try
{
        Db.EnsureCreated();
string path =Path.Combine(AppContext.BaseDirectory,"movies_ru_emoji.tsv");
        Db.SeedFromFile(path);
}
catch(Exception ex)
{
MessageBox.Show($"Не удалось запустить базу данных:{ex.Message}","Ошибка запуска");
}
        Application.Run(new MainForm());
    }
}