using Microsoft.Data.Sqlite;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace MyDiaryApplication
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow s = new AccountWindow();
            this.Close();
            s.Show();
        }

        private void loginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoginBox.Text.Length == 0)
            {
                HintLoginBox.Text = "Login";
            }
            else
            {
                HintLoginBox.Text = "";
                return;
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length == 0)
            {
                HintPasswordBox.Text = "Password";
            }
            else
            {
                HintPasswordBox.Text = "";
                return;
            }
        }

        private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (RepeatPasswordBox.Password.Length == 0)
                HintRepeatPasswordBox.Text = "Repeat password";
                else
                HintRepeatPasswordBox.Text = "";
            
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!BoxesFilledRight())
            {
                LoginBox.BorderBrush = Brushes.Red;
                PasswordBox.BorderBrush = Brushes.Red;
                RepeatPasswordBox.BorderBrush = Brushes.Red;
            }

            try
            {
                string connectionString = "Data Source=MyDiaryDB.db;Version=3;";
                using (SQLiteConnection dbConnection = new SQLiteConnection(connectionString))
                {
                    string sql =
                        "CREATE TABLE IF NOT EXISTS users (id INT PRIMARY KEY,login TEXT(20), password TEXT(20))";
                    dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand(sql);
                    sql = $"SELECT COUNT(*) FROM users WHERE login = '{LoginBox.Text}'";
                    command.Parameters.AddWithValue("login", LoginBox.Text);
                    command = new SQLiteCommand(sql,dbConnection);
                    int records = int.Parse(command.ExecuteScalar().ToString());
                    if(records == 0){
                        sql = $"INSERT INTO users (login, password) values ('{LoginBox.Text}', '{PasswordBox.Password}')";
                        command = new SQLiteCommand(sql, dbConnection);
                        command.ExecuteNonQuery();
                        dbConnection.Close();
                        MessageBox.Show("Successful operation!");
                        BackToLoginButton_Click(sender, e);
                    }
                    else
                    {
                        MessageLabel.Content = "User with same login already exists, try another one!";
                    }
                    
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Oops, Something went wrong!");
            }
        }
        

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow accountWindow = new AccountWindow();
            this.Close();
            accountWindow.Show();
        }

        private bool BoxesFilledRight()
        {
            return LoginBox.Text.Length > 5 && PasswordBox.Password.Length > 6 && RepeatPasswordBox.Password.Length > 6;
        }

        private bool SamePasswordInBoxes()
        {
            return PasswordBox.Password == RepeatPasswordBox.Password;
        }

        private void LoginBox_TouchEnter(object sender, System.Windows.Input.TouchEventArgs e)
        {
            LoginBox.BorderBrush = Brushes.White;
        }

        private void PasswordBox_TouchEnter(object sender, System.Windows.Input.TouchEventArgs e)
        {
            PasswordBox.BorderBrush = Brushes.White;
        }

        private void RepeatPasswordBox_TouchEnter(object sender, System.Windows.Input.TouchEventArgs e)
        {
            RepeatPasswordBox.BorderBrush = Brushes.White;
        }
    }
}
