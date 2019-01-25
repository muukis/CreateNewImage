using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Image.Statics;

namespace Image.Core
{
    public class ImageCreator
    {
        public const int WIDTH = 640;
        public const int HEIGHT = 384;

        public const string DEFAULT_FONTFAMILYNAME = "Arial";

        private const int IMAGE_MARGINAL = 5;
        private const int HEADER_FONTSIZE = 22;
        private const int ITEM_FONTSIZE = 18;
        private const int DAY_FONTSIZE = 11;
        private const int FOOTER_FONTSIZE = 24;

        private const int HEADER_LINE_MARGINAL = 5;
        private const int HEADER_LINE_WIDTH = 3;

        private const int FOOTER_HEIGHT = 45;

        private const int ITEM_ROW_EXTRA = 4;

        private static readonly int ITEM_ROWHEIGHT = ITEM_FONTSIZE + ITEM_ROW_EXTRA;
        private static readonly int HEADER_LINE_POSITION_Y = IMAGE_MARGINAL + HEADER_FONTSIZE + 2 * HEADER_LINE_MARGINAL + 2;
        private static readonly int DYNAMIC_START_POS_Y = HEADER_LINE_POSITION_Y + HEADER_LINE_MARGINAL;

        private static readonly int WIDTH_HALF = WIDTH / 2;
        private static readonly int FOOTER_POSITION_Y = HEIGHT - FOOTER_HEIGHT;

        private static readonly Brush BRUSH_BLACK = new SolidBrush(Color.Black);
        private static readonly Brush BRUSH_WHITE = new SolidBrush(Color.White);

        public ImageCreator()
        {
            ShoppingList = new List<string>();
            CalendarItems = new List<CalendarItem>();
            FontFamilyName = DEFAULT_FONTFAMILYNAME;
        }

        public string FontFamilyName { get; set; }

        public List<string> ShoppingList { get; }

        public List<CalendarItem> CalendarItems { get; set; }

        public WaveshareImages CreateBitmaps()
        {
            var retval = new WaveshareImages();

            var fontFamily = new FontFamily(FontFamilyName);
            var fontHeader = new Font(fontFamily, HEADER_FONTSIZE, FontStyle.Bold);
            var fontItem = new Font(fontFamily, ITEM_FONTSIZE, FontStyle.Bold);
            var fontFooter = new Font(fontFamily, FOOTER_FONTSIZE, FontStyle.Bold);
            var fontDate = new Font(fontFamily, DAY_FONTSIZE, FontStyle.Bold);

            var blackGraph = Graphics.FromImage(retval.BlackImage);
            var redGraph = Graphics.FromImage(retval.RedImage);

            blackGraph.Clear(Color.White);
            redGraph.Clear(Color.White);

            blackGraph.FillRectangle(BRUSH_BLACK, 0, 0, WIDTH_HALF, FOOTER_POSITION_Y); // Left black box
            blackGraph.FillRectangle(BRUSH_BLACK, 0, FOOTER_POSITION_Y, WIDTH, HEIGHT); // Footer (black text)
            redGraph.FillRectangle(BRUSH_BLACK, 0, FOOTER_POSITION_Y, WIDTH, HEIGHT); // Footer red box

            blackGraph.DrawLine(new Pen(BRUSH_WHITE, HEADER_LINE_WIDTH), 0, HEADER_LINE_POSITION_Y, WIDTH_HALF, HEADER_LINE_POSITION_Y);

            var sortedShoppingList = ShoppingList.OrderBy(si => si).ToList();
            int displayedShoppingItemCount = 0;

            for (int i = 0; i < sortedShoppingList.Count; i++)
            {
                int nextY = DYNAMIC_START_POS_Y + i * ITEM_ROWHEIGHT;

                if (nextY + ITEM_ROWHEIGHT > FOOTER_POSITION_Y)
                {
                    break;
                }

                displayedShoppingItemCount++;
                blackGraph.DrawString(sortedShoppingList[i], fontItem, BRUSH_WHITE, new PointF(IMAGE_MARGINAL, nextY));
                //redGraph.DrawString(sortedShoppingList[i], fontItem, BRUSH_BLACK, new PointF(IMAGE_MARGINAL, nextY));
            }

            string ostoslistaHeader = "OSTOSLISTA" + (displayedShoppingItemCount < ShoppingList.Count
                                          ? $" (+{ShoppingList.Count - displayedShoppingItemCount})"
                                          : string.Empty);

            //blackGraph.DrawString(ostoslistaHeader, fontHeader, BRUSH_WHITE, new PointF(IMAGE_MARGINAL, IMAGE_MARGINAL));
            redGraph.DrawString(ostoslistaHeader, fontHeader, BRUSH_BLACK, new PointF(IMAGE_MARGINAL, IMAGE_MARGINAL));

            redGraph.DrawString("KALENTERI", fontHeader, BRUSH_BLACK, new PointF(WIDTH_HALF + IMAGE_MARGINAL, IMAGE_MARGINAL));
            redGraph.DrawLine(new Pen(BRUSH_BLACK, HEADER_LINE_WIDTH), WIDTH_HALF, HEADER_LINE_POSITION_Y, WIDTH, HEADER_LINE_POSITION_Y);

            var sortedCalendarItems = CalendarItems.OrderBy(ci => ci.Time).ToList();
            CalendarItem lastCalendarItem = null;
            Graphics calendarItemCanvas = redGraph;
            var finnishCulture = CultureInfo.GetCultureInfo("fi-fi");
            SizeF graphSize = blackGraph.VisibleClipBounds.Size;

            for (int i = 0; i < sortedCalendarItems.Count; i++)
            {
                int nextY = DYNAMIC_START_POS_Y + i * ITEM_ROWHEIGHT;

                if (nextY + ITEM_ROWHEIGHT > FOOTER_POSITION_Y)
                {
                    break;
                }

                var currentCalendarItem = sortedCalendarItems[i];

                string dateText = currentCalendarItem.Time.ToString("ddd", finnishCulture).ToUpper();
                SizeF dateTextSize = calendarItemCanvas.MeasureString(dateText, fontDate);
                float dateTextPosX = graphSize.Height - nextY - ITEM_FONTSIZE - (2 * ITEM_ROW_EXTRA);
                float dateTextMovedPosX = dateTextPosX + ((ITEM_ROWHEIGHT - dateTextSize.Width) / 2);

                if (lastCalendarItem?.Time.Date != currentCalendarItem.Time.Date)
                {
                    calendarItemCanvas = (calendarItemCanvas == blackGraph ? redGraph : blackGraph);

                    calendarItemCanvas.TranslateTransform(0, graphSize.Height);
                    calendarItemCanvas.RotateTransform(-90);
                    calendarItemCanvas.DrawString(dateText, fontDate, BRUSH_BLACK, new PointF(dateTextMovedPosX, WIDTH_HALF + 2));

                    calendarItemCanvas.TranslateTransform(graphSize.Height, 0);
                    calendarItemCanvas.RotateTransform(90);
                }

                if (currentCalendarItem.Image != null)
                {
                    string calendarItemDateText = string.Format(CalendarItem.DEFAULT_DATETIME_FORMAT, currentCalendarItem.Time);
                    SizeF calendarItemDateTextSize = calendarItemCanvas.MeasureString(calendarItemDateText, fontItem);
                    float currentX = WIDTH_HALF + IMAGE_MARGINAL + (dateTextSize.Height / 2);
                    calendarItemCanvas.DrawString(calendarItemDateText, fontItem, BRUSH_BLACK, new PointF(currentX, nextY));
                    currentX += calendarItemDateTextSize.Width - 4;
                    blackGraph.DrawImage(currentCalendarItem.Image, new PointF(currentX, nextY + 4));
                    currentX += currentCalendarItem.Image.Width - 3;
                    calendarItemCanvas.DrawString(currentCalendarItem.Title, fontItem, BRUSH_BLACK, new PointF(currentX, nextY));
                }
                else
                {
                    calendarItemCanvas.DrawString(currentCalendarItem.ToString(), fontItem, BRUSH_BLACK, new PointF(WIDTH_HALF + IMAGE_MARGINAL + (dateTextSize.Height / 2), nextY));
                }

                lastCalendarItem = currentCalendarItem;
            }

            string footer = DateTime.Now.ToString("D", CultureInfo.GetCultureInfo("fi-fi")).ToUpper();

            //blackGraph.DrawString(footer, fontFooter, BRUSH_BLACK, new PointF(IMAGE_MARGINAL, FOOTER_POSITION_Y + IMAGE_MARGINAL));
            redGraph.DrawString(footer, fontFooter, BRUSH_WHITE, new PointF(IMAGE_MARGINAL, FOOTER_POSITION_Y + IMAGE_MARGINAL));

            return retval;
        }
    }
}
