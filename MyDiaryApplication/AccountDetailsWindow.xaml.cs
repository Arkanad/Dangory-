using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MyDiaryApplication
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class AccountDetailsWindow
    {

        public AccountDetailsWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string? currentLogin = null;
            using (var connection = new SqliteConnection("Data Source = MyDiaryDB.db; Mode=ReadOnly;"))
            {
                try
                {
                    connection.Open();
                    SqliteDataReader sqliteDatareader;
                    SqliteCommand sqliteCmd;
                    sqliteCmd = connection.CreateCommand();
                    sqliteCmd.CommandText = $"SELECT * FROM Users";

                    sqliteDatareader = sqliteCmd.ExecuteReader();
                    while (sqliteDatareader.Read())
                    {
                        if (LoginBox.Text.Length == 0)
                        {
                            currentLogin = sqliteDatareader["login"].ToString();
                            HintLoginBox.Text = $"{currentLogin}";
                        }
                        else
                        {
                            HintLoginBox.Text = "";
                            return;
                        }

                    }
                }
                catch (Exception exception)
                {
                    ShowErrorMessage();
                }
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HintPasswordBox.Text = PasswordBox.Password.Length == 0 ? "Enter current password" : "";
        }

        private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HintRepeatPasswordBox.Text = RepeatPasswordBox.Password.Length == 0 ? "Confirm your current password" : "";
        }
        private bool AccountBoxesFilled()
        {
            if (LoginBox.Text.Length <= 4 || PasswordBox.Password.Length <= 5 || PasswordBox.Password.Length <= 5)
            {
                return false;
            }
            return true;
        }

        private int GetCurrentID()
        {
            int id = 0;
            using (var connection = new SqliteConnection("Data Source = MyDiaryDB.db; Mode=ReadOnly;"))
            {
                try
                {
                    connection.Open();
                    SqliteDataReader sqlite_datareader;
                    SqliteCommand sqlite_cmd;
                    sqlite_cmd = connection.CreateCommand();
                    sqlite_cmd.CommandText = $"SELECT * FROM Users";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if (sqlite_datareader["login"].ToString() == this.LoginBox.Text && sqlite_datareader["password"].ToString() ==
                            this.PasswordBox.Password)
                            id = int.Parse(sqlite_datareader["id"].ToString());
                    }
                    return id;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Oops, Something went wrong!");
                }
            }

            return 0;
        }

        private void ChangeLabelInfo(string infoText)
        {
            InfoLabel.Content = infoText;
        }

        private void ShowErrorMessage()
        {
            MessageBox.Show("Oops, Something went wrong!");
        }

        private void NewLoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HintNewLogin.Text = NewLoginTextBox.Text.Length == 0 ? "New login" : "";
        }

        private void NewPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HintNewPasswordBox.Text = NewPasswordTextBox.Password.Length == 0 ? "New password" : "";
        }

        private void UpdateButtonClick_Click(object sender, RoutedEventArgs e)
        {
            if (!AccountBoxesFilled())
            {
                if (LoginBox.Text.Length <= 4 || NewLoginTextBox.Text.Length <= 4)
                {
                    ChangeLabelInfo("Login length has to be at least 4 symbols");
                }
                else if ((PasswordBox.Password.Length <= 5 || PasswordBox.Password.Length <= 5) || !PasswordBox.Password.Equals(RepeatPasswordBox.Password) || NewPasswordTextBox.Password.Length <= 5)
                {
                    ChangeLabelInfo("Fields Password and Repeat Password have to be filled same and have at least 6 symbols!");
                }
                else
                {
                    ShowErrorMessage();
                }
            }
            try
            {
                string connectionString = "Data Source=MyDiaryDB.db;Version=3;";
                using (SQLiteConnection mDbConnection = new SQLiteConnection(connectionString))
                {
                    mDbConnection.Open();
                    string sql = $"UPDATE users SET login = \"{NewLoginTextBox.Text}\", password = \"{NewPasswordTextBox.Password}\" WHERE id = \"{GetCurrentID()}\"";
                    SQLiteCommand command = new SQLiteCommand(sql, mDbConnection);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Done!");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error! Couldn`t end registration process. \n Please, try again!");
            }
        }
    }
}
