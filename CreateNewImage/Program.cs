using System;
using System.IO;
using Image.Calendar.Google;
using Image.Core;
using Image.Statics;

namespace CreateNewImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageCreator = new ImageCreator();

            imageCreator.ShoppingList.Add("Maitoa");
            imageCreator.ShoppingList.Add("Leipää");
            imageCreator.ShoppingList.Add("Voita");
            imageCreator.ShoppingList.Add("Vessapaperia");
            imageCreator.ShoppingList.Add("Ketsuppia");
            imageCreator.ShoppingList.Add("Perunoita");
            imageCreator.ShoppingList.Add("Olutta");
            imageCreator.ShoppingList.Add("Puurohiutaleita");

            ICalendarCreator creator = new CalendarCreator();
            imageCreator.CalendarItems.AddRange(creator.GetCalendarItems(20));

            imageCreator.CreateBitmap().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image.bmp"));
        }
    }
}
