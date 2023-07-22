using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using LinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;

namespace MyDiaryApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void BackgroundPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String fn = Environment.CurrentDirectory + "\\Data\\Pictures\\" + (BackgroundPicker.SelectedIndex + 1).ToString() + ".jpg";
            Uri s = new Uri(fn);
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(s));
            Background = new ImageBrush(new BitmapImage(s));
            //--------------------Save Settings-------------
            Properties.Settings.Default.SelectedIndexBackground =
                BackgroundPicker.SelectedIndex + 1;
            Properties.Settings.Default.Save();
            //----------------------------------------------
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            int selectedBackgroundIndexByDefault =
                Properties.Settings.Default.SelectedIndexBackground;
            String fn = Environment.CurrentDirectory + "\\Data\\Pictures\\" + (selectedBackgroundIndexByDefault).ToString() + ".jpg";
            Uri s = new Uri(fn);
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(s));
            Background = new ImageBrush(new BitmapImage(s));
        }

        private void TimeUpdate(object? sender, EventArgs e)
        {
            ClockDate.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        private void ClockDate_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimeUpdate;
            timer.Start();
        }

        private void AddMemoButton_Click(object sender, RoutedEventArgs e)
        {
            MemoWindow memoWindow = new MemoWindow();
            memoWindow.ShowDialog();
        }

        private void DateBlock_Loaded(object sender, RoutedEventArgs e)
        {
            DateBlock.Text = DateTime.Now.Date.Kind.ToString();
        }

        private void DateBlockMonth_Loaded(object sender, RoutedEventArgs e)
        {
            DateBlockMonth.Text = $"{DateTime.Now.Day}/{DateTime.Now.Month.ToString()}";
        }

        private void DateBlockYear_Loaded(object sender, RoutedEventArgs e)
        {
            DateBlockYear.Text = DateTime.Now.Year.ToString();
        }

        private void AboutUsButton_Click(object sender, RoutedEventArgs e)
        {
            AboutUsWindow aboutUsWindow = new AboutUsWindow();
            aboutUsWindow.Topmost = true;
            aboutUsWindow.ShowDialog();
        }

        private void AccountDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            AccountDetailsWindow accountDetailsWindow = new AccountDetailsWindow();
            accountDetailsWindow.Topmost = true;
            accountDetailsWindow.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SearchMemoWindow searchMemo = new SearchMemoWindow();
            this.Close();
            searchMemo.Show();
        }

        
    }
}
