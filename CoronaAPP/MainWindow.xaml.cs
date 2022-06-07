using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;


namespace CoronaAPP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Root
        {
            public string Country_text { get; set; }

            [JsonProperty("Last Update")]
            public string LastUpdate { get; set; }

            [JsonProperty("Total Cases_text")]
            public string TotalCasesText { get; set; }

            [JsonProperty("Total Deaths_text")]
            public string TotalDeathsText { get; set; }

        }
        List<Root> CountriesList = new List<Root>();

        public async void DownloadJasonData()
        {
            HttpClient httpClient = new HttpClient();

            string url = "https://covid-19.dataflowkit.com/v1";

            var httpResponse = await httpClient.GetAsync(url);

            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

            CountriesList = JsonConvert.DeserializeObject<List<Root>>(jsonResponse);
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

            foreach (var item in CountriesList)
            {
                SelectCountry.Items.Add(item.Country_text);
            }

            SelectCountry.SelectedIndex = 0;
        }
    }
}
