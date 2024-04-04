using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Log
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Log(string text)
        {
            Text = text;
            Date = DateTime.Now;
        }
    }
}
