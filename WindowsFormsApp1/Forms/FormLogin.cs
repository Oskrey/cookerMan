using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Поварёнок
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        private string text = String.Empty;

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        int w = 3;
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (w > 0)
            {
                if (textBoxCaptcha.Text == text)
                {

                    string login = textBoxLogin.Text;
                    string password = textBoxPassword.Text;
                    SqlCommand command = DBhelper.connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select * from [User] Where [UserLogin] = @login and [UserPassword] = @password";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        DBhelper.id = (int)reader["UserID"];
                        DBhelper.idRole = (int)reader["UserRole"];
                        reader.Close();
                        Hide();
                        FormProduct fp = new FormProduct(DBhelper.idRole);
                        fp.ShowDialog();
                        Show();
                       
                    }
                    else
                    {
                        MessageBox.Show("Логин и/или пароль введены неверно. Осталось попыток:" + w);
                        reader.Close();
                        w--;
                    }
                }
                else
                {
                    MessageBox.Show("Капча введена неверно");
                    pictureBoxCaptcha.Image = CreateImage(pictureBoxCaptcha.Width, pictureBoxCaptcha.Height);
                    textBoxCaptcha.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы заблокированы. Попробуйте снова через 5 минут.");
                tableLayoutPanelMain.Hide();
                timerBlock.Start();
            }

        }
        /// <summary>
        /// Генерация капчи
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        private Bitmap CreateImage(int Width, int Height) 
        {
            Random rnd = new Random();

            Bitmap result = new Bitmap(Width, Height);

            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);

            Brush[] colors =
            {
                 Brushes.Black,
                 Brushes.Red,
                 Brushes.RoyalBlue,
                 Brushes.Green
            };

            Graphics g = Graphics.FromImage((Image)result);

            g.Clear(Color.Gray);

            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            g.DrawString(text,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));

            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);
            textBoxCaptcha.Text = text;
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBoxCaptcha.Image = CreateImage(pictureBoxCaptcha.Width, pictureBoxCaptcha.Height);

        }

        private void timerBlock_Tick(object sender, EventArgs e)
        {
            tableLayoutPanelMain.Show();
            timerBlock.Stop();
        }
    }
}
