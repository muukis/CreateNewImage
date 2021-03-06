﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Image.Calendar.Google;
using Image.Core;
using Image.Statics;
using Image.ShoppingList;
using Image.ShoppingList.Models;
using Microsoft.Extensions.Configuration;

namespace CreateNewImage
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var imageCreator = new ImageCreator();

            using (var shoppingListClient = new ShoppingListAPI(new Uri(config["shoppingListApiUrl"])))
            {
                if (shoppingListClient.GetAllShoppingListItems(config["shoppingListShopperName"]) is List<ShoppingListItemResult> items)
                {
                    foreach (var item in items)
                    {
                        imageCreator.ShoppingList.Add(item.Title);
                    }
                }
            }

            var blackDuck = System.Drawing.Image.FromFile("blackduck.png");

            ICalendarCreator creator = new CalendarCreator();
            imageCreator.CalendarItems.AddRange(creator.GetCalendarItems(20, credPath: "token.json"));
            imageCreator.CalendarItems.AddRange(creator.GetCalendarItems(20, credPath: "token2.json", ignoredCalendars: new[] {"Week numbers"}, image: blackDuck));

            foreach (var item in imageCreator.CalendarItems.ToArray())
            {
                if (imageCreator.CalendarItems.Any(o => o != item && o.Time == item.Time && o.Title == item.Title))
                {
                    imageCreator.CalendarItems.Remove(item);
                }
            }

            imageCreator.CalendarItems = imageCreator.CalendarItems.OrderBy(ci => ci.Time).Take(20).ToList();

            var waveshareImages = imageCreator.CreateBitmaps(); //.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image.bmp"));
            waveshareImages.BlackImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image-b.bmp"));
            waveshareImages.RedImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image-r.bmp"));
        }
    }
}
