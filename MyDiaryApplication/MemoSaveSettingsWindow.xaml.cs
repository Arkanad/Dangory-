using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyDiaryApplication.Properties;

namespace MyDiaryApplication
{
    /// <summary>
    /// Interaction logic for MemoSaveSettingsWindow.xaml
    /// </summary>
    public partial class MemoSaveSettingsWindow : Window
    {
        public static Dictionary<string?, int> MonthsDictionary = new()
        {
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(1)}",1},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(2)}",2},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(3)}",3},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(4)}",4},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(5)}",5},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(6)}",6},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(7)}",7},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(8)}",8},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(9)}",9},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(10)}",10},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(11)}",11},
            {$"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(12)}",12}
        };

        public MemoSaveSettingsWindow()
        {
            InitializeComponent();
        }

        private void YearSelection_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MemoSettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            YearSelection.ItemsSource = Enumerable.Range(2000, DateTime.Now.Year - 1983 + 1);
        }

        private void MonthSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.MonthSelection.SelectedIndex > 0 && MonthSelection.SelectedIndex <= 12)
            {
                this.DaySelection.IsEnabled = true;
                DaySelection.ItemsSource = Enumerable.Range(1, DateTime.DaysInMonth(int.Parse(YearSelection.SelectedItem.ToString()), int.Parse(MonthsDictionary[MonthSelection.SelectedItem.ToString()].ToString())));
            }
        }

        private void YearSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.YearSelection.SelectedIndex != -1)
            {
                
                MonthSelection.IsEnabled = true;
                MonthSelection.ItemsSource = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames.Take(12);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.IsDateByDefault = false;
            Settings.Default.DaySavingData = int.Parse(DaySelection.SelectedItem.ToString() ?? throw new InvalidOperationException());
            Settings.Default.MonthSavingData = MonthSelection.SelectedItem.ToString();
            Settings.Default.YearSavingData = int.Parse(YearSelection.SelectedItem.ToString() ?? throw new InvalidOperationException());
            MessageBox.Show("Settings saved!");
        }

        private void SetTodayDateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    }

