using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace EmojiQuiz;

public partial class MainForm : Form
{
    public MainForm()
    {
        Text = "Эмодзи-викторина";
        Width = 400;
        Height = 380;
        BackColor = Color.FromArgb(30, 30, 60);

        var title = new Label();
        title.Text = " Угадай по эмодзи";
        title.Font = new Font("Segoe UI Emoji", 16, FontStyle.Bold);
        title.ForeColor = Color.White;
        title.Width = 360;
        title.Height = 40;
        title.Left = 20;
        title.Top = 20;
        Controls.Add(title);

        var labelCat = new Label();
        labelCat.Text = "Категория:";
        labelCat.ForeColor = Color.White;
        labelCat.Left = 40;
        labelCat.Top = 70;
        labelCat.Width = 150;
        Controls.Add(labelCat);

        var comboCategory = new ComboBox();
        comboCategory.Top = 100;
        comboCategory.Width = 320;
        comboCategory.Left = 20;
        comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
comboCategory.Items.Add("Все");
        comboCategory.Items.AddRange(Db.GetCategories().ToArray());
        comboCategory.SelectedIndex = 0;
        Controls.Add(comboCategory);

        var buttonPlay = new Button();
        buttonPlay.Text = "Играть";
        buttonPlay.Width = 300;
        buttonPlay.Height = 50;
        buttonPlay.Left = 40;
        buttonPlay.Top = 160;
        buttonPlay.BackColor = Color.FromArgb(80, 80, 200);
        buttonPlay.ForeColor = Color.White;
        buttonPlay.FlatStyle = FlatStyle.Flat;
        buttonPlay.Click += (s, e) =>
        {
            string cat = comboCategory.SelectedItem?.ToString() == "Все" ? "" : comboCategory.SelectedItem?.ToString() ?? "";
            new GameForm(cat).Show();
        };
        Controls.Add(buttonPlay);

        var buttonAdmin = new Button();
        buttonAdmin.Text = "Администратор";
        buttonAdmin.Width = 300;
        buttonAdmin.Height = 50;
        buttonAdmin.Left = 40;
        buttonAdmin.Top = 230;
        buttonAdmin.BackColor = Color.FromArgb(60, 60, 60);
        buttonAdmin.ForeColor = Color.White;
        buttonAdmin.FlatStyle = FlatStyle.Flat;
        buttonAdmin.Click += (s, e) => new AdminForm().Show();
        Controls.Add(buttonAdmin);
     
        
        
    }
}