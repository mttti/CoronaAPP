using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
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

namespace CoronaAPP
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            GetValues();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window1 = new MainWindow();
            this.Close();
            window1.Show();
        }

        public void GetValues()
        {
            Database database = new Database();
            string query = "SELECT * FROM history";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);

            database.myConnection.Open();
            myCommand.ExecuteNonQuery();

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(myCommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DatabaseView.ItemsSource = ds.Tables[0].DefaultView;
            }
            database.myConnection.Close();
        }
    }
}
