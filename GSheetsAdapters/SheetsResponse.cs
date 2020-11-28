using System.Collections.Generic;

namespace TarangBot.GSheetsAdapters
{
    public class SheetsResponse
    {
        public string range { get; set; }
        public string majorDimension { get; set; }
        public List<string[]> values { get; set; }
    }
}
