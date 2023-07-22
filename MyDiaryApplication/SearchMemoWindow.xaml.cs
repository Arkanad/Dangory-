using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace MyDiaryApplication
{
    /// <summary>
    /// Interaction logic for SearchMemoWindow.xaml
    /// </summary>
    public partial class SearchMemoWindow : Window
    {
        private string folderName;
        private string pathToFile;
        public SearchMemoWindow()
        {
            InitializeComponent();
        }

        private void MonthSelection_Loaded(object sender, RoutedEventArgs e)
        {
            MonthSelection.ItemsSource = CultureInfo.InvariantCulture.DateTimeFormat
                                                     .MonthNames.Take(12).ToList();
            MonthSelection.SelectedItem = CultureInfo.InvariantCulture.DateTimeFormat
                                                    .MonthNames[DateTime.Now.AddMonths(-1).Month - 1];
        }

        private void YearSelection_Loaded(object sender, RoutedEventArgs e)
        {
            YearSelection.ItemsSource = Enumerable.Range(1983, DateTime.Now.Year - 1983 + 1).ToList();
            YearSelection.SelectedItem = DateTime.Now.Year;
        }

        private void PopulateListBox(ListBox lsb, string Folder, string FileType)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            FileInfo[] Files = dinfo.GetFiles(FileType);
            foreach (FileInfo file in Files)
            {
                lsb.Items.Add(file.Name);
            }
        }

        private void MonthSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyDiaryFilesListBox.Items.Clear();
            try
            { 
                folderName = $"{MonthSelection.SelectedItem.ToString()}_{YearSelection.SelectedItem.ToString()}"; 
                pathToFile = $"{Environment.CurrentDirectory}\\Data\\DiaryFiles\\{folderName}";
                foreach (string file in Directory.EnumerateFiles(pathToFile, "*.rtf"))
                {
                    MyDiaryFilesListBox.Items.Add(new ListBoxItem().Content = Path.GetFileName(file).Replace('_',' '));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Not found any data!");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void MyDiaryFilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            folderName = $"{MonthSelection.SelectedItem.ToString()}_{YearSelection.SelectedItem.ToString()}";
            pathToFile = $"{Environment.CurrentDirectory}\\Data\\DiaryFiles\\{folderName}\\{MyDiaryFilesListBox.SelectedValue.ToString().Replace(' ','_')}";
            MessageRTB.Visibility = Visibility.Hidden;
            TextRange tRange = new TextRange(this.DemoFileRichTextBox.Document.ContentStart,
                this.DemoFileRichTextBox.Document.ContentEnd);
            if (!string.IsNullOrEmpty(pathToFile))
            {
                try
                {
                    System.IO.FileStream fileStream =
                        new FileStream(pathToFile, System.IO.FileMode.Open);
                    tRange.Load(fileStream, DataFormats.Rtf);
                    fileStream.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error!");
                }
            }
            else
            {
                return;
            }
        }

        private void LoadToWorkButton_Click(object sender, RoutedEventArgs e)
        {
            MemoWindow memoWindow = new MemoWindow(pathToFile);
            this.Close();
            memoWindow.Show();
            memoWindow.MainRichTextBox.IsEnabled = true;
        }
    }
}
