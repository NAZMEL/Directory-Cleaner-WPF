using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wpf_Directory_Cleaner.Resources
{
    /// <summary>
    /// Model for work with JSON
    /// </summary>
    class JsonRecord
    {
        [JsonProperty(PropertyName = "date")]
        public string date = Convert.ToString(DateTime.Now);

        [JsonProperty(PropertyName = "windowsAccount")]
        public string windowsAccount = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

        [JsonProperty(PropertyName = "title")]
        public string[] title { set; get; } 
    }

    /// <summary>
    ///  Model for work with array
    /// </summary>
    public class Files : INotifyPropertyChanged
    {
        string title { set; get; }
        string path { set; get; }
        DateTime dateChange { set; get; }
        bool isChecked { get; set; }

        public Files()
        {
            IsChecked = false;
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
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
        public DateTime DateChange
        {
            get => dateChange;
            set
            {
                dateChange = value;
                OnPropertyChanged("Date_Change");
            }
        }

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    /// <summary>
    /// Model for History deleted files
    /// /// </summary>
    public class HistoryModel : INotifyPropertyChanged
    {
        string title { set; get; }
        string windowsAccount { set; get; }
        string date { set; get; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public string WindowsAccount
        {
            get => windowsAccount;
            set
            {
                windowsAccount = value;
                OnPropertyChanged("WindowsAccount");
            }
        }
        public string Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
