using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;


namespace Wpf_Directory_Cleaner.Resources
{
        public class HistoryJson
        {
            public ObservableCollection<HistoryModel> History { set; get; }

            public HistoryJson()
            {
                History = new ObservableCollection<HistoryModel>();
            }

            public void GetHistory()
            {
                HistoryModel modelView = new HistoryModel();
                string jsonString;

                try
                {
                    using (StreamReader sw = new StreamReader("History deleted files/history.json", System.Text.Encoding.Default))
                    {
                        jsonString = sw.ReadToEnd();
                    }
                }
                catch (FileNotFoundException)
                {
                    return;
                }

                // editing JSON-objects for next work with it
                int len = jsonString.Length;
                jsonString = jsonString.Substring(0, len - 1);   // delete end ','
                jsonString = jsonString.Insert(0, "[");
                jsonString = jsonString.Insert(len - 1, "]");

                var jR = JsonConvert.DeserializeObject<List<JsonRecord>>(jsonString);
                
                jR.Reverse();

                foreach (var item in jR)
                {
                    History.Add(new HistoryModel
                    {
                        Title = string.Join(",\n", item.title),
                        WindowsAccount = item.windowsAccount,
                        Date = item.date
                    });
                }
            }
        }
    
}