using System;

namespace Image.Statics
{
    public class CalendarItem
    {
        public const string DEFAULT_DATETIME_FORMAT = "{0:d.M.}";

        public CalendarItem() { }

        public CalendarItem(DateTime time, string title, System.Drawing.Image image)
        {
            Time = time;
            Title = title;
            Image = image;
        }

        public DateTime Time { get; set; }
        public string Title { get; set; }
        public System.Drawing.Image Image { get; set; }

        public string ToString(string format = null)
        {
            return string.Format(format ?? DEFAULT_DATETIME_FORMAT, Time) + $" {Title}";
        }
    }
}
