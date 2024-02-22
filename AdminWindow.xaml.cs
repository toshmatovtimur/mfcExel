using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace exel_for_mfc
{
    public partial class AdminWindow : Window
    {
        private int temp = 0;
        public AdminWindow()
        {
            InitializeComponent();
            nam.Content = "Поставить все галочки";
            StartAdminWin();
        }
        #region Редактирование таблиц
        private async void StartAdminWin()
        {
            using ExDbContext db = new();

            var AreaDataGrid = await db.Areas.FromSqlRaw("SELECT * FROM Area").ToListAsync();
            AreaX.ItemsSource = AreaDataGrid.ToList();

            var LocalDataGrid = await db.Localities.FromSqlRaw("SELECT * FROM Locality").ToListAsync();
            LocalX.ItemsSource = LocalDataGrid.ToList();

            var PayDataGrid = await db.PayAmounts.FromSqlRaw("SELECT * FROM PayAmount").ToListAsync();
            PayX.ItemsSource = PayDataGrid.ToList();

            var PrivelDataGrid = await db.Privileges.FromSqlRaw("SELECT * FROM Privileges").ToListAsync();
            PrivelX.ItemsSource = PrivelDataGrid.ToList();

            var SolDataGrid = await db.SolutionTypes.FromSqlRaw("SELECT * FROM SolutionType").ToListAsync();
            SolutionX.ItemsSource = SolDataGrid.ToList();

            var SolDataGridForAdmin = await db.SolutionTypes.FromSqlRaw("SELECT * FROM SolutionType").Take(2).ToListAsync();
            AdminsX.ItemsSource = SolDataGridForAdmin.ToList();

            PayC.ItemsSource = PayDataGrid.ToList();
        }
        private async void AreaCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Считывание строки
            Area? a = e.Row.Item as Area;

            using ExDbContext db = new();

            if (a.Id != 0)
            {
                //Обновление таблицы Район
                await db.Database.ExecuteSqlRawAsync("UPDATE Area SET AreaName = {0} WHERE Id = {1}", a.AreaName, a.Id);
            }

            else if (a.Id == 0)
            {
                // Добавление записи
                if (a.AreaName != null)
                {
                    //Добавить новую запись в таблицу Район
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Area(AreaName) VALUES({a.AreaName})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }
        }
        private async void LocalCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Считывание строки
            Locality? a = e.Row.Item as Locality;

            using ExDbContext db = new();

            if (a.Id != 0)
            {
                //Обновление таблицы Населенный пункт
                await db.Database.ExecuteSqlRawAsync("UPDATE Locality SET LocalName = {0} WHERE Id = {1}", a.LocalName, a.Id);
            }

            else if (a.Id == 0)
            {
                // Добавление записи
                if (a.LocalName != null)
                {
                    //Добавить новую запись в таблицу Населенный пункт
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Locality(LocalName) VALUES({a.LocalName})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }
        }
        private async void LgotaCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Считывание строки
            Privilege? a = e.Row.Item as Privilege;

            using ExDbContext db = new();

            if (a.Id != 0)
            {
                //Обновление таблицы Льгота
                await db.Database.ExecuteSqlRawAsync("UPDATE Privileges SET PrivilegesName = {0} WHERE Id = {1}", a.PrivilegesName, a.Id);
            }

            else if (a.Id == 0)
            {
                // Добавление записи
                if (a.PrivilegesName != null)
                {
                    //Добавить новую запись в таблицу Льгота
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Privileges(PrivilegesName) VALUES({a.PrivilegesName})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }
        }
        private async void PayCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Считывание строки
            PayAmount? a = e.Row.Item as PayAmount;

            using ExDbContext db = new();

            if (a.Id != 0)
            {
                //Обновление таблицы Выплаты
                await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET Pay = {0} WHERE Id = {1}", a.Pay, a.Id);
            }

            else if (a.Id == 0)
            {
                // Добавление записи
                if (a.Pay != null)
                {
                    //Добавить новую запись в таблицу Выплаты
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO PayAmount(Pay) VALUES({a.Pay})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }
        }
        private async void SolutionCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Считывание строки
            SolutionType? a = e.Row.Item as SolutionType;

            using ExDbContext db = new();

            if (a.Id != 0)
            {
                //Обновление таблицы Выплаты
                await db.Database.ExecuteSqlRawAsync("UPDATE SolutionType SET SolutionName = {0} WHERE Id = {1}", a.SolutionName, a.Id);
            }

            else if (a.Id == 0)
            {
                // Добавление записи
                if (a.SolutionName != null)
                {
                    //Добавить новую запись в таблицу Выплаты
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO SolutionType(SolutionName) VALUES({a.SolutionName})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }
        }
        private async void AdminCell(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Считывание строки
            SolutionType? a = e.Row.Item as SolutionType;

            using ExDbContext db = new();

            if (a.Id != 0)
            {
                //Обновление таблицы Логинов и паролей
                await db.Database.ExecuteSqlRawAsync("UPDATE SolutionType SET Login = {0}, Passwords = {1} WHERE Id = {2}", a.Login, MD5Hash(a.Passwords), a.Id);
                StartAdminWin();
            }

            //Метод хэширования вводимого пароля
            static string MD5Hash(string input)
            {
                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }
        #endregion
        #region Интеграция
        [Obsolete]
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await GoIntegration();
            StartAdminWin();
        }

        //Хочу в ТГУ
        static async Task GoIntegration()
        {
            await Task.Run(() =>
            {
                //Интеграция
                OpenFileDialog of = new()
                {
                    Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
                };
                /*
                 1 - фамилия
                 2 - имя
                 3 - отчество
                 4 - снилс
                 5 - район
                 6 - населенный пункт
                 7 - адрес
                 8 - льгота будет Contains
                 9 - серия и номер сертификата
                10 - дата выдачи
                11 - решение
                12 - дата и номер решения по сертификату
                13 - Выплата
                14 - Трек
                15 - Дата отправки почтой
                Предусмотреть NULL
                 */
                if (of.ShowDialog() == true)
                {
                    using FileStream fs = new(of.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false);
                    WorkbookPart? workbookPart = doc.WorkbookPart;
                    SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringTable sst = sstpart.SharedStringTable;

                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    Worksheet sheet = worksheetPart.Worksheet;

                    var cells = sheet.Descendants<Cell>();
                    var rows = sheet.Descendants<Row>();
                    var app = new Applicant();
                    var reg = new Registry();
                    int temp = 0;
                    try
                    {
                        //Второе условие срабатывает на цифры
                        //Просто адский цикл
                        foreach (Row row in rows)
                        {
                            foreach (Cell cell in row.Elements<Cell>())
                            {
                                switch (temp)
                                {
                                    case 0:
                                        temp++;
                                        break;
                                    case 1: //Фамилия **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s1 = int.Parse(cell.CellValue.Text);
                                            string str1 = sst.ChildElements[s1].InnerText;
                                            app.Firstname = str1;
                                        }
                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.Firstname = cell.CellValue.Text;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.Firstname = null;
                                        }
                                        temp++;


                                        break;

                                    case 2: //Имя **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s2 = int.Parse(cell.CellValue.Text);
                                            string str2 = sst.ChildElements[s2].InnerText;
                                            app.Middlename = str2;
                                        }
                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.Middlename = cell.CellValue.Text;
                                        }
                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.Middlename = null;
                                        }
                                        temp++;
                                        break;

                                    case 3: //Отчество **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            app.Lastname = str3;
                                        }
                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.Lastname = cell.CellValue.Text;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.Lastname = null;
                                        }
                                        temp++;
                                        break;

                                    case 4: //Снилс **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            app.Snils = str3;
                                        }
                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.Snils = cell.CellValue.Text;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.Snils = null;
                                        }
                                        temp++;
                                        break;

                                    case 5: //Район **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            app.AreaFk = ReturnIdArea(str3);
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.AreaFk = null;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.AreaFk = null;
                                        }
                                        temp++;
                                        break;

                                    case 6:  //Населенный пункт **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            app.LocalityFk = ReturnIdLocal(str3);
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.LocalityFk = null;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.LocalityFk = null;
                                        }
                                        temp++;
                                        break;

                                    case 7: //Адрес **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            app.Adress = str3;
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.Adress = cell.CellValue.Text;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.Adress = null;
                                        }
                                        temp++;
                                        break;
                                    case 8:  //Льгота **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            app.PrivilegesFk = ReturnIdPriv(str3);
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            app.PrivilegesFk = null;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            app.PrivilegesFk = null;
                                        }
                                        temp++;
                                        break;

                                    case 9:   //Серия и номер сертификата **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            reg.SerialAndNumberSert = str3;
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            reg.SerialAndNumberSert = cell.CellValue.Text;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            reg.SerialAndNumberSert = null;
                                        }
                                        temp++;
                                        break;

                                    case 10: //Дата выдачи сертификата **************************************
                                             //Числа
                                        if (cell.CellValue != null)
                                        {
                                            double s3 = double.Parse(cell.CellValue.Text);
                                            reg.DateGetSert = DateTime.FromOADate(s3);
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            reg.DateGetSert = null;
                                        }
                                        temp++;
                                        break;

                                    case 11: //Решение **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            reg.SolutionFk = ReturnIdSol(str3);
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            reg.SolutionFk = null;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            reg.SolutionFk = null;
                                        }
                                        temp++;
                                        break;

                                    case 12: //Дата и номер решения по сертификату **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            reg.DateAndNumbSolutionSert = str3;
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            reg.DateAndNumbSolutionSert = cell.CellValue.Text;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            reg.DateAndNumbSolutionSert = null;
                                        }
                                        temp++;
                                        break;

                                    case 13: //Выплата **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            reg.PayAmountFk = ReturnIdPay(str3);
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            reg.PayAmountFk = ReturnIdPay(cell.CellValue.Text);
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            reg.PayAmountFk = null;
                                        }
                                        temp++;
                                        break;

                                    case 14: //Трек **************************************
                                        if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                                        {
                                            int s3 = int.Parse(cell.CellValue.Text);
                                            string str3 = sst.ChildElements[s3].InnerText;
                                            reg.Trek = str3;
                                        }

                                        //Числа
                                        else if (cell.CellValue != null)
                                        {
                                            reg.Trek = cell.CellValue.Text;
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            reg.Trek = null;
                                        }
                                        temp++;
                                        break;

                                    case 15: //Дата отправки почтой **************************************
                                             //Числа
                                        if (cell.CellValue != null)
                                        {
                                            double s3 = double.Parse(cell.CellValue.Text);
                                            reg.MailingDate = DateTime.FromOADate(s3);
                                        }

                                        //NULL
                                        else if (cell.DataType == null)
                                        {
                                            reg.MailingDate = null;
                                        }

                                        //Контрольное условие и вставка **************************************
                                        temp = 0;
                                        using (ExDbContext db = new())
                                        {
                                            
                                            db.Applicants.Add(app);
                                            db.SaveChanges();
                                            //Делай запрос чтоб получить данного заявителя и вставлю id в idApplicant
                                            //и потом добавлю это в таблицу Регистр
                                            var getLastApp = db.Applicants.AsNoTracking().OrderBy(u => u.Id).LastOrDefault();
                                            if (getLastApp != null)
                                                reg.ApplicantFk = getLastApp.Id;
                                            else
                                                reg.ApplicantFk = null;
                                            db.Registries.Add(reg);
                                            db.SaveChanges();
                                            app = new Applicant();
                                            reg = new Registry();
                                        }
                                        break;

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }


        //Функция возврата Района
        static int ReturnIdArea(string str)
        {
            int idArea = 0;
            using (ExDbContext db = new())
            {
                var equalArea = db.Areas.AsNoTracking().Where(u => u.AreaName == str).FirstOrDefault();
                if (equalArea != null)
                    idArea = equalArea.Id;

                else if (equalArea == null)
                {
                    Area area = new();
                    area.AreaName = str;
                    area.HidingArea = 1;
                    db.Areas.Add(area);
                    db.SaveChanges();
                    // И ветнуть id нового
                    var getIdLast = db.Areas.AsNoTracking().OrderBy(u => u.Id).LastOrDefault();
                    if (getIdLast != null)
                        idArea = getIdLast.Id;
                    else return 0;

                }
            }
            return idArea;
        }

        //Функция возврата Населенного пункта
        static int ReturnIdLocal(string str)
        {
            int idLocal = 0;
            using (ExDbContext db = new())
            {
                var equalLoc = db.Localities.AsNoTracking().Where(u => u.LocalName == str).FirstOrDefault();
                if (equalLoc != null)
                    idLocal = equalLoc.Id;
                 
                else if (equalLoc == null)
                {
                    Locality loc = new();
                    loc.LocalName = str;
                    loc.HidingLocal = 1;
                    db.Localities.Add(loc);
                    db.SaveChanges();
                    // И ветнуть id нового
                    var getIdLast = db.Localities.AsNoTracking().OrderBy(u => u.Id).LastOrDefault();
                    if (getIdLast != null)
                        idLocal = getIdLast.Id;
                    else return 0;
                }
            }
            return idLocal;
        }

        //Функция возврата Льгота
        static int ReturnIdPriv(string str)
        {
            int idPriv = 0;
            using (ExDbContext db = new())
            {
                var equalPriv = db.Privileges.AsNoTracking().Where(u => u.PrivilegesName == str).FirstOrDefault();
                if (equalPriv != null)
                    idPriv = equalPriv.Id;

                else if (equalPriv == null)
                {
                    Privilege privilege = new();
                    privilege.PrivilegesName = str;
                    privilege.HidingPriv = 1;
                    db.Privileges.Add(privilege);
                    db.SaveChanges();
                    // И ветнуть id нового
                    var getIdLast = db.Privileges.AsNoTracking().OrderBy(u => u.Id).LastOrDefault();
                    if (getIdLast != null)
                        idPriv = getIdLast.Id;
                    else return 0;

                }
            }
            return idPriv;
        }

        //Функция возврата Решение
        static int ReturnIdSol(string str)
        {
            int idSol = 0;
            using (ExDbContext db = new())
            {
                var equalSol = db.SolutionTypes.AsNoTracking().Where(u => u.SolutionName == str).FirstOrDefault();
                if (equalSol != null)
                    idSol = equalSol.Id;

                else if (equalSol == null)
                {
                    SolutionType solution = new();
                    solution.SolutionName = str;
                    solution.HidingSol = 1;
                    db.SolutionTypes.Add(solution);
                    db.SaveChanges();
                    // И ветнуть id нового
                    var getIdLast = db.SolutionTypes.OrderBy(u => u.Id).LastOrDefault();
                    if (getIdLast != null)
                        idSol = getIdLast.Id;
                    else return 0;
                }
            }
            return idSol;
        }

        //Функция возврата Выплата
        static int ReturnIdPay(string str)
        {
            int idPay = 0;
            using (ExDbContext db = new())
            {
                var equalPay = db.PayAmounts.AsNoTracking().Where(u => u.Pay == Convert.ToDecimal(str)).FirstOrDefault();
                if (equalPay != null)
                    idPay = equalPay.Id;

                else if (equalPay == null)
                {
                    PayAmount pays = new();
                    pays.Pay = Convert.ToDecimal(str);
                    pays.HidingPay = 1;
                    db.PayAmounts.Add(pays);
                    db.SaveChanges();
                    // И ветнуть id нового
                    var getIdLast = db.PayAmounts.AsNoTracking().OrderBy(u => u.Id).LastOrDefault();
                    if (getIdLast != null)
                        idPay = getIdLast.Id;
                    else return 0;
                }
            }
            return idPay;
        }
        #endregion
        #region CheckBoxesHiding
        //Чек Район
        private async void AreaCheck(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE Area SET hidingArea={0} WHERE id={1}", 1, (AreaX.SelectedItem as Area)?.Id);
        }

        //Анчек район
        private async void UnCheckArea(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE Area SET hidingArea={0} WHERE id={1}", 0, (AreaX.SelectedItem as Area)?.Id);
        }

        //Чек локал
        private async void CheckLocal(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE Locality SET hidingLocal={0} WHERE id={1}", 1, (LocalX.SelectedItem as Locality)?.Id);
        }

        //Анчек локал
        private async void UnCheckLocal(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE Locality SET hidingLocal={0} WHERE id={1}", 0, (LocalX.SelectedItem as Locality)?.Id);
        }

        //Чек прив
        private async void ChPriv(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE Privileges SET hidingPriv={0} WHERE id={1}", 1, (PrivelX.SelectedItem as Privilege)?.Id);
        }

        //Анчек прив
        private async void UnchPriv(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE Privileges SET hidingPriv={0} WHERE id={1}", 0, (PrivelX.SelectedItem as Privilege)?.Id);
        }

        //Чек Пай
        private async void ChPay(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET hidingPay={0} WHERE id={1}", 1, (PayX.SelectedItem as PayAmount)?.Id);
        }

        //АнЧек Пай
        private async void UnChPay(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET hidingPay={0} WHERE id={1}", 0, (PayX.SelectedItem as PayAmount)?.Id);
        }

        //Чек Сол
        private async void ChSol(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE SolutionType SET hidingSol={0} WHERE id={1}", 1, (SolutionX.SelectedItem as SolutionType)?.Id);
        }

        //Анчек Сол
        private async void UnChSol(object sender, RoutedEventArgs e)
        {
            using ExDbContext db = new();
            await db.Database.ExecuteSqlRawAsync("UPDATE SolutionType SET hidingSol={0} WHERE id={1}", 0, (SolutionX.SelectedItem as SolutionType)?.Id);
        }
        #endregion
        #region Разное


        private void AdminClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
        }

        //Поставить все галочки
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (temp == 0)
            {
                nam.Content = "Убрать все галочки";
                using ExDbContext db = new();
                await db.Database.ExecuteSqlRawAsync("UPDATE SolutionType SET hidingSol={0}", 1);
                await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET hidingPay={0}", 1);
                await db.Database.ExecuteSqlRawAsync("UPDATE Privileges SET hidingPriv={0}", 1);
                await db.Database.ExecuteSqlRawAsync("UPDATE Locality SET hidingLocal={0}", 1);
                await db.Database.ExecuteSqlRawAsync("UPDATE Area SET hidingArea={0}", 1);
                StartAdminWin();
                temp = 1;
            }

            else if (temp == 1)
            {
                nam.Content = "Поставить все галочки";
                temp = 0;
                using ExDbContext db = new();
                await db.Database.ExecuteSqlRawAsync("UPDATE SolutionType SET hidingSol={0}", 0);
                await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET hidingPay={0}", 0);
                await db.Database.ExecuteSqlRawAsync("UPDATE Privileges SET hidingPriv={0}", 0);
                await db.Database.ExecuteSqlRawAsync("UPDATE Locality SET hidingLocal={0}", 0);
                await db.Database.ExecuteSqlRawAsync("UPDATE Area SET hidingArea={0}", 0);
                StartAdminWin();
            }

        }

        //Редактирование адреса
        private async void CellAdress(object sender, DataGridCellEditEndingEventArgs e)
        {
            //Считывание строки
            PayAmount? a = e.Row.Item as PayAmount;

            using ExDbContext db = new();

            if (a.Id != 0 && e.Column.Header.ToString() == "Микро/Типы")
            {
                //Обновление таблицы Выплаты
                await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET mkr = {0} WHERE Id = {1}", a.Mkr, a.Id);
            }

            else if (a.Id != 0 && e.Column.Header.ToString() == "Улица/Типы")
            {
                //Обновление таблицы Выплаты
                await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET ulica = {0} WHERE Id = {1}", a.Ulica, a.Id);
            }

            else if (a.Id != 0 && e.Column.Header.ToString() == "Квартира/типы")
            {
                //Обновление таблицы Выплаты
                await db.Database.ExecuteSqlRawAsync("UPDATE PayAmount SET kvartira = {0} WHERE Id = {1}", a.Kvartira, a.Id);
            }

            else if (a.Id == 0 && e.Column.Header.ToString() == "Микро/Типы")
            {
                // Добавление записи
                if (a.Pay != null)
                {
                    //Добавить новую запись в таблицу Выплаты
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO PayAmount(mkr) VALUES({a.Mkr})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }

            else if (a.Id == 0 && e.Column.Header.ToString() == "Улица/Типы")
            {
                // Добавление записи
                if (a.Pay != null)
                {
                    //Добавить новую запись в таблицу Выплаты
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO PayAmount(ulica) VALUES({a.Ulica})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }

            else if (a.Id == 0 && e.Column.Header.ToString() == "Квартира/типы")
            {
                // Добавление записи
                if (a.Pay != null)
                {
                    //Добавить новую запись в таблицу Выплаты
                    await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO PayAmount(kvartira) VALUES({a.Kvartira})");
                    await Task.Delay(50);
                    StartAdminWin();
                }
                else return;
            }
        }
        #endregion

       
    }
}