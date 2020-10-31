using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TarangBot.GSheetsAdapters
{
    public class GSheetAdapter
    {
        private string Sheet_Id;

        private string API_key;

        private static HttpClient httpClient;

        public Action<string> Log;

        static GSheetAdapter()
        {
            httpClient = new HttpClient();
        }

        public GSheetAdapter(string sheet_id,string API_key)
        {
            Sheet_Id = sheet_id;
            this.API_key = API_key;
        }



        public async Task Poll()
        {
            DateTime tp = DateTime.Now;
            Log($"Polling sheets file");
            string get = await httpClient.GetStringAsync($"https://sheets.googleapis.com/v4/spreadsheets/{Sheet_Id}/values/A:Z?key={API_key}");
            File.WriteAllText("./test.txt", get);

            Log?.Invoke($"Polling took {(DateTime.Now - tp).TotalMilliseconds} ms");
        }
    }
}
