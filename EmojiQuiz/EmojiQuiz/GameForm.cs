using System.Windows.Forms;
using System.Drawing;

namespace EmojiQuiz;

public partial class GameForm : Form
{
    static readonly Random rng = new();
    Question? current;
    int score = 0;
    int questionNumber = 0;
    int totalQuestions = 10;
    int secondsLeft = 15;
    string category;

    Label labelEmoji, labelScore, labelResult, labelTimer;
    Button button1, button2, button3, button4;
    System.Windows.Forms.Timer timer = new();

    public GameForm(string cat = "")
    {
        category = cat;
        Text = "Игра";
        Width = 500;
        Height = 580;
        BackColor = Color.FromArgb(20, 20, 50);

        labelEmoji = new Label();
        labelEmoji.Font = new Font("Segoe UI Emoji", 48);
        labelEmoji.Width = 420;
        labelEmoji.Height = 80;
        labelEmoji.Left = 30;
        labelEmoji.Top = 10;
        Controls.Add(labelEmoji);
        labelEmoji.BackColor = Color.White;

        labelScore = new Label();
        labelScore.Text = "Счёт: 0";
        labelScore.ForeColor = Color.White;
        labelScore.Width = 200;
        labelScore.Left = 30;
        labelScore.Top = 100;
        Controls.Add(labelScore);

        labelTimer = new Label();
        labelTimer.Text = "⏱ 15";
        labelTimer.ForeColor = Color.Yellow;
        labelTimer.Width = 200;
        labelTimer.Left = 250;
        labelTimer.Top = 100;
        Controls.Add(labelTimer);

        labelResult = new Label();
        labelResult.ForeColor = Color.LightGreen;
        labelResult.Width = 420;
        labelResult.Left = 30;
        labelResult.Top = 130;
        Controls.Add(labelResult);

        button1 = MakeButton(160); 
        button2 = MakeButton(215);
        button3 = MakeButton(270);
        button4 = MakeButton(325);

        button1.Click += (s, e) => CheckAnswer(button1.Text);
        button2.Click += (s, e) => CheckAnswer(button2.Text);
        button3.Click += (s, e) => CheckAnswer(button3.Text);
        button4.Click += (s, e) => CheckAnswer(button4.Text);

        Controls.Add(button1);
        Controls.Add(button2);
        Controls.Add(button3);
        Controls.Add(button4);

        timer.Interval = 1000;
        timer.Tick += Timer_Tick;

        NextQuestion();
    }

    Button MakeButton(int top)
    {
        var b = new Button();
        b.Width = 420;
        b.Left = 30;
        b.Top = top;
        b.Height = 45;
        b.BackColor = Color.FromArgb(60, 60, 120);
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        return b;
    }

    void Timer_Tick(object? sender, EventArgs e)
    {
        secondsLeft--;
        labelTimer.Text = "⏱ " + secondsLeft;
        if (secondsLeft <= 0)
        {
            timer.Stop();
            labelResult.Text = "Время вышло! Это " + current?.Answer;
            labelResult.ForeColor = Color.OrangeRed;
            questionNumber++;
            if (questionNumber >= totalQuestions) { ShowResult(); return; }
            Task.Delay(1500).ContinueWith(_ => Invoke(NextQuestion));
        }
    }

    void NextQuestion()
    {
        current = Db.GetRandom(category);
        if (current == null) { labelEmoji.Text = "База пуста"; return; }

        labelEmoji.Text = current.Emoji;
        labelResult.Text = "";
        secondsLeft = 15;
        labelTimer.Text = "⏱ 15";
        labelTimer.ForeColor = Color.Yellow;
        timer.Start();

        var options = Db.GetWrongAnswers(current.Answer, 3, category);
        options.Add(current.Answer);
        Shuffle(options);

        var buttons = new[] { button1, button2, button3, button4 };
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < options.Count) { buttons[i].Text = options[i]; buttons[i].Visible = true; }
            else { buttons[i].Visible = false; }
        }
    }

    void CheckAnswer(string chosen)
    {
        if (current == null) return;
        timer.Stop();
        if (chosen == current.Answer)
        {
            score++;
            labelResult.Text = " Верно!";
            labelResult.ForeColor = Color.LightGreen;
        }
        else
        {
            labelResult.Text = " Неверно, это " + current.Answer;
            labelResult.ForeColor = Color.OrangeRed;
        }
        labelScore.Text = "Счёт: " + score;
        questionNumber++;
        if (questionNumber >= totalQuestions) { ShowResult(); return; }
        Task.Delay(1500).ContinueWith(_ => Invoke(NextQuestion));
    }

    void ShowResult()
    {
        timer.Stop();
        int percent = (int)((double)score / totalQuestions * 100);
        MessageBox.Show(
            $"Раунд окончен!\n\nПравильных ответов: {score} из {totalQuestions}\nРезультат: {percent}%",
            "Результат",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
        Close();
    }

    void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}