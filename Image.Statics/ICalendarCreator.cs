using System;
using System.Collections.Generic;

namespace Image.Statics
{
    public interface ICalendarCreator
    {
        List<CalendarItem> GetCalendarItems(int count);
    }
}
