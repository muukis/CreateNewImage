using System;
using System.IO;
using Image.Core;

namespace CreateNewImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var imageCreator = new ImageCreator();

            imageCreator.ShoppingList.Add("Maitoa");
            imageCreator.ShoppingList.Add("Leipää");
            imageCreator.ShoppingList.Add("Voita");
            imageCreator.ShoppingList.Add("Vessapaperia");
            imageCreator.ShoppingList.Add("Ketsuppia");
            imageCreator.ShoppingList.Add("Perunoita");
            imageCreator.ShoppingList.Add("Olutta");
            imageCreator.ShoppingList.Add("Puurohiutaleita");

            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now, "Uimaan"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(1), "Salibandyä"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(2), "Milon matikan koe"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(3), "Auton katsastus"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(4), "Käy moikkaamassa kaveria"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(5), "Lautapelit"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(7), "Futista"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(3), "Palaveri"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(5), "Ota iisisti"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(15), "Hammaslääkäri"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(12), "Salibandyä"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(30), "Treenimatsi"));
            imageCreator.CalendarItems.Add(new CalendarItem(DateTime.Now.AddDays(16), "Bileet"));

            imageCreator.CreateBitmap().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image.bmp"));
        }
    }
}
