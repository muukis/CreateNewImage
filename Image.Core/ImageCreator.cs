using System;
using System.Collections.Generic;
using System.Drawing;
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
        private const int FOOTER_FONTSIZE = 24;

        private const int HEADER_LINE_MARGINAL = 5;
        private const int HEADER_LINE_WIDTH = 3;

        private const int FOOTER_HEIGHT = 45;

        private static readonly int ITEM_ROWHEIGHT = ITEM_FONTSIZE + 4;
        private static readonly int HEADER_LINE_POSITION_Y = IMAGE_MARGINAL + HEADER_FONTSIZE + 2 * HEADER_LINE_MARGINAL + 2;
        private static readonly int DYNAMIC_START_POS_Y = HEADER_LINE_POSITION_Y + HEADER_LINE_MARGINAL;

        private static readonly int WIDTH_HALF = WIDTH / 2;
        private static readonly int FOOTER_POSITION_Y = HEIGHT - FOOTER_HEIGHT;

        private static readonly Color COLOR_BLACK = Color.FromArgb(0, 0, 0);
        private static readonly Color COLOR_WHITE = Color.FromArgb(255, 255, 255);
        private static readonly Color COLOR_RED = Color.FromArgb(237, 28, 36);

        private static readonly Brush BRUSH_BLACK = new SolidBrush(COLOR_BLACK);
        private static readonly Brush BRUSH_WHITE = new SolidBrush(COLOR_WHITE);
        private static readonly Brush BRUSH_RED = new SolidBrush(COLOR_RED);

        public ImageCreator()
        {
            ShoppingList = new List<string>();
            CalendarItems = new List<CalendarItem>();
            FontFamilyName = DEFAULT_FONTFAMILYNAME;
        }

        public string FontFamilyName { get; set; }

        public List<string> ShoppingList { get; }

        public List<CalendarItem> CalendarItems { get; set; }

        public Bitmap CreateBitmap()
        {
            var fontFamily = new FontFamily(FontFamilyName);
            var fontHeader = new Font(fontFamily, HEADER_FONTSIZE, FontStyle.Bold);
            var fontItem = new Font(fontFamily, ITEM_FONTSIZE, FontStyle.Bold);
            var fontFooter = new Font(fontFamily, FOOTER_FONTSIZE, FontStyle.Bold);

            var image = new Bitmap(WIDTH, HEIGHT);
            var graph = Graphics.FromImage(image);

            graph.Clear(COLOR_BLACK);
            graph.FillRectangle(BRUSH_WHITE, WIDTH_HALF, 0, WIDTH - WIDTH_HALF, HEIGHT);
            graph.FillRectangle(BRUSH_RED, 0, FOOTER_POSITION_Y, WIDTH, HEIGHT);

            graph.DrawLine(new Pen(BRUSH_WHITE, HEADER_LINE_WIDTH), 0, HEADER_LINE_POSITION_Y, WIDTH_HALF, HEADER_LINE_POSITION_Y);

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
                graph.DrawString(sortedShoppingList[i], fontItem, BRUSH_WHITE, new PointF(IMAGE_MARGINAL, nextY));
            }

            graph.DrawString(
                "OSTOSLISTA" + (displayedShoppingItemCount < ShoppingList.Count
                    ? $" (+{ShoppingList.Count - displayedShoppingItemCount})"
                    : string.Empty), fontHeader, BRUSH_RED, new PointF(IMAGE_MARGINAL, IMAGE_MARGINAL));

            graph.DrawString("KALENTERI", fontHeader, BRUSH_RED, new PointF(WIDTH_HALF + IMAGE_MARGINAL, IMAGE_MARGINAL));
            graph.DrawLine(new Pen(BRUSH_RED, HEADER_LINE_WIDTH), WIDTH_HALF, HEADER_LINE_POSITION_Y, WIDTH, HEADER_LINE_POSITION_Y);

            var sortedCalendarItems = CalendarItems.OrderBy(ci => ci.Time).ToList();

            for (int i = 0; i < sortedCalendarItems.Count; i++)
            {
                int nextY = DYNAMIC_START_POS_Y + i * ITEM_ROWHEIGHT;

                if (nextY + ITEM_ROWHEIGHT > FOOTER_POSITION_Y)
                {
                    break;
                }

                graph.DrawString(sortedCalendarItems[i].ToString(), fontItem, BRUSH_BLACK, new PointF(WIDTH_HALF + IMAGE_MARGINAL, nextY));
            }

            DateTime now = DateTime.Now;
            graph.DrawString(now.ToString("D").ToUpper(), fontFooter, BRUSH_BLACK, new PointF(IMAGE_MARGINAL, FOOTER_POSITION_Y + IMAGE_MARGINAL));

            return image;
        }
    }
}
