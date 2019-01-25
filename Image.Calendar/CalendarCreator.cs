using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Image.Statics;

namespace Image.Calendar.Google
{
    public class CalendarCreator : ICalendarCreator
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        private static readonly string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        private const string ApplicationName = "Google Calendar API .NET Quickstart";

        public List<CalendarItem> GetCalendarItems(int count, string rootPath = null, string credPath = "token.json", string[] ignoredCalendars = null, System.Drawing.Image image = null)
        {
            var calendarItems = new List<CalendarItem>(count);

            using (var service = CreateCalendarService(rootPath, credPath))
            {
                var calendarsRequest = service.CalendarList.List();
                var calendars = calendarsRequest.Execute();
                var selectedCalendars = calendars.Items.Where(c =>
                    (c.Selected ?? false) && (ignoredCalendars == null || !ignoredCalendars.Any(o =>
                                                  string.Equals(o, c.Summary,
                                                      StringComparison.InvariantCultureIgnoreCase)))).ToList();

                foreach (var calendarListEntry in selectedCalendars)
                {
                    Console.WriteLine($"Calendar: {calendarListEntry.Summary}");
                    // Define parameters of request.
                    EventsResource.ListRequest request = service.Events.List(calendarListEntry.Id);
                    request.TimeMin = DateTime.Today;
                    request.ShowDeleted = false;
                    request.SingleEvents = true;
                    request.MaxResults = count;
                    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                    // List events.
                    Events events = request.Execute();
                    Console.WriteLine("Upcoming events:");
                    if (events.Items != null && events.Items.Count > 0)
                    {
                        foreach (var eventItem in events.Items)
                        {
                            DateTime when = eventItem.Start.DateTime ?? DateTime.Parse(eventItem.Start.Date);
                            Console.WriteLine($"{eventItem.Summary} ({when:M})", eventItem.Summary, when);
                            calendarItems.Add(new CalendarItem(when, eventItem.Summary, image));
                        }
                    }
                    else
                    {
                        Console.WriteLine("No upcoming events found.");
                    }
                }
            }

            return calendarItems.Where(o => o.Time.Date >= DateTime.Today).OrderBy(o => o.Time).Take(count).ToList();
        }

        private UserCredential CreateUserCredentials(string rootPath = null, string credPath = "token.json")
        {
            UserCredential credential;
            string credentialsPath = "credentials.json";

            if (rootPath != null)
            {
                credentialsPath = Path.Combine(rootPath, credentialsPath);
            }

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                if (rootPath != null)
                {
                    credPath = Path.Combine(rootPath, credPath);
                }

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            return credential;
        }

        private CalendarService CreateCalendarService(string rootPath = null, string credPath = "token.json")
        {
            // Create Google Calendar API service.
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = CreateUserCredentials(rootPath, credPath),
                ApplicationName = ApplicationName,
            });
        }
    }
}
