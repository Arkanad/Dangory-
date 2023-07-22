using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;



namespace MyDiaryApplication
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        MainWindow mainWindow = new MainWindow();

        public AccountWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            
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
                        {
                            Properties.Settings.Default.Login = this.LoginBox.Text;
                    Properties.Settings.Default.Password = this.PasswordBox.Password.ToString();
                            mainWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            LoginLabelMessage.Content = "Incorrect login or password!";
                        }
                    }

                    
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Oops, Something went wrong!");
                }
            }
        }

        //static void CreateTable(SqliteConnection connection)
        //{
        //    SqliteCommand sqliteCmd;
        //    string createsqliteTable = "CREATE TABLE SampleTable(id INT PRIMARYKEY, login TEXT UNIQUE, password TEXT)";
        //    sqliteCmd = connection.CreateCommand();
        //    sqliteCmd.CommandText = $"{createsqliteTable}";
        //    sqliteCmd.ExecuteNonQuery();
        //}

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            this.AccountWindow1.Close();
            registrationWindow.Show();

        }

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
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

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
