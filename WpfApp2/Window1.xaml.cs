
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;
using System.Data;
using System.Data.Sql;
using Microsoft.Reporting.WinForms;
using System.Text.RegularExpressions;



namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>

    public partial class Window1 : Window
    {
        String connectionString;

        Npgsql.NpgsqlConnection con;// = new NpgsqlConnection(connectionString);
        Npgsql.NpgsqlDataAdapter adap;
        DataTable dt;

        public Window1()
        {
            InitializeComponent();
            con = new NpgsqlConnection();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (procCB.SelectedIndex != -1)
            {
                exeBtn.Visibility = Visibility.Visible;
            }
        }

        private void logBtnClicked(object sender, RoutedEventArgs e)
        {
            Regex pat = new Regex("^([A-Za-z0-9])+$");
            if (!pat.IsMatch(TB1.Text))
            {
                MessageBox.Show("Некорректный формат строк", "Ошибка!");
                return;
            }

            connectionString = String.Format("Host=127.0.0.1;Username={0};Password={1};Database=name",
                TB1.Text, pasTB.Password);
            pasTB.Password = "";

            con.ConnectionString = connectionString.ToString();

            try
            {
                con.Open();

                MessageBox.Show("Connection established", "Connection established!");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message, "Connection error");
                return;
            }

            TB1.Text = "";
            TB1.Visibility = Visibility.Hidden;
            TB2.Visibility = Visibility.Hidden;
            pasTB.Visibility = Visibility.Hidden;
            pasTB.Password = "";
            admTab.Visibility = Visibility.Visible;

            textB1.Text = "";
            textB2.Text = "";

            updateBtn.Visibility = Visibility.Visible;
            logBtn.Visibility = Visibility.Hidden;
            disBtn.Visibility = Visibility.Visible;
            xmlBtn.Visibility = Visibility.Visible;
            procCB.Visibility = Visibility.Visible;
            catCB.Visibility = Visibility.Visible;
            admTabTyp.Visibility = Visibility.Visible;
            exeBtn.Visibility = Visibility.Visible;


            adap = new NpgsqlDataAdapter("SELECT * FROM users_v", con);

            dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                adap.Fill(dt);
            }
            catch (Exception)
            {
                return;
            }

            admTabTyp.SelectedIndex = 0;

            admTab.ItemsSource = dt.DefaultView;

            procCB.Visibility = Visibility.Visible;
            catCB.Visibility = Visibility.Visible;
        }

        private void disBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Close();

            TB2.Visibility = Visibility.Visible;
            disBtn.Visibility = Visibility.Hidden;
            logBtn.Visibility = Visibility.Visible;
            pasTB.Visibility = Visibility.Visible;
            exeBtn.Visibility = Visibility.Hidden;
            procCB.Visibility = Visibility.Hidden;
            catCB.Visibility = Visibility.Hidden;
            updateBtn.Visibility = Visibility.Hidden;
            admTab.Visibility = Visibility.Visible;
            xmlBtn.Visibility = Visibility.Hidden;
            admTabTyp.Visibility = Visibility.Hidden;
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (admTabTyp.SelectedIndex)
            {
                case 0: // юзера

                    adap = new NpgsqlDataAdapter("SELECT * FROM users_v", con);

                    dt = new DataTable();

                    try
                    {
                        adap.Fill(dt);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    admTab.ItemsSource = dt.DefaultView;

                    admTab.Columns[0].Header = "Имя роли";
                    admTab.Columns[1].Header = "Имя группы";

                    break;

                case 1: // логи

                    adap = new NpgsqlDataAdapter("SELECT * FROM logs", con);

                    dt = new DataTable();

                    try
                    {
                        adap.Fill(dt);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    admTab.ItemsSource = dt.DefaultView;

                    break;
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (catCB.SelectedIndex != -1)
            {
                exeBtn.Visibility = Visibility.Visible;
            }

            textB1.Text = "";
            textB2.Text = "";

            TB1.Visibility = Visibility.Hidden;
            TB2.Visibility = Visibility.Hidden;

            switch (procCB.SelectedIndex) {
                case 0:
                    textB1.Text = "Имя роли";
                    textB2.Text = "Пароль роли";

                    TB1.Visibility = Visibility.Visible;
                    TB2.Visibility = Visibility.Visible;
                    break;
                case 1:
                    textB1.Text = "Имя роли";
                    textB2.Text = "";

                    TB1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    textB1.Text = "Имя роли";
                    textB2.Text = "";

                    TB1.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void exeBtnClicked(object sender, RoutedEventArgs e)
        {
            if ((TB1.Text.Length == 0 || TB2.Text.Length == 0) && procCB.SelectedIndex == 0)
            {
                MessageBox.Show("Поля должны быть заполнены!", "Ошибка");
                return;
            }
            
            Npgsql.NpgsqlCommand com = new NpgsqlCommand();
            com.Connection = con;

            string tmpGr = "";

            if (procCB.SelectedIndex != 2)
                switch (catCB.SelectedIndex)
                {
                    case 0:
                        tmpGr = "admin";
                        break;
                    case 1:
                        tmpGr = "air_manager";
                        break;
                    case 2:
                        tmpGr = "air_vehicles_manager";
                        break;
                    case 3:
                        tmpGr = "airport_manager";
                        break;
                    case 4:
                        tmpGr = "client";
                        break;
                    case 5:
                        tmpGr = "operator";
                        break;
                }
            else
                if (TB1.Text == "admin"
                    || TB1.Text == "air_manager"
                    || TB1.Text == "air_vehicles_manager"
                    || TB1.Text == "airport_manager"
                    || TB1.Text == "client"
                    || TB1.Text == "operator")
                {
                    MessageBox.Show("Данные роли удалять нельзя", "Ошибка!");
                    return;
                }

            Regex pat = new Regex("^([A-Za-z0-9])+$");

            try
            {
                switch (procCB.SelectedIndex)
                {
                    case 0:
                        if (!pat.IsMatch(TB1.Text) || !pat.IsMatch(TB2.Text))
                        {
                            MessageBox.Show("Некорректный формат строк!", "Ошибка");
                            return;
                        }
                        com.CommandText = "SELECT createRole (:login, :password, :gr)";
                        com.Parameters.AddWithValue(":login", TB1.Text);
                        com.Parameters.AddWithValue(":password", TB2.Text);
                        com.Parameters.AddWithValue(":gr", tmpGr);
                        break;
                    case 1:
                        if (!pat.IsMatch(TB1.Text))
                        {
                            MessageBox.Show("Некорректный формат строк!", "Ошибка");
                            return;
                        }
                        com.CommandText = "SELECT changeInherit (:login, :gr)";
                        com.Parameters.AddWithValue(":login", TB1.Text);
                        com.Parameters.AddWithValue(":gr", tmpGr);
                        break;
                    case 2:
                        if (!pat.IsMatch(TB1.Text))
                        {
                            MessageBox.Show("Некорректный формат строк!", "Ошибка");
                            return;
                        }
                        com.CommandText = "SELECT dropRole (:login)";
                        com.Parameters.AddWithValue(":login", TB1.Text);
                        break;
                }

                com.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
                return;
            }

            MessageBox.Show("Манипуляция с данными прошла успешно. Обновите таблицу!","Ок");
        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            switch (admTabTyp.SelectedIndex)
            {
                case 0: // юзера

                    adap = new NpgsqlDataAdapter("SELECT * FROM users_v", con);

                    dt = new DataTable();
                    
                    try
                    {
                        adap.Fill(dt);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    admTab.ItemsSource = dt.DefaultView;

                    admTab.Columns[0].Header = "Имя роли";
                    admTab.Columns[1].Header = "Имя группы";

            break;

                case 1: // логи

                    adap = new NpgsqlDataAdapter("SELECT * FROM logs", con);

                    dt = new DataTable();
                    
                    try
                    {
                        adap.Fill(dt);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    admTab.ItemsSource = dt.DefaultView;

            break;
            }
        }

        private void xmlBtnClicked(object sender, RoutedEventArgs e)
        {
            DataTable[] dt_a = new DataTable[7];
            dt_a[0] = new DataTable();
            dt_a[1] = new DataTable();
            dt_a[2] = new DataTable();
            dt_a[3] = new DataTable();
            dt_a[4] = new DataTable();
            dt_a[5] = new DataTable();
            dt_a[6] = new DataTable();

            DataSet ds = new DataSet();

            System.Windows.Forms.SaveFileDialog fd = new System.Windows.Forms.SaveFileDialog();
            try
            {
                if (fd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                
            }
            catch (System.Exception)
            {
                MessageBox.Show("Ошибка во время открытия файла XML!", "Ок");
            }


            try
            {
                adap = new NpgsqlDataAdapter("SELECT * FROM airplanes_v", con);
                adap.Fill(dt_a[0]);
                dt_a[0].TableName = "Самолеты";
                ds.Tables.Add(dt_a[0]);

                adap = new NpgsqlDataAdapter("SELECT * FROM airports_v", con);
                adap.Fill(dt_a[1]);
                dt_a[1].TableName = "Аэропорты";
                ds.Tables.Add(dt_a[1]);

                adap = new NpgsqlDataAdapter("SELECT * FROM cities_v", con);
                adap.Fill(dt_a[2]);
                dt_a[2].TableName = "Города";
                ds.Tables.Add(dt_a[2]);

                adap = new NpgsqlDataAdapter("SELECT * FROM flights_v", con);
                adap.Fill(dt_a[3]);
                dt_a[3].TableName = "Рейсы";
                ds.Tables.Add(dt_a[3]);

                adap = new NpgsqlDataAdapter("SELECT * FROM passengers_v", con);
                adap.Fill(dt_a[4]);
                dt_a[4].TableName = "Пассажиры";
                ds.Tables.Add(dt_a[4]);

                adap = new NpgsqlDataAdapter("SELECT * FROM tickets_v", con);
                adap.Fill(dt_a[5]);
                dt_a[5].TableName = "Билеты";
                ds.Tables.Add(dt_a[5]);

                adap = new NpgsqlDataAdapter("SELECT * FROM avg_cost_v", con);
                adap.Fill(dt_a[6]);
                dt_a[6].TableName = "Средняя стоимость билета";
                ds.Tables.Add(dt_a[6]);

                ds.WriteXml(fd.FileName);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Ошибка во время генерации XML!", "Ок");
            }
         }
    }
}
