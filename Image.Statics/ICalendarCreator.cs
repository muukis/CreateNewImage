using System;
using System.Collections.Generic;

namespace Image.Statics
{
    public interface ICalendarCreator
    {
        List<CalendarItem> GetCalendarItems(int count, string rootPath = null, string credPath = "token.json", string[] ignoredCalendars = null);
    }
}
