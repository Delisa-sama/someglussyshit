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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.Sql;
using Microsoft.Reporting.WinForms;
using System.Text.RegularExpressions;

using Npgsql;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        //private bool _isReportViewerLoaded;
        //String connectionString;

        //Npgsql.NpgsqlConnection con;
        //Npgsql.NpgsqlDataAdapter adap;
        //DataTable dt;
        Window1 admWin;
        private bool _isReportViewerLoaded;
        String connectionString;

        Npgsql.NpgsqlConnection connection;
        Npgsql.NpgsqlDataAdapter adapter;
        DataTable tab;

        public MainWindow()
        {
            DataTable tab = new DataTable();

            NpgsqlDataAdapter adapter = null;
            connection = new NpgsqlConnection();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void logButtonClicked(object sender, RoutedEventArgs e)
        {
            Label1.Visibility = Visibility.Hidden;
            Label2.Visibility = Visibility.Hidden;
            AdminPanelButton.Visibility = Visibility.Hidden;
            connectionString = String.Format("Host=127.0.0.1;Username={0};Password={1};Database=name",
                textBox1.Text, pasBox.Password);
            pasBox.Password = "";

            connection.ConnectionString = connectionString.ToString();

            try
            {
                connection.Open();

                MessageBox.Show("Успешно", "Соединение");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка соединения");
            }

            pasBox.Visibility = Visibility.Hidden;
            textBox1.Visibility = Visibility.Hidden;
            logButton.Visibility = Visibility.Hidden;
            discButton.Visibility = Visibility.Visible;
            execButton.Visibility = Visibility.Visible;
            tablesComboBox.Visibility = Visibility.Visible;
            procComboBox.Visibility = Visibility.Visible;
            updateButton.Visibility = Visibility.Visible;

        }

        private void disButtonClick(object sender, RoutedEventArgs e)
        {
            pasBox.Visibility = Visibility.Visible;
            logButton.Visibility = Visibility.Visible;
            discButton.Visibility = Visibility.Hidden;
            execButton.Visibility = Visibility.Hidden;
            tablesComboBox.Visibility = Visibility.Hidden;
            procComboBox.Visibility = Visibility.Hidden;

            textBox1.Visibility = Visibility.Visible;
            textBox2.Visibility = Visibility.Hidden;
            textBox3.Visibility = Visibility.Hidden;
            textBox4.Visibility = Visibility.Hidden;
            textBox5.Visibility = Visibility.Hidden;
            textBox6.Visibility = Visibility.Hidden;
            datePicker1.Visibility = Visibility.Hidden;
            datePicker2.Visibility = Visibility.Hidden;

            updateButton.Visibility = Visibility.Hidden;

            connection.Close();
        }

        private void update()
        {

            _reportViewer.Reset();
            switch (tablesComboBox.SelectedIndex)
            {

                case 0:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM airplanes_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "airplanes_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "airplanes_rep.rdlc";
                    break;
                case 1:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM airports_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "airports_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "airports_rep.rdlc";
                    break;
                case 2:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM cities_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "cities_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "cities_rep.rdlc";
                    break;
                case 3:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM flights_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "flights_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "flights_rep.rdlc";
                    break;
                case 4:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM passengers_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "passengers_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "passengers_rep.rdlc";
                    break;
                case 5:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM tickets_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "tickets_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "tickets_rep.rdlc";
                    break;
                case 6:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM avg_cost_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "avg_ticket_cost_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "avg_ticket_cost_rep.rdlc";
                    break;
            }
            _reportViewer.ProcessingMode = ProcessingMode.Local;
            tab = new DataTable();
            DataSet ds = new DataSet();
            
            try
            {
                adapter.Fill(tab);
            }
            catch (Exception)
            {
                return;
            }
            MainTable.ItemsSource = tab.DefaultView;

            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", tab);
            _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            _reportViewer.RefreshReport();

            //switch (tablesComboBox.SelectedIndex)
            //{
            //    case 0:
            //        MainTable.Columns[0].Header = "Номер самолета";
            //        MainTable.Columns[1].Header = "Название самолета";
            //        MainTable.Columns[2].Header = "Марка самолета";
            //        break;
            //    case 1:
            //        MainTable.Columns[0].Header = "Номер аэропорта";
            //        MainTable.Columns[1].Header = "Номер города";
            //        MainTable.Columns[2].Header = "Название аэропорта";
            //        break;
            //    case 2:
            //        MainTable.Columns[0].Header = "Номер города";
            //        MainTable.Columns[1].Header = "Название города";
            //        break;
            //    case 3:
            //        MainTable.Columns[0].Header = "Дата отправления";
            //        MainTable.Columns[1].Header = "Дата прибытия";
            //        MainTable.Columns[2].Header = "Номер рейса";
            //        MainTable.Columns[3].Header = "Номер самолета";
            //        MainTable.Columns[4].Header = "Номер аэропорта прибытия";
            //        MainTable.Columns[5].Header = "Номер аэропорта отбытия";
            //        break;
            //    case 4:
            //        MainTable.Columns[0].Header = "Номер пассажира";
            //        MainTable.Columns[1].Header = "Имя";
            //        MainTable.Columns[2].Header = "Фамилия";
            //        MainTable.Columns[3].Header = "Отчество";
            //        MainTable.Columns[4].Header = "Дата рождения";
            //        break;
            //    case 5:
            //        MainTable.Columns[0].Header = "Номер места";
            //        MainTable.Columns[1].Header = "Цена";
            //        MainTable.Columns[2].Header = "Номер билета";
            //        MainTable.Columns[3].Header = "Номер пассажира";
            //        MainTable.Columns[4].Header = "Номер рейса";
            //        break;
            //    case 6:
            //        MainTable.Columns[0].Header = "Номер рейса";
            //        MainTable.Columns[1].Header = "Средняя цена билета";
            //        break;
            //}

        }

        private void tablesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            update();
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Label1.Visibility = Visibility.Hidden;
            Label2.Visibility = Visibility.Hidden;
            Label3.Visibility = Visibility.Hidden;
            Label4.Visibility = Visibility.Hidden;
            Label5.Visibility = Visibility.Hidden;
            Label6.Visibility = Visibility.Hidden;
            Label1.Text = "";
            Label2.Text = "";
            Label3.Text = "";
            Label4.Text = "";
            Label5.Text = "";
            Label6.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox1.Visibility = Visibility.Hidden;
            textBox2.Visibility = Visibility.Hidden;
            textBox3.Visibility = Visibility.Hidden;
            textBox4.Visibility = Visibility.Hidden;
            textBox5.Visibility = Visibility.Hidden;
            textBox6.Visibility = Visibility.Hidden;
            datePicker1.Visibility = Visibility.Hidden;
            datePicker2.Visibility = Visibility.Hidden;

            comboBox1.Visibility = Visibility.Hidden;
            comboBox2.Visibility = Visibility.Hidden;
            comboBox3.Visibility = Visibility.Hidden;
            comboBox4.Visibility = Visibility.Hidden;
            comboBox5.Visibility = Visibility.Hidden;
            comboBox6.Visibility = Visibility.Hidden;

            NpgsqlDataAdapter a_tmp;
            switch (procComboBox.SelectedIndex)
            {
                case 0:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label1.Text = "Марка самолета";
                    Label2.Text = "Имя самолета";
                    textBox1.Visibility = Visibility.Visible;
                    textBox2.Visibility = Visibility.Visible;
                    break;
                case 1:
                    Label1.Visibility = Visibility.Visible;
                    Label1.Text = "Город";
                    textBox2.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label2.Text = "Название аэропорта";
                    // Пример
                    comboBox1.Visibility = Visibility.Visible;
                    DataTable dt_cities_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM cities_v", connection);
                    a_tmp.Fill(dt_cities_tmp);

                    comboBox1.DisplayMemberPath = "Название";
                    comboBox1.SelectedValuePath = "Номер";
                    comboBox1.ItemsSource = dt_cities_tmp.DefaultView;
                    
                    break;
                case 2:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label4.Visibility = Visibility.Visible;
                    Label1.Text = "Дата рождения";
                    Label2.Text = "Имя";
                    Label3.Text = "Фамилия";
                    Label4.Text = "Отчество";
                    datePicker1.Visibility = Visibility.Visible;
                    textBox2.Visibility = Visibility.Visible;
                    textBox3.Visibility = Visibility.Visible;
                    textBox4.Visibility = Visibility.Visible;
                    break;
                case 3:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label1.Text = "Логин";
                    Label2.Text = "Пароль";
                    Label3.Text = "Номер роли";
                    textBox1.Visibility = Visibility.Visible;
                    textBox2.Visibility = Visibility.Visible;
                    textBox3.Visibility = Visibility.Visible;
                    break;
                case 4:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label4.Visibility = Visibility.Visible;
                    Label1.Text = "Номер пассажира";

                    DataTable dt_pasnum_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM passengers_v", connection);
                    a_tmp.Fill(dt_pasnum_tmp);

                    comboBox1.DisplayMemberPath = "Номер";
                    comboBox1.SelectedValuePath = "Номер";
                    comboBox1.ItemsSource = dt_pasnum_tmp.DefaultView;
                    
                    Label2.Text = "Имя";
                    Label3.Text = "Фамилия";
                    Label4.Text = "Отчество";
                    comboBox1.Visibility = Visibility.Visible;
                    textBox2.Visibility = Visibility.Visible;
                    textBox3.Visibility = Visibility.Visible;
                    textBox4.Visibility = Visibility.Visible;
                    break;
                case 5:
                    Label1.Visibility = Visibility.Visible;
                    Label1.Text = "Номер рейса";
                    comboBox1.Visibility = Visibility.Visible;
                    DataTable dt_flnum_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM flights_v", connection);
                    a_tmp.Fill(dt_flnum_tmp);

                    comboBox1.DisplayMemberPath = "flight_number";
                    comboBox1.SelectedValuePath = "flight_number";
                    comboBox1.ItemsSource = dt_flnum_tmp.DefaultView;
                    
                    break;
                case 6:
                    Label1.Visibility = Visibility.Visible;
                    Label1.Text = "Номер билета";
                    comboBox1.Visibility = Visibility.Visible;
                    DataTable dt_tnum_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM tickets_v", connection);
                    a_tmp.Fill(dt_tnum_tmp);

                    comboBox1.DisplayMemberPath = "Номер билета";
                    comboBox1.SelectedValuePath = "Номер билета";
                    comboBox1.ItemsSource = dt_tnum_tmp.DefaultView;
                    break;
                case 7:
                    Label1.Visibility = Visibility.Visible;
                    Label1.Text = "Номер самолета";
                    comboBox1.Visibility = Visibility.Visible;
                    DataTable dt_airpnum_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM airplanes_v", connection);
                    a_tmp.Fill(dt_airpnum_tmp);

                    comboBox1.DisplayMemberPath = "Номер";
                    comboBox1.SelectedValuePath = "Номер";
                    comboBox1.ItemsSource = dt_airpnum_tmp.DefaultView;
                    break;
                case 8:
                    Label1.Visibility = Visibility.Visible;
                    Label1.Text = "Номер аэропорта";
                    comboBox1.Visibility = Visibility.Visible;
                    DataTable dt_airnum_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM airports_v", connection);
                    a_tmp.Fill(dt_airnum_tmp);

                    comboBox1.DisplayMemberPath = "Номер";
                    comboBox1.SelectedValuePath = "Номер";
                    comboBox1.ItemsSource = dt_airnum_tmp.DefaultView;
                    break;
                case 9:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label1.Text = "Номер самолета";
                    Label2.Text = "Имя самолета";
                    Label3.Text = "Марка самолета";
                    textBox1.Visibility = Visibility.Visible;
                    textBox2.Visibility = Visibility.Visible;
                    textBox3.Visibility = Visibility.Visible;
                    break;
                case 10:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label1.Text = "Номер аэропорта";
                    comboBox1.Visibility = Visibility.Visible;

                    DataTable dt_airports_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM airports_v", connection);
                    a_tmp.Fill(dt_airports_tmp);

                    comboBox1.DisplayMemberPath = "Номер аэропорта";
                    comboBox1.SelectedValuePath = "Номер аэропорта";
                    comboBox1.ItemsSource = dt_airports_tmp.DefaultView;

                    Label2.Text = "Название города";
                    comboBox2.Visibility = Visibility.Visible;

                    DataTable dt_cities_tmpp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM cities_v", connection);
                    a_tmp.Fill(dt_cities_tmpp);

                    comboBox2.DisplayMemberPath = "Название";
                    comboBox2.SelectedValuePath = "Номер";
                    comboBox2.ItemsSource = dt_cities_tmpp.DefaultView;

                    Label3.Text = "Название аэропорта";
                    textBox3.Visibility = Visibility.Visible;
                    break;
                case 11:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label4.Visibility = Visibility.Visible;
                    Label5.Visibility = Visibility.Visible;
                    Label6.Visibility = Visibility.Visible;
                    Label1.Text = "Дата отбытия";
                    Label2.Text = "Дата прибытия";
                    Label3.Text = "Номер рейса";
                    comboBox3.Visibility = Visibility.Visible;
                    DataTable dt_flights_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM flights_v", connection);
                    a_tmp.Fill(dt_flights_tmp);

                    comboBox3.DisplayMemberPath = "flight_number";
                    comboBox3.SelectedValuePath = "flight_number";
                    comboBox3.ItemsSource = dt_flights_tmp.DefaultView;

                    comboBox4.Visibility = Visibility.Visible;

                    Label4.Text = "Номер самолета";
                    DataTable dt_fl_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM airplanes_v", connection);
                    a_tmp.Fill(dt_fl_tmp);

                    comboBox4.DisplayMemberPath = "Номер";
                    comboBox4.SelectedValuePath = "Номер";
                    comboBox4.ItemsSource = dt_fl_tmp.DefaultView;

                    Label5.Text = "Номер аэропорта прибытия";
                    Label6.Text = "Номер аэропорта отбытия";
                    datePicker1.Visibility = Visibility.Visible;
                    datePicker2.Visibility = Visibility.Visible;
                    textBox5.Visibility = Visibility.Visible;
                    textBox6.Visibility = Visibility.Visible;
                    break;
                case 12:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label4.Visibility = Visibility.Visible;
                    Label5.Visibility = Visibility.Visible;
                    Label1.Text = "Дата рождения";
                    Label2.Text = "Номер пассажира";
                    comboBox2.Visibility = Visibility.Visible;
                    DataTable dt_passengers_tmp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM passengers_v", connection);
                    a_tmp.Fill(dt_passengers_tmp);

                    comboBox2.DisplayMemberPath = "Номер";
                    comboBox2.SelectedValuePath = "Номер";
                    comboBox2.ItemsSource = dt_passengers_tmp.DefaultView;

                    Label3.Text = "Имя";
                    Label4.Text = "Фамилия";
                    Label5.Text = "Отчество";
                    datePicker1.Visibility = Visibility.Visible;
                    textBox3.Visibility = Visibility.Visible;
                    textBox4.Visibility = Visibility.Visible;
                    textBox5.Visibility = Visibility.Visible;
                    break;
                case 13:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label1.Text = "Логин";
                    Label2.Text = "Пароль";
                    Label3.Text = "Номер роли";
                    textBox1.Visibility = Visibility.Visible;
                    textBox2.Visibility = Visibility.Visible;
                    textBox3.Visibility = Visibility.Visible;

                    break;
                case 14:
                    Label1.Visibility = Visibility.Visible;
                    Label2.Visibility = Visibility.Visible;
                    Label3.Visibility = Visibility.Visible;
                    Label4.Visibility = Visibility.Visible;
                    Label5.Visibility = Visibility.Visible;
                    Label6.Visibility = Visibility.Visible;
                    Label1.Text = "Дата отбытия";
                    Label2.Text = "Дата прибытия";
                    Label3.Text = "Номер рейса";
                    comboBox3.Visibility = Visibility.Visible;
                    DataTable dt_flights_tmpp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM flights_v", connection);
                    a_tmp.Fill(dt_flights_tmpp);

                    comboBox3.DisplayMemberPath = "flight_number";
                    comboBox3.SelectedValuePath = "flight_number";
                    comboBox3.ItemsSource = dt_flights_tmpp.DefaultView;

                    Label4.Text = "Номер самолета";
                    DataTable dt_fl_tmpp = new DataTable();
                    a_tmp = new NpgsqlDataAdapter("SELECT * FROM airplanes_v", connection);
                    a_tmp.Fill(dt_fl_tmpp);

                    comboBox4.DisplayMemberPath = "Номер";
                    comboBox4.SelectedValuePath = "Номер";
                    comboBox4.ItemsSource = dt_fl_tmpp.DefaultView;

                    Label5.Text = "Номер аэропорта прибытия";
                    Label6.Text = "Номер аэропорта отбытия";
                    datePicker1.Visibility = Visibility.Visible;
                    datePicker2.Visibility = Visibility.Visible;
                    textBox4.Visibility = Visibility.Visible;
                    textBox5.Visibility = Visibility.Visible;
                    textBox6.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void execButtonClicked(object sender, RoutedEventArgs e)
        {
            Npgsql.NpgsqlCommand com = new NpgsqlCommand();
            com.Connection = connection;

            Regex namePattern = new Regex("[A-Za-zА-Яа-я]+");
            Regex markPattern = new Regex("[A-Za-zА-Яа-я0-9-]+");
            Regex numberPattern = new Regex("[0-9]+");

            switch (procComboBox.SelectedIndex)
            {
                case 0:
                    //if (namePattern.IsMatch(textBox1.Text) && markPattern.IsMatch(textBox2.Text))
                    //    com.CommandText = String.Format("SELECT * FROM add_airplane ('{0}', '{1}')",
                    //                                    textBox1.Text, textBox2.Text);
                    //else MessageBox.Show("Ошибка");

                    com.CommandText = "SELECT * FROM add_airplane(:air_brand, :air_name)";
                    com.Parameters.AddWithValue(":air_brand", textBox1.Text);
                    com.Parameters.AddWithValue(":air_name", textBox2.Text);
                    break;
                case 1:
                    //if (numberPattern.IsMatch(textBox1.Text) && namePattern.IsMatch(textBox2.Text))
                    //    com.CommandText = String.Format("SELECT * FROM add_airport ('{0}', '{1}')",
                    //                                textBox1.Text, textBox2.Text);
                    //else MessageBox.Show("Ошибка");
                    
                    com.CommandText = "SELECT * FROM add_airport (:city_num, :air_name)";
                    com.Parameters.AddWithValue(":city_num", NpgsqlTypes.NpgsqlDbType.Integer, comboBox1.SelectedIndex + 1);
                    com.Parameters.AddWithValue(":air_name", textBox2.Text);
                    break;
                case 2:
                    //if (namePattern.IsMatch(textBox2.Text) && namePattern.IsMatch(textBox3.Text)
                    //    && namePattern.IsMatch(textBox4.Text))
                    //    com.CommandText = String.Format("SELECT * FROM add_passenger ('{0}', '{1}', '{2}','{3}')",
                    //                                    textBox2.Text, textBox3.Text, textBox4.Text,
                    //                                    datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"));
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM add_passenger (:f_name, :l_name, :m_name,  :date_of_b)";
                    com.Parameters.AddWithValue(":f_name", textBox2.Text);
                    com.Parameters.AddWithValue(":l_name", textBox3.Text);
                    com.Parameters.AddWithValue(":m_name", textBox4.Text);
                    com.Parameters.AddWithValue(":date_of_b", NpgsqlTypes.NpgsqlDbType.Date, datePicker1.SelectedDate.Value);
                    break;
                case 3:
                    //if (datePicker1.SelectedDate.HasValue)
                    //    if (namePattern.IsMatch(textBox1.Text) && namePattern.IsMatch(textBox2.Text)
                    //    && numberPattern.IsMatch(textBox3.Text))
                    //        com.CommandText = String.Format("SELECT * FROM add_user ('{0}', '{1}', '{2}')",
                    //                                textBox1.Text, textBox2.Text,//datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    //                                textBox3.Text);
                    //    else MessageBox.Show("Ошибка");

                    break;
                case 4:
                    //if (numberPattern.IsMatch(textBox1.Text) && namePattern.IsMatch(textBox2.Text)
                    //    && namePattern.IsMatch(textBox3.Text) && namePattern.IsMatch(textBox4.Text))
                    //    com.CommandText = String.Format("SELECT * FROM buy_ticket ('{0}', '{1}', '{2}', '{3}')",
                    //                                textBox1.Text, textBox2.Text,
                    //                                textBox3.Text, textBox4.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM buy_ticket (:pas_num, :f_name, :l_name, :m_name)";
                    com.Parameters.AddWithValue(":pas_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox1.Text);
                    com.Parameters.AddWithValue(":f_name", textBox2.Text);
                    com.Parameters.AddWithValue(":l_name", textBox3.Text);
                    com.Parameters.AddWithValue(":m_name", textBox4.Text);
                    break;
                case 5:
                    //if (numberPattern.IsMatch(textBox1.Text))
                    //    com.CommandText = String.Format("SELECT * FROM cancel_flight ('{0}')",
                    //                                    textBox1.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM cancel_flight (:f_num)";
                    com.Parameters.AddWithValue(":f_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox1.Text);
                    break;
                case 6:
                    //if (numberPattern.IsMatch(textBox1.Text))
                    //    com.CommandText = String.Format("SELECT * FROM cancel_ticket ('{0}')",
                    //                                textBox1.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM cancel_ticket (:t_num)";
                    com.Parameters.AddWithValue(":t_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox1.Text);
                    break;
                case 7:
                    //if (numberPattern.IsMatch(textBox1.Text))
                    //    com.CommandText = String.Format("SELECT * FROM delete_airplane ('{0}')",
                    //                                textBox1.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM delete_airplane (:a_num)";
                    com.Parameters.AddWithValue(":a_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox1.Text);
                    break;
                case 8:
                    //if (numberPattern.IsMatch(textBox1.Text))
                    //    com.CommandText = String.Format("SELECT * FROM delete_airport ('{0}')",
                    //                                textBox1.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM delete_airplane (:a_num)";
                    com.Parameters.AddWithValue(":a_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox1.Text);
                    break;
                case 9:
                    //if (numberPattern.IsMatch(textBox1.Text) && namePattern.IsMatch(textBox2.Text)
                    //    && namePattern.IsMatch(textBox3.Text))
                    //    com.CommandText = String.Format("SELECT * FROM modify_airplane ('{0}', '{1}', '{2}')",
                    //                                textBox1.Text, textBox2.Text, textBox3.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM modify_airplane (:air_num, :air_name, :air_brand)";
                    com.Parameters.AddWithValue(":air_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox1.Text);
                    com.Parameters.AddWithValue(":air_name", textBox2.Text);
                    com.Parameters.AddWithValue(":air_brand", textBox3.Text);
                    break;
                case 10:
                    //if (numberPattern.IsMatch(textBox1.Text) && numberPattern.IsMatch(textBox2.Text)
                    //    && namePattern.IsMatch(textBox3.Text))
                    //    com.CommandText = String.Format("SELECT * FROM modify_airport ('{0}', '{1}', '{2}')",
                    //                                textBox1.Text, textBox2.Text, textBox3.Text);
                    //else MessageBox.Show("Ошибка");

                    com.CommandText = "SELECT * FROM modify_airport (:air_num, :city_num, :airp_num)";
                    com.Parameters.AddWithValue(":air_num", NpgsqlTypes.NpgsqlDbType.Integer, comboBox1.SelectedValue);
                    com.Parameters.AddWithValue(":city_num", NpgsqlTypes.NpgsqlDbType.Integer, comboBox2.SelectedValue);
                    com.Parameters.AddWithValue(":airp_num", NpgsqlTypes.NpgsqlDbType.Text, textBox3.Text);
                    break;
                case 11:
                    //if (numberPattern.IsMatch(textBox3.Text) && numberPattern.IsMatch(textBox4.Text)
                    //    && numberPattern.IsMatch(textBox5.Text) && numberPattern.IsMatch(textBox6.Text))
                    //    com.CommandText = String.Format("SELECT * FROM modify_flight ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                    //                                textBox3.Text, textBox4.Text,
                    //                                datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    //                                datePicker2.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    //                                textBox5.Text, textBox6.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM modify_flight (:fl_number, :air_num, :dep_date, :arr_date, :arr_air_num, :dep_air_num)";
                    com.Parameters.AddWithValue(":fl_number", NpgsqlTypes.NpgsqlDbType.Integer, textBox3.Text);
                    com.Parameters.AddWithValue(":air_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox4.Text);
                    com.Parameters.AddWithValue(":dep_date", NpgsqlTypes.NpgsqlDbType.Date, datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"));
                    com.Parameters.AddWithValue(":arr_date", NpgsqlTypes.NpgsqlDbType.Date, datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"));
                    com.Parameters.AddWithValue(":arr_air_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox5.Text);// COMBOBOXES
                    com.Parameters.AddWithValue(":dep_air_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox6.Text);
                    break;
                case 12:
                    //if (numberPattern.IsMatch(textBox2.Text) && namePattern.IsMatch(textBox3.Text)
                    //    && namePattern.IsMatch(textBox4.Text) && namePattern.IsMatch(textBox5.Text))
                    //    com.CommandText = String.Format("SELECT * FROM modify_passengers ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    //                                textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text,
                    //                                datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"));
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM modify_passengers (:pas_num, :f_name, :l_name, :m_name, :date_of_b)";
                    com.Parameters.AddWithValue(":pas_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox2.Text);
                    com.Parameters.AddWithValue(":f_name", textBox3.Text);
                    com.Parameters.AddWithValue(":l_name", textBox4.Text);
                    com.Parameters.AddWithValue(":m_name", textBox5.Text);
                    com.Parameters.AddWithValue(":date_of_b", NpgsqlTypes.NpgsqlDbType.Date, datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"));
                    break;
                case 13:
                    //if (namePattern.IsMatch(textBox1.Text) && namePattern.IsMatch(textBox2.Text)
                    //    && numberPattern.IsMatch(textBox3.Text))
                    //    com.CommandText = String.Format("SELECT * FROM modify_user ('{0}', '{1}', '{2}')",
                    //                                textBox1.Text, textBox2.Text, textBox3.Text);
                    //else MessageBox.Show("Ошибка");

                    break;
                case 14:
                    //if (numberPattern.IsMatch(textBox3.Text) && numberPattern.IsMatch(textBox4.Text)
                    //    && numberPattern.IsMatch(textBox5.Text) && numberPattern.IsMatch(textBox6.Text))
                    //    com.CommandText = String.Format("SELECT * FROM register_flight ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                    //                                textBox3.Text, textBox4.Text,
                    //                                datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    //                                datePicker2.SelectedDate.Value.ToString("dd-MM-yyyy"),
                    //                                textBox5.Text, textBox6.Text);
                    //else MessageBox.Show("Ошибка");
                    com.CommandText = "SELECT * FROM register_flight (:fl_number, :air_num, :dep_date, :arr_date, :arr_air_num, :dep_air_num)";
                    com.Parameters.AddWithValue(":fl_number", NpgsqlTypes.NpgsqlDbType.Integer, textBox3.Text);
                    com.Parameters.AddWithValue(":air_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox4.Text);
                    com.Parameters.AddWithValue(":dep_date", NpgsqlTypes.NpgsqlDbType.Date, datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"));
                    com.Parameters.AddWithValue(":arr_date", NpgsqlTypes.NpgsqlDbType.Date, datePicker1.SelectedDate.Value.ToString("dd-MM-yyyy"));
                    com.Parameters.AddWithValue(":arr_air_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox5.Text);// COMBOBOXES
                    com.Parameters.AddWithValue(":dep_air_num", NpgsqlTypes.NpgsqlDbType.Integer, textBox5.Text);
                    break;
            }
            try
            {
                com.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {

                MessageBox.Show(ex.Message, "Ошибка");
            }

            update();
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {

                _reportViewer.Reset();
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource("", tab);
                _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                _reportViewer.LocalReport.ReportEmbeddedResource = "Report1.rdlc";
                _reportViewer.RefreshReport();
            }
        }

        private void gen_col(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                DataGridTextColumn dataGridTextColumn = e.Column as DataGridTextColumn;
                if (dataGridTextColumn != null)
                {
                    dataGridTextColumn.Binding.StringFormat = "{0:dd.MM.yyyy}";
                }
            }
        }

        private void admButtonClicked(object sender, RoutedEventArgs e)
        {
            if (admWin is null || (!admWin.IsVisible))
            {
                admWin = new Window1();
                admWin.Show();
            }
        }

        private void UpClickded(object sender, RoutedEventArgs e)
        {
            _reportViewer.Reset();

            switch (tablesComboBox.SelectedIndex)
            {
                case 0:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM airplanes_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "airplanes_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "airplanes_rep.rdlc";
                    break;
                case 1:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM airports_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "airports_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "airports_rep.rdlc";
                    break;
                case 2:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM cities_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "cities_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "cities_rep.rdlc";
                    break;
                case 3:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM flights_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "flights_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "flights_rep.rdlc";
                    break;
                case 4:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM passengers_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "passengers_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "passengers_rep.rdlc";
                    break;
                case 5:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM tickets_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "tickets_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "tickets_rep.rdlc";
                    break;
                case 6:
                    adapter = new NpgsqlDataAdapter("SELECT * FROM avg_cost_v", connection);
                    _reportViewer.LocalReport.ReportEmbeddedResource = "avg_ticket_cost_rep.rdlc";
                    _reportViewer.LocalReport.ReportPath = "avg_ticket_cost_rep.rdlc";
                    break;
            }
            _reportViewer.ProcessingMode = ProcessingMode.Local;
            tab = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                adapter.Fill(tab);
            }
            catch (Exception)
            {
                return;
            }
            tab.Locale = System.Globalization.CultureInfo.GetCultureInfo(0x0019);

            MainTable.ItemsSource = tab.DefaultView;

            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", tab);
            _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            _reportViewer.RefreshReport();

            //switch (tablesComboBox.SelectedIndex)
            //{
            //    case 0:
            //        MainTable.Columns[0].Header = "Номер самолета";
            //        MainTable.Columns[1].Header = "Название самолета";
            //        MainTable.Columns[2].Header = "Марка самолета";
            //        break;
            //    case 1:
            //        MainTable.Columns[0].Header = "Номер аэропорта";
            //        MainTable.Columns[1].Header = "Номер города";
            //        MainTable.Columns[2].Header = "Название аэропорта";
            //        break;
            //    case 2:
            //        MainTable.Columns[0].Header = "Номер города";
            //        MainTable.Columns[1].Header = "Название города";
            //        break;
            //    case 3:
            //        MainTable.Columns[0].Header = "Дата отправления";
            //        MainTable.Columns[1].Header = "Дата прибытия";
            //        MainTable.Columns[2].Header = "Номер рейса";
            //        MainTable.Columns[3].Header = "Номер самолета";
            //        MainTable.Columns[4].Header = "Номер аэропорта прибытия";
            //        MainTable.Columns[5].Header = "Номер аэропорта отбытия";
            //        break;
            //    case 4:
            //        MainTable.Columns[0].Header = "Номер пассажира";
            //        MainTable.Columns[1].Header = "Имя";
            //        MainTable.Columns[2].Header = "Фамилия";
            //        MainTable.Columns[3].Header = "Отчество";
            //        MainTable.Columns[4].Header = "Дата рождения";
            //        break;
            //    case 5:
            //        MainTable.Columns[0].Header = "Номер места";
            //        MainTable.Columns[1].Header = "Цена";
            //        MainTable.Columns[2].Header = "Номер билета";
            //        MainTable.Columns[3].Header = "Номер пассажира";
            //        MainTable.Columns[4].Header = "Номер рейса";
            //        break;
            //    case 6:
            //        MainTable.Columns[0].Header = "Номер рейса";
            //        MainTable.Columns[1].Header = "Средняя цена билета";
            //        break;
            //}
        }

        private void onUpdateButton_Clicked(object sender, RoutedEventArgs e)
        {
            update();
        }
    }
}
