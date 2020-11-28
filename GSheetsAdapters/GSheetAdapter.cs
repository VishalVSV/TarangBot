using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TarangBot.TarangEvent;

namespace TarangBot.GSheetsAdapters
{
    public class GSheetAdapter
    {
        public string Sheet_Id;
        public string SheetName;
        public string API_key;

        [JsonIgnore]
        private static HttpClient httpClient;

        [JsonIgnore]
        public Action<string> Log;

        public List<string[]> records = new List<string[]>();

        [JsonIgnore]
        public Action<Registration> OnNewRecord;

        public int ProcessedRecords = 0;

        static GSheetAdapter()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        public GSheetAdapter(string sheet_id, string API_key)
        {
            Sheet_Id = sheet_id;
            this.API_key = API_key;
        }



        public async Task Poll()
        {
            Tarang.Data.StatusDisp["Poll Status"] = "Polling";
            DateTime tp = DateTime.Now;
            try
            {
                string get = await httpClient.GetStringAsync($"https://sheets.googleapis.com/v4/spreadsheets/{Sheet_Id}/values/\'{SheetName}\'!A3:DA?key={API_key}");
                Tarang.Data.StatusDisp["Poll Status"] = $"Last Poll took {Math.Round((DateTime.Now - tp).TotalMilliseconds, 2)}ms";

                SheetsResponse s = JsonConvert.DeserializeObject<SheetsResponse>(get);

                if (s.values != null)
                    if (s.values.Count > ProcessedRecords)
                    {
                        for (int i = ProcessedRecords; i < s.values.Count; i++)
                        {
                            ProcessedRecords++;
                            if (s.values[i].Length > 0)
                            {
                                records.Add(s.values[i]);
                                OnNewRecord?.Invoke(new Registration(s.values[i]));
                            }
                        }
                    }
            }
            catch (Exception e)
            {
                Log("Poll failed because of \"" + e.Message + "\"");
                Tarang.Data.SendDiscordLog("Poll failed because of \"" + e.Message + "\"");
                Tarang.Data.LastError = "@GSheet integration: [" + e.Message + "]";

                await Tarang.Data.TarangBot.UpdateDashboard();
            }
        }
    }
}
