using DocumentFormat.OpenXml.Drawing;
using exel_for_mfc.SupportClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace exel_for_mfc
{
    public partial class StaticWindow : Window
    {
        public DateTime yearCodeBehind = DateTime.Today;
        public StaticWindow()
        {
            InitializeComponent();
            StartapStatic();
        }

        void StartapStatic()
        {
            using ExDbContext db = new();

            //Общее количество выплат
            var AllTimePays = from r in db.Registries.Where(u => u.PayAmountFk != null)
                          join p in db.PayAmounts.Where(u => u.Pay != null) on r.PayAmountFk equals p.Id
                          select new
                          {
                              p.Pay,
                          };

            decimal? allTimeSummPays = 0;

            foreach (var item in AllTimePays)
            {
                allTimeSummPays += item.Pay;
            }


            TotalAmountForAllTime.Text = "Общая сумма выплат за все время: " + allTimeSummPays.ToString();


            YearXaml.Text = yearCodeBehind.Year.ToString();


            //Общее количество сертификатов
            var getCountSert = db.Registries.Where(u => u.SerialAndNumberSert != null && u.DateGetSert.Value.Year == yearCodeBehind.Year).Count();
            Sert.Text = "Сертификаты(общее количество за выбранный год): " + getCountSert.ToString();

            //Размер выплат
            var getNamePays = db.PayAmounts.Where(u => u.Pay != null).ToList();
            List<PayClass> names = new();
            foreach (var item in getNamePays)
            {
                names.Add(new PayClass(item.Id, item.Pay, db.Registries.Where(u => u.PayAmountFk == item.Id && u.DateGetSert.Value.Year == yearCodeBehind.Year).Count()));
            }
            payFilter.ItemsSource = names.ToList();

            //Общее количество выплат
            var AllPays = from r in db.Registries.Where(u => u.PayAmountFk != null && u.DateGetSert.Value.Year == yearCodeBehind.Year)
                          join p in db.PayAmounts.Where(u => u.Pay != null) on r.PayAmountFk equals p.Id
                          select new
                          {
                              p.Pay,
                              r.DateGetSert
                          };

            decimal? allSummPays = 0;

            foreach (var item in AllPays)
            {
                allSummPays += item.Pay;
            }

            payCount.Text = "Общая сумма выплат за год: " + allSummPays.ToString() + " рублей";


            //Решения
            var getNameSoul = db.SolutionTypes.Where(u => u.SolutionName != "").ToList();
            List<SolutionClass> names1 = new();
            foreach (var item in getNameSoul)
            {
                names1.Add(new SolutionClass(item.Id, item.SolutionName, db.Registries.Where(u => u.SolutionFk == item.Id && u.DateGetSert.Value.Year == yearCodeBehind.Year).Count()));
            }
            solFilter.ItemsSource = names1.ToList();

        }

        //Кнопка вправо
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            yearCodeBehind = yearCodeBehind.AddYears(1);
            YearXaml.Text = yearCodeBehind.Year.ToString();
            StartapStatic();
        }

        //Кнопка влево
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            yearCodeBehind = yearCodeBehind.AddYears(-1);
            YearXaml.Text = yearCodeBehind.Year.ToString();
            StartapStatic();
        }

    }
}