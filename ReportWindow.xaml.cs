using Microsoft.Win32;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace exel_for_mfc
{
    public partial class ReportWindow : Window
    {
        private bool flag = true;

        private DateTime yearCodeBehind = DateTime.Today;

        private List<string> listMouth = new();

        private List<int> totalByMonth = new();

        private int nextLine = 0;

        private int nextLine1 = 0;
        

        public ReportWindow()
        {
            InitializeComponent();
            TotalAmountForAllTime.Text = yearCodeBehind.Year.ToString();
        }

        //Генерация шаблона для отчета
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Чищу словарь итоговых результатов
            totalByMonth.Clear();

            // Получил список выбранных Месяцев
            listMouth = ListMouth();


            if (listMouth.Count == 1)
                MessageBox.Show("Нужно выбрать хотя бы один месяц для отчета\n Или выбрать все!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);

            else if (listMouth.Count > 1)
            {

                SaveFileDialog saveFile = new()
                {
                    DefaultExt = "xlsx"
                    
                };

                if (saveFile.ShowDialog() == true)
                {
                    new Thread(() => { CreateFile(saveFile.FileName); }) { IsBackground = true }.Start();
                }
            }

        }

        private void CreateFile(string str)
        {
            #region Стили
            //Стиль главного заголовка
            SLStyle titleStyle = new SLStyle();
            titleStyle.Font.FontName = "Arial";
            titleStyle.Font.FontSize = 16;
            titleStyle.Font.Bold = true;
            titleStyle.SetWrapText(true);
            titleStyle.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            titleStyle.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center);

            //Стиль месяца
            SLStyle itemRowHeaderStyle = new SLStyle();
            itemRowHeaderStyle.Font.FontName = "Arial";
            itemRowHeaderStyle.Font.FontSize = 14;
            itemRowHeaderStyle.SetWrapText(true);
            titleStyle.Font.Bold = true;
            itemRowHeaderStyle.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            itemRowHeaderStyle.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center);
            itemRowHeaderStyle.Border.BottomBorder.BorderStyle = itemRowHeaderStyle.Border.TopBorder.BorderStyle = itemRowHeaderStyle.Border.LeftBorder.BorderStyle = itemRowHeaderStyle.Border.RightBorder.BorderStyle = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
            itemRowHeaderStyle.Border.BottomBorder.Color = itemRowHeaderStyle.Border.TopBorder.Color = itemRowHeaderStyle.Border.LeftBorder.Color = itemRowHeaderStyle.Border.RightBorder.Color = System.Drawing.Color.Black;

            //Стиль значения
            SLStyle strokeStyle = new SLStyle();
            strokeStyle.Font.FontName = "Arial";
            strokeStyle.Font.FontSize = 12;
            strokeStyle.Font.Bold = false;
            strokeStyle.SetWrapText(true);
            strokeStyle.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            strokeStyle.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center);

            //Новый стиль
            SLStyle liderStyle = new SLStyle();
            liderStyle.Font.FontName = "Arial";
            liderStyle.Font.FontSize = 13;
            liderStyle.Font.Bold = false;
            liderStyle.Font.Italic = true;
            liderStyle.SetWrapText(true);
            liderStyle.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            liderStyle.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Left);

            //Из них
            SLStyle izNixStyle = new SLStyle();
            izNixStyle.Font.FontName = "Arial";
            izNixStyle.Font.FontSize = 13;
            izNixStyle.Font.Bold = true;
            izNixStyle.Font.Italic = true;
            izNixStyle.SetWrapText(true);
            izNixStyle.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            izNixStyle.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Left);

            //Выплаты
            SLStyle payStyle = new SLStyle();
            payStyle.Font.FontName = "Arial";
            payStyle.Font.FontSize = 13;
            payStyle.Font.Bold = false;
            payStyle.Font.Italic = true;
            payStyle.SetWrapText(true);
            payStyle.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            payStyle.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center);

            //Стиль Итого
            SLStyle totalStyle = new SLStyle();
            totalStyle.Font.FontName = "Arial";
            totalStyle.Font.FontSize = 12;
            totalStyle.Font.Bold = true;
            totalStyle.SetWrapText(true);
            totalStyle.SetVerticalAlignment(DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center);
            totalStyle.SetHorizontalAlignment(DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center);



            #endregion

            if (str != string.Empty)
            {
                try
                {
                    // Создаю документ
                    using SLDocument doc = new();


                    // Генерация колонок в зависимости от выбора Месяцев
                    // Создаю объкт таблицы
                    DataTable dt = new();

                    //Затем в цикле надо задать колонки Месяцев
                    foreach (var item in listMouth)
                    {
                        dt.Columns.Add(item, typeof(string));

                    }

                    // Задать стиль района Главного Заголовка
                    doc.SetColumnWidth(1, 35);
                    doc.SetRowHeight(1, 35);
                    doc.SetCellStyle(1, 1, titleStyle);


                    // Задать стили заголовков месяцев колонок
                    for (int j = 2; j < listMouth.Count + 1; j++)
                    {
                        doc.SetColumnWidth(j, 15);
                        doc.SetCellStyle(1, j, itemRowHeaderStyle);
                    }

                    /////////////////-----  Заполнение районов и их значений ------///////////////////*****
                    ///
                    using ExDbContext db = new();

                    //Задаю стиль заголовка
                    doc.ImportDataTable(1, 1, dt, true);

                    //Запрос на получение списка районов
                    var getMyArea = db.Areas.Where(u => u.HidingArea == 1 && u.AreaName != "").OrderBy(u => u.AreaName).ToList();

                    int i = 2;

                    //Создаю список чисел аналогов Месяцам
                    List<int> analog = IntMouth(listMouth);

                    char[] chars = { 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M'  };
                    int summTotal = 0;
                    int ch = 0;

                    bool dewq = true;
                    //Мне нужно получить количество сертификатов за определенный год и за определенный месяц

                    // Заполнение колонки районами и значениями сразу //Индекс района
                    foreach (var a in analog)
                    {
                        summTotal = 0;
                        foreach (var item in getMyArea)
                        {
                            /* A B C D E F G H I J K L M */

                            if (dewq)
                            {
                                doc.SetCellValue($"A{i}", item.AreaName);
                                doc.SetCellStyle($"A{i}", liderStyle);
                                doc.SetRowHeight(i, 25);
                            }
                           

                            // Получить Id Района
                            var idArea = db.Areas.Where(u => u.AreaName == item.AreaName).FirstOrDefault();


                            // Количество сертов

                            var countSert = from r in db.Registries.Where(u => u.SerialAndNumberSert != null
                                                                            && u.DateGetSert.Value.Year == yearCodeBehind.Year
                                                                            && u.DateGetSert.Value.Month == a)
                                            join ap in db.Applicants.Where(a => a.AreaFk == idArea.Id) on r.ApplicantFk equals ap.Id
                                            select new
                                            {
                                                id = r.Id
                                            };


                            doc.SetCellValue($"{chars[ch]}{i}", countSert.Count());
                            summTotal += countSert.Count();
                            doc.SetCellStyle($"{chars[ch]}{i}", strokeStyle);
                            doc.SetRowHeight(i, 25);

                            i++;
                        }
                        nextLine = i;
                        totalByMonth.Add(summTotal);
                        i = 2;
                        ch++;
                        dewq = false;
                    }


                    //Данные о количестве записать сразу после обработки райнов или во время обработки, окех!

                    /*#############################################################################*/
                    /*********************************   ВЫПЛАТЫ   *********************************/
                    /*#############################################################################*/
                    dewq = true;
                    ch = 0;

                        doc.SetCellValue($"A{nextLine}", "Из них");
                        doc.SetCellStyle($"A{nextLine}", izNixStyle);
                        doc.SetRowHeight(nextLine, 25);

                    

                    nextLine++;


                    //Запрос на получение списка Выплат
                    var getMyPay = db.PayAmounts.Where(u => u.HidingPay == 1 && u.Pay != null).OrderBy(u => u.Pay).ToList();

                    // Из Них
                    i = nextLine;
                    foreach (var a in analog)
                    {
                        foreach (var item in getMyPay)
                        {
                            if (dewq)
                            {
                                doc.SetCellValue($"A{nextLine}", item.Pay.ToString());
                                doc.SetCellStyle($"A{nextLine}", payStyle);
                                doc.SetRowHeight(nextLine, 25);
                            }

                            // Получить Id Выплаты
                            var idPay = db.PayAmounts.Where(u => u.Pay == item.Pay).FirstOrDefault();


                            // Количество сертов по выплатам

                            var countSertPay = from r in db.Registries.Where(u => u.SerialAndNumberSert != null
                                                                            && u.DateGetSert.Value.Year == yearCodeBehind.Year
                                                                            && u.DateGetSert.Value.Month == a
                                                                            && u.PayAmountFk == idPay.Id)
                                            select new
                                            {
                                                id = r.Id
                                            };


                            doc.SetCellValue($"{chars[ch]}{nextLine}", countSertPay.Count());
                            doc.SetCellStyle($"{chars[ch]}{nextLine}", strokeStyle);
                            doc.SetRowHeight(nextLine, 25);


                            nextLine++;

                        }
                        nextLine1 = nextLine;
                        nextLine = i;
                        dewq = false;
                        ch++;
                    }



                    /*#############################################################################*/
                    /*********************************   Льготы   *********************************/
                    /*#############################################################################*/

                    dewq = true;
                    ch = 0;

                    //Из них
                    doc.SetCellValue($"A{nextLine1}", "Из них");
                    doc.SetCellStyle($"A{nextLine1}", izNixStyle);
                    doc.SetRowHeight(nextLine1, 25);



                    nextLine1++;




                    //Запрос на получение списка Льгот
                    var getMyPriv = db.Privileges.Where(u => u.HidingPriv == 1 && u.PrivilegesName != "").OrderBy(u => u.PrivilegesName).ToList();

                    //Чтоб сохранить текущее состояние
                    i = nextLine1;
                    foreach (var a in analog)
                    {
                        foreach (var item in getMyPriv)
                        {
                            if (dewq)
                            {
                                doc.SetCellValue($"A{nextLine1}", item.PrivilegesName);
                                doc.SetCellStyle($"A{nextLine1}", liderStyle);
                                doc.SetRowHeight(nextLine1, 50);
                            }

                            // Получить Id Льготы
                            var idPriv = db.Privileges.Where(u => u.PrivilegesName == item.PrivilegesName).FirstOrDefault();


                            // Количество сертов по льготам

                            var countSertPriv = from r in db.Registries.Where(u => u.SerialAndNumberSert != null
                                                                           && u.DateGetSert.Value.Year == yearCodeBehind.Year
                                                                           && u.DateGetSert.Value.Month == a)
                                            join ap in db.Applicants.Where(a => a.PrivilegesFk == idPriv.Id) on r.ApplicantFk equals ap.Id
                                            select new
                                            {
                                                id = r.Id
                                            };


                            doc.SetCellValue($"{chars[ch]}{nextLine1}", countSertPriv.Count());
                            doc.SetRowHeight(nextLine1, 55);
                            doc.SetCellStyle($"{chars[ch]}{nextLine1}", strokeStyle);
                            
                            


                            nextLine1++;

                        }
                        nextLine = nextLine1;
                        nextLine1 = i;
                        dewq = false;
                        ch++;

                    }


                    /*****************ИТОГО******************/


                    dewq = true;
                    ch = 0;
                    //Из них
                    doc.SetCellValue($"A{nextLine}", "Итого");
                    doc.SetCellStyle($"A{nextLine}", izNixStyle);
                    doc.SetRowHeight(nextLine, 25);


                    foreach (var a in analog)
                    {
                        doc.SetCellValue($"{chars[ch]}{nextLine}", totalByMonth[ch]);
                        doc.SetRowHeight(nextLine, 25);
                        doc.SetCellStyle($"{chars[ch]}{nextLine}", totalStyle);

                        ch++;
                    }






                    // Сохранение документа
                    doc.SaveAs(str);


                    // Открыть файл
                    Process.Start(new ProcessStartInfo { FileName = str, UseShellExecute = true });

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
              

            }

        }

        #region Методы помошники

        //Вернем список выбранных месяцев
        private List<string> ListMouth()
        {
            List<string> strings = new();

            strings.Add("Район");

            if ((bool)checkBox1.IsChecked)
                strings.Add((string)checkBox1.Content);

            if ((bool)checkBox2.IsChecked)
                strings.Add((string)checkBox2.Content);

            if ((bool)checkBox3.IsChecked)
                strings.Add((string)checkBox3.Content);

            if ((bool)checkBox4.IsChecked)
                strings.Add((string)checkBox4.Content);

            if ((bool)checkBox5.IsChecked)
                strings.Add((string)checkBox5.Content);

            if ((bool)checkBox6.IsChecked)
                strings.Add((string)checkBox6.Content);

            if ((bool)checkBox7.IsChecked)
                strings.Add((string)checkBox7.Content);

            if ((bool)checkBox8.IsChecked)
                strings.Add((string)checkBox8.Content);

            if ((bool)checkBox9.IsChecked)
                strings.Add((string)checkBox9.Content);

            if ((bool)checkBox10.IsChecked)
                strings.Add((string)checkBox10.Content);

            if ((bool)checkBox11.IsChecked)
                strings.Add((string)checkBox11.Content);

            if ((bool)checkBox12.IsChecked)
                strings.Add((string)checkBox12.Content);


            return strings;
        }

        //Метод для возврата чисел аналогов Месяцам
        private static List<int> IntMouth(List<string> arft)
        {
            List<int> ints = new();

            foreach (var item in arft)
            {
                switch (item)
                {
                    case "Район": break;
                        
                    case "Январь":
                        ints.Add(1);
                        break;

                    case "Февраль":
                        ints.Add(2);
                        break;

                    case "Март":
                        ints.Add(3);
                        break;

                    case "Апрель":
                        ints.Add(4);
                        break;

                    case "Май":
                        ints.Add(5);
                        break;

                    case "Июнь":
                        ints.Add(6);
                        break;

                    case "Июль":
                        ints.Add(7);
                        break;

                    case "Август":
                        ints.Add(8);
                        break;

                    case "Сентябрь":
                        ints.Add(9);
                        break;

                    case "Октябрь":
                        ints.Add(10);
                        break;

                    case "Ноябрь":
                        ints.Add(11);
                        break;

                    case "Декабрь":
                        ints.Add(12);
                        break;

                    default: break;
                }
            }

            return ints;
        }

        //Кнопка влево
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            yearCodeBehind = yearCodeBehind.AddYears(-1);
            TotalAmountForAllTime.Text = yearCodeBehind.Year.ToString();
        }

        //Кнопка вправо
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            yearCodeBehind = yearCodeBehind.AddYears(1);
            TotalAmountForAllTime.Text = yearCodeBehind.Year.ToString();
        }

        //Галочки поставить все и убрать
        private void AllCheckOrNo_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                checkBox1.IsChecked = true;
                checkBox2.IsChecked = true;
                checkBox3.IsChecked = true;
                checkBox4.IsChecked = true;
                checkBox5.IsChecked = true;
                checkBox6.IsChecked = true;
                checkBox7.IsChecked = true;
                checkBox8.IsChecked = true;
                checkBox9.IsChecked = true;
                checkBox10.IsChecked = true;
                checkBox11.IsChecked = true;
                checkBox12.IsChecked = true;
                allCheckOrNo.Content = "Убрать все галочки";
                flag = false;
            }
            
            else if (!flag)
            {
                checkBox1.IsChecked = false;
                checkBox2.IsChecked = false;
                checkBox3.IsChecked = false;
                checkBox4.IsChecked = false;
                checkBox5.IsChecked = false;
                checkBox6.IsChecked = false;
                checkBox7.IsChecked = false;
                checkBox8.IsChecked = false;
                checkBox9.IsChecked = false;
                checkBox10.IsChecked = false;
                checkBox11.IsChecked = false;
                checkBox12.IsChecked = false;
                allCheckOrNo.Content = "Отметить все галочки";
                flag = true;
            }

        }
        #endregion


    }
}