using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using Microsoft.Win32;
using Newtonsoft.Json;


namespace CoronaAPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        List<Root> CountriesList = new List<Root>();

        private void DownloadJasonData()
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Accept", "application/json");
                string jsonResponse = client.DownloadString("https://covid-19.dataflowkit.com/v1");
                CountriesList = JsonConvert.DeserializeObject<List<Root>>(jsonResponse);
            }
            catch (Exception ex)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Odczyt z pliku JSON";
                dialog.Filter = "Plik(*.json)|*.json";
                string fileResponse = File.ReadAllText(@"api.json");
                CountriesList = JsonConvert.DeserializeObject<List<Root>>(fileResponse);
                MessageBox.Show("Currently API is unavailable.\nData dowloaded from file.");
            }     
            CountriesList = CountriesList.OrderBy(x => x.Country_text).ToList();
        }

        public MainWindow()
        {
            InitializeComponent();
            DownloadJasonData();
            UpdateGui();
        }

        private void UpdateGui()
        {
            SelectCountry.Items.Clear();
            SelectCountry.Items.Add("World");
            foreach (var item in CountriesList)
            {
                if (item.Country_text != "World")
                    SelectCountry.Items.Add(item.Country_text);
            }

            SelectCountry.SelectedIndex = 0;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SaveToDatabase.IsEnabled = true;
            ShowDatabase.IsEnabled = true;
            string country = "";
            string date = "";
            string deaths = "";
            string cases = "";


            for (int i = 0; i < CountriesList.Count; i++)
            {
                if (CountriesList[i].Country_text == SelectCountry.Text)
                {
                    country = CountriesList[i].Country_text;
                    date = CountriesList[i].LastUpdate;
                    deaths = CountriesList[i].TotalDeathsText;
                    cases = CountriesList[i].TotalCasesText;
                }
            }

            CountryName.Text = country;
            try
            {
                Date.Text = date.Substring(0, 10);
            }
            catch (Exception ex)
            {
                Date.Text = "No data";
            }
            try
            {
                ConfD.Text = deaths;
            }
            catch (Exception ex)
            {
                ConfD.Text = "No data";
            }
            try
            {
                ConfC.Text = cases;
            }
            catch (Exception ex)
            {
                ConfC.Text = "No data";
            }           
        }

        private void SaveToDatabase_Click(object sender, RoutedEventArgs e)
        {
            Database database = new Database();
            string query = "INSERT INTO history (`country`,`date`,`confC`,`confD`) VALUES(@country,@date,@confC,@confD)";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);

            database.myConnection.Open();

            myCommand.Parameters.AddWithValue("@country", CountryName.Text);
            myCommand.Parameters.AddWithValue("@date", Date.Text);
            myCommand.Parameters.AddWithValue("@confC", ConfC.Text);
            myCommand.Parameters.AddWithValue("@confD", ConfD.Text);

            myCommand.ExecuteNonQuery();
            database.myConnection.Close();
        }

        private void ShowDatabase_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            //this.Visibility = Visibility.Hidden;
            window1.Show();
        }
    }
}