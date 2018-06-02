using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Wpf_Directory_Cleaner.Resources
{
    // Why your naming is not consistent? It should be PascalCase, e.g. DirectoryCleaner
    class DirectoryCleaner: INotifyPropertyChanged
    {
        string path { set; get; }
        int countFiles { set; get; }
        ulong countBytes { set; get; }
        string lastWriteTime { get; set; }

        public string date { get; set; }
        public bool dateIsAll { get; set; }

        // MAIN LIST
        public ObservableCollection<Files> FilesArray { get; set; }

        public int CountFiles
        {
            get => countFiles;
            set
            {
                countFiles = value;
                OnPropertyChanged("CountFiles");
            }
        }

        public ulong CountBytes
        {
            get => countBytes;
            set
            {
                countBytes = value;
                OnPropertyChanged("CountBytes");
            }
        }

        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        public DirectoryCleaner()
        {
            FilesArray = new ObservableCollection<Files>();
            CountFiles = 0;                   
            CountBytes = 0;

            // these variables are necessary
            date = "all";
            dateIsAll = true;
        }

        // asynchronous recursive function for count files and size
        public async Task RecurseCountFilesAsync(string _path)
        {
            string[] files = Directory.GetFileSystemEntries(_path);

            foreach (string file_path in files)
            {
                if (Directory.Exists(file_path))
                {
                    await RecurseCountFilesAsync(file_path);
                }
                if (File.Exists(file_path))
                {
                    FileInfo f = new FileInfo(file_path);

                    if (dateIsAll == true)
                    {
                        CountBytes += (ulong)((f.Length / 1024) / 1024);   // converting in megabytes
                        CountFiles++;
                    }

                    lastWriteTime = f.LastWriteTime.ToShortDateString();

                    // if value was taken from calendar
                    if ((string.Compare(lastWriteTime, date, true) == 0) && dateIsAll == false)
                    {
                        FilesArray.Add(new Files
                        {
                            Title = f.Name,
                            DateChange = f.LastWriteTime,
                            Path = file_path
                        });
                    }
                    else if ((string.Compare(lastWriteTime, date, true) != 0) && dateIsAll == true)
                    {
                        FilesArray.Add(new Files
                        {
                            Title = f.Name,
                            DateChange = f.LastWriteTime,
                            Path = file_path
                        });
                    }
                }
            }
        }


        public void DirectoryDeleteFiles(List<Files> deleteFiles)
        {
            JsonRecord jR = new JsonRecord();

            List<string> titlesList = new List<string>();

            foreach (Files item in deleteFiles)
            {
                Files file = FilesArray.Where(f => f.Title == item.Title && f.Path == item.Path).FirstOrDefault();
                if (file != null)
                {
                    FilesArray.Remove(FilesArray.First(f => f.Title == file.Title && f.Path == file.Path));

                    File.SetAttributes(file.Path, FileAttributes.Normal);    // <= IMPORTANT: if user account doesn't have any access to the file 
                    titlesList.Add(file.Title);
                    File.Delete(file.Path);
                }
            }

            jR.title = titlesList.ToArray();
            jR.date = Convert.ToString(DateTime.Now);
            jR.windowsAccount = System.Security.Principal.WindowsIdentity.GetCurrent().Name;  // <= Windows account

            string json_string = JsonConvert.SerializeObject(jR, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter("History deleted files/history.json", true, System.Text.Encoding.Default))
            {
                sw.WriteLine(json_string);
                sw.Write(',');
            }
        }

        public void ClearFileArray()
        {
            FilesArray.Clear();
        }

        public void Empty()
        { 
            FilesArray.Clear();
            CountBytes = 0; CountFiles = 0;
            date = "all"; dateIsAll = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
