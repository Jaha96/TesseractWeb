using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesseractWeb.Models
{
    public class FileModel
    {
        public int historyId { get; set; }
        public string outputFile { get; set; }
        public string inputFile { get; set; }
        public DateTime date { get; set; }
        public int userId { get; set; }
    }
}