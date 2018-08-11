using System;
using System.Collections.Generic;
using System.Text;

namespace Image.Core
{
    public class CalendarItem
    {
        public const string DEFAULT_FORMAT = "{0:d.M.} {1}";

        public CalendarItem() { }

        public CalendarItem(DateTime time, string title)
        {
            Time = time;
            Title = title;
        }

        public DateTime Time { get; set; }
        public string Title { get; set; }

        public string ToString(string format = null)
        {
            return string.Format(format ?? DEFAULT_FORMAT, Time, Title);
        }
    }
}
