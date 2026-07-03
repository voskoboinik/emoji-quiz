using System.Windows.Forms;
using System.Drawing;

namespace EmojiQuiz;

public partial class AdminForm : Form
{
    TextBox textEmoji, textAnswer, textCategory;

    public AdminForm()
    {
        Text = "Администратор";
        Width = 420;
        Height = 380;
        BackColor = Color.FromArgb(30, 30, 50);

        var lbl1 = new Label(); lbl1.Text = "Эмодзи:"; lbl1.Left = 40; lbl1.Top = 30; lbl1.Width = 300; lbl1.ForeColor = Color.White;
        textEmoji = new TextBox(); textEmoji.Left = 40; textEmoji.Top = 55; textEmoji.Width = 320; textEmoji.BackColor = Color.FromArgb(50, 50, 80); textEmoji.ForeColor = Color.White;

        var lbl2 = new Label(); lbl2.Text = "Ответ (русское название):"; lbl2.Left = 40; lbl2.Top = 90; lbl2.Width = 300; lbl2.ForeColor = Color.White;
        textAnswer = new TextBox(); textAnswer.Left = 40; textAnswer.Top = 115; textAnswer.Width = 320; textAnswer.BackColor = Color.FromArgb(50, 50, 80); textAnswer.ForeColor = Color.White;

        var lbl3 = new Label(); lbl3.Text = "Категория (Фильмы / Мультфильмы / Сериалы):"; lbl3.Left = 40; lbl3.Top = 150; lbl3.Width = 340; lbl3.ForeColor = Color.White;
        textCategory = new TextBox(); textCategory.Left = 40; textCategory.Top = 175; textCategory.Width = 320; textCategory.BackColor = Color.FromArgb(50, 50, 80); textCategory.ForeColor = Color.White;

        var buttonAdd = new Button();
        buttonAdd.Text = "Добавить";
        buttonAdd.Left = 40;
        buttonAdd.Top = 230;
        buttonAdd.Width = 320;
        buttonAdd.Height = 45;
        buttonAdd.BackColor = Color.FromArgb(40, 160, 80);
        buttonAdd.ForeColor = Color.White;
        buttonAdd.FlatStyle = FlatStyle.Flat;
        buttonAdd.Click += ButtonAdd_Click;

        Controls.Add(lbl1); Controls.Add(textEmoji);
        Controls.Add(lbl2); Controls.Add(textAnswer);
        Controls.Add(lbl3); Controls.Add(textCategory);
        Controls.Add(buttonAdd);
    }

    void ButtonAdd_Click(object sender, EventArgs e)
    {
        string emoji  = textEmoji.Text.Trim();
        string answer = textAnswer.Text.Trim();
        string cat    = textCategory.Text.Trim();

        if (string.IsNullOrWhiteSpace(emoji) || string.IsNullOrWhiteSpace(answer) || string.IsNullOrWhiteSpace(cat))
        {
            MessageBox.Show("Заполните все поля.");
            return;
        }
        try
       {
        Db.Add(emoji, answer, cat);
        MessageBox.Show("Добавлено!");
        textEmoji.Clear();
        textAnswer.Clear();
        textCategory.Clear();
        }
        catch(Exception ex)
{
 MessageBox.Show("Произошла ошибка при сохранении:{ex.Message}", "Ошибка базы данных");
}
    }
}