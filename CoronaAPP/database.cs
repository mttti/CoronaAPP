﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoronaAPP
{
    internal class Database
    {
        public SQLiteConnection myConnection;

        public Database()
        {
            myConnection = new SQLiteConnection("Data Source = database.sqlite3");
            if (!File.Exists("./database.sqlite3"))
                SQLiteConnection.CreateFile("database.sqlite3");

            string sql = "create table if not exists history(country varchar(50), date varchar(12), confC varchar(10), confD varchar(10))";
            OpenConnection();
            SQLiteCommand cmd = new SQLiteCommand(sql, myConnection);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public void OpenConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Open)
                myConnection.Open();
        }

        public void CloseConnection()
        {
            if(myConnection.State != System.Data.ConnectionState.Closed)
                myConnection.Close();
        }

    }
}
