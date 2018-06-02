using System;
using System.IO;
using System.Windows;

namespace Wpf_Directory_Cleaner
{
    public partial class App : Application
    {
        public App()
        {

            this.Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            // if folder for keeping json-file isn't exist then create it
            if (!Directory.Exists("History deleted files"))
            {
                Directory.CreateDirectory("History deleted files");
            }
        }
    }
}
