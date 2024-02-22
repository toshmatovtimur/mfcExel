using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace exel_for_mfc
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        //Войти
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //При нажатии на кнопку Войти
            //Вход
            //Проверка на пустые поля
            //Проверка входных значений
            if (string.IsNullOrWhiteSpace(login_text.Text)
            && string.IsNullOrWhiteSpace(password_text.Password)
            || string.IsNullOrWhiteSpace(login_text.Text)
            || string.IsNullOrWhiteSpace(password_text.Password))
            {
                MessageBox.Show("Не заполнено одно или несколько полей", "Пропущено поле", MessageBoxButton.OK, MessageBoxImage.Stop);
                if (string.IsNullOrWhiteSpace(login_text.Text))
                    login_text.BorderBrush = Brushes.Red;


                if (string.IsNullOrWhiteSpace(password_text.Password))
                    password_text.BorderBrush = Brushes.Red;

            }
            else
            {
                //Если поля не пустые

                int temp = 0;
                using ExDbContext db = new();
                var GetUserLogPass = await db.SolutionTypes.Where(u => u.Id == 2).FirstOrDefaultAsync();

                if (GetUserLogPass.Login == login_text.Text && GetUserLogPass.Passwords == MD5Hash(password_text.Password) && GetUserLogPass.Id == 2)
                {
                    TableWindow table = new();
                    table.Show();
                    temp = 1;
                    Close();
                }

                if (temp == 0)
                {
                    var GetAdminLogPass = await db.SolutionTypes.Where(u => u.Id == 1).FirstOrDefaultAsync();
                    if (GetAdminLogPass.Login == login_text.Text && GetAdminLogPass.Passwords == MD5Hash(password_text.Password) && GetAdminLogPass.Id == 1)
                    {
                        AdminWindow admin = new();
                        admin.Show();
                        temp = 1;
                        Close();
                    }
                }

                if (temp == 0)
                {
                    MessageBox.Show("Повторите попытку", "Неправильный логин или пароль", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        //Метод хэширования вводимого пароля
        private static string MD5Hash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }

        //Метод хорошего старта
        private void Start()
        {
            using (ExDbContext db = new())
            {
                var start = db.SolutionTypes.ToList();
                foreach (var item in start) { }
            }
            login_text.Focus();
        }

        //Методы подсвечивают рамки красным при неправильном вводе
        private void Pa(object sender, MouseEventArgs e)
        {
            password_text.BorderBrush = Brushes.Black;
        }
        private void Bo(object sender, MouseEventArgs e)
        {
            login_text.BorderBrush = Brushes.Black;
        }
    }
}