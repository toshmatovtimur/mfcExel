using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;


namespace exel_for_mfc
{
    public partial class AdressWindow : Window
    {
        public AdressWindow()
        {
            InitializeComponent();
            name.Focus();
            StartAdress();
        }

        public AdressWindow(ref string str)
        {
            InitializeComponent();
            name.Focus();
            StartAdress();
        }

        //Заполнение ComboboxesAdress
        private async void StartAdress()
        {
            using ExDbContext db = new();
            Xmkr.ItemsSource = await db.PayAmounts.Where(u => u.Mkr != null).Select(s => s.Mkr).ToListAsync();
            ulicaX.ItemsSource = await db.PayAmounts.Where(u => u.Ulica != null).Select(s => s.Ulica).ToListAsync();
            kv.ItemsSource = await db.PayAmounts.Where(u => u.Kvartira != null).Select(s => s.Kvartira).ToListAsync();
        }

        //Добавить адрес
        private void Button_Click(object sender, RoutedEventArgs e)
        {
                TableWindow.temp1 += string.IsNullOrEmpty(nameMKR.Text.Trim()) ? null : $"{Xmkr.Text} {nameMKR.Text},";
                TableWindow.temp1 += string.IsNullOrEmpty(name.Text) ? null : $" {ulicaX.Text} {name.Text},";
                TableWindow.temp1 += string.IsNullOrEmpty(numberDom.Text) ? null : $" {dom.Text} {numberDom.Text},";
                TableWindow.temp1 += string.IsNullOrEmpty(Stroenie.Text) ? null : $"стр. {Stroenie.Text},";
                TableWindow.temp1 += string.IsNullOrEmpty(numCorpus.Text) ? null : $" корп. {numCorpus.Text},";
                TableWindow.temp1 += string.IsNullOrEmpty(kvartira.Text) ? null : $" {kv.Text} {kvartira.Text}";
                Close();
            
        }

        //Очистить поля
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nameMKR.Clear();
            name.Clear();
            numberDom.Clear();
            Stroenie.Clear();
            numCorpus.Clear();
            kvartira.Clear();
            name.Focus();
        }
    }
}
