using System.Windows;
using Wpf_Directory_Cleaner.Resources;

namespace Wpf_Directory_Cleaner.Windows
{
    public partial  class ShowHistory : Window
    {
        public HistoryJson historyJson;

        public ShowHistory()
        {
            InitializeComponent();

            historyJson = new HistoryJson();
            historyJson.GetHistory();
            this.DataContext = historyJson;
        }

        public void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    

}
