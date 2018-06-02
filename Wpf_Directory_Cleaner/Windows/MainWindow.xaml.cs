using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Wpf_Directory_Cleaner.Resources;
//using Microsoft.Win32;                        //OpenFileDialog
using WinForms = System.Windows.Forms;          // FolderBrowserDialog and OpenFileDialog
using Wpf_Directory_Cleaner.Windows;


namespace Wpf_Directory_Cleaner
{
    public partial class MainWindow : Window
    {
        // object for work with files
        WinForms.OpenFileDialog SelectFile = new WinForms.OpenFileDialog();

        //object for work with directories
        WinForms.FolderBrowserDialog SelectDyrectory = new WinForms.FolderBrowserDialog();

        DirectoryCleaner dir_clean;

        public MainWindow()
        {
            InitializeComponent();
            this.SelectFile.Multiselect = true;
            this.SelectFile.Title = "Directory cleaner";
            this.ResizeMode = ResizeMode.NoResize;

            dir_clean = new DirectoryCleaner();
            DataContext = dir_clean;
        }

        private async void OpenFolder(object sender, EventArgs e)
        {
            if (SelectDyrectory.ShowDialog().ToString().Equals("OK"))
            {
                dir_clean.Empty();
                dir_clean.Path = SelectDyrectory.SelectedPath;
                await dir_clean.RecurseCountFilesAsync(dir_clean.Path);
            }
        }

        void Delete_Submit(object sender, RoutedEventArgs e)
        {
            List<Files> deletedFiles = new List<Files>();

            var listDeletedFiles = from object item in ListFiles.Items
                                   where ((Files)item).IsChecked == true
                                   select item;

            foreach (var item in listDeletedFiles)
            {
                deletedFiles.Add(item as Files);
            }

            dir_clean.DirectoryDeleteFiles(deletedFiles);
        }

        private async void SelectedDateChanged_Select(object sender, SelectionChangedEventArgs e)
        {
            if (dir_clean.Path == null)
            {
                return;
            }

            dir_clean.date = Calendar.SelectedDate.Value.Date.ToShortDateString();
            dir_clean.dateIsAll = false;

            dir_clean.ClearFileArray();
            await dir_clean.RecurseCountFilesAsync(dir_clean.Path);
        }

        public void Show_History(object sender, RoutedEventArgs e)
        {
            ShowHistory historyWindow = new ShowHistory();
            historyWindow.Owner = this;
            historyWindow.Show();
        }
    }
}

