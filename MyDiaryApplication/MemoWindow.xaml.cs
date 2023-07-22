using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using MyDiaryApplication.Properties;

namespace MyDiaryApplication
{
    /// <summary>
    /// Interaction logic for MemoWindow.xaml
    /// </summary>
    public partial class MemoWindow : Window
    {
        public MemoWindow()
        {
            InitializeComponent();
            
        }
        public MemoWindow(string searchedFile)
        {
            InitializeComponent();
            LoadSearchedFile(searchedFile);
        }

        private bool _sizeCheck;

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FontSizeMinus_Click(object sender, RoutedEventArgs e)
        {
            MainRichTextBox.FontSize -= 1;
        }

        private void FontSizePlus_Click(object sender, RoutedEventArgs e)
        {
            MainRichTextBox.FontSize += 1;
        }

        private void AlignTextLeft_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.AlignLeft.Execute(null, this.MainRichTextBox);
        }

        private void AlignTextCenter_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.AlignCenter.Execute(null, this.MainRichTextBox);
        }

        private void AlignTextRight_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.AlignRight.Execute(null, this.MainRichTextBox);
        }

        private void JustifyText_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.AlignJustify.Execute(null, this.MainRichTextBox);
        }

        private void PushTextLeftButton_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.DecreaseIndentation.Execute(null, this.MainRichTextBox);
        }

        private void PushTextRightButton_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.IncreaseIndentation.Execute(null, this.MainRichTextBox);
        }

        private void UnderlineTextButton_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleUnderline.Execute(null,this.MainRichTextBox);
        }

        private void ItalicTextButton_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleItalic.Execute(null, this.MainRichTextBox);
        }

        private void BoldTextButton_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleBold.Execute(null, this.MainRichTextBox);
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainRichTextBox.Redo();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainRichTextBox.Undo();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainRichTextBox.Copy();
        }

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainRichTextBox.Cut();
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainRichTextBox.Paste();
        }

        private void Collapse_Click(object sender, RoutedEventArgs e)
        {
            if (_sizeCheck == false)
            {
                MainMemoWindow.WindowState = WindowState.Minimized;
                _sizeCheck = true;
            }
            else if (_sizeCheck == true)
            {
                MainMemoWindow.WindowState = WindowState.Normal;
                _sizeCheck = false;
            }
        }

        private void NewMemoButton_Click(object sender, RoutedEventArgs e)
        {
            this.MainRichTextBox.IsEnabled = true;
        }

        private void SaveMemoButton_Click(object sender, RoutedEventArgs e)
        {
            string? monthSaving = null;
            string? yearSaving = null; 
            string? daySaving = null;
            string? folderName = null;
            string? path =
                null;
            MemoSaveSettingsWindow memoSaveSettingsWindow = new MemoSaveSettingsWindow();
            memoSaveSettingsWindow.ShowDialog();
            if (Settings.Default.IsDateByDefault == false)
            {
                yearSaving = Settings.Default.YearSavingData.ToString();
                monthSaving = Settings.Default.MonthSavingData;
                daySaving = Settings.Default.DaySavingData.ToString();

            }
            else
            {
                monthSaving = CultureInfo.InvariantCulture.DateTimeFormat
                    .MonthNames[DateTime.Now.AddMonths(-1).Month];
                yearSaving = DateTime.Now.Year.ToString();
                daySaving = DateTime.Now.Day.ToString();
            }

            folderName = $"{monthSaving}_{yearSaving}";
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            path = $"{Environment.CurrentDirectory}\\Data\\DiaryFiles\\{folderName}\\{daySaving}_{monthSaving}_{yearSaving}.rtf";
            if (!Directory.Exists($"{Environment.CurrentDirectory}\\Data\\DiaryFiles\\{folderName}"))
            {
                Directory.CreateDirectory($"{Environment.CurrentDirectory}\\Data\\DiaryFiles\\{folderName}");
            }



            TextRange tRange = new TextRange(this.MainRichTextBox.Document.ContentStart,
                this.MainRichTextBox.Document.ContentEnd);
            System.IO.FileStream fileStream =
                new FileStream(path, System.IO.FileMode.OpenOrCreate);
            tRange.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();
            MessageBox.Show("Saved!");
        }

        private void SettingsMemo_Click(object sender, RoutedEventArgs e)
        {
            MemoSaveSettingsWindow memoSaveSettingsWindow = new MemoSaveSettingsWindow();
            memoSaveSettingsWindow.ShowDialog();
        }

        private void LoadSearchedFile(string pathToSearchedFile)
        {
            string pathToFile = pathToSearchedFile;
            TextRange tRange = new TextRange(this.MainRichTextBox.Document.ContentStart,
                this.MainRichTextBox.Document.ContentEnd);
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

        private void BackToMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}
