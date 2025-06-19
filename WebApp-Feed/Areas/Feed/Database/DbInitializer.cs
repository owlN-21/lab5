using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApp_Feed.Database;
using WebApp_Feed.Models;

namespace tutorialRazorPages.DataBase
{
    public class DbInitializer
    {
        public static byte[] DateTimeToByteArray(DateTime dateTime)
        {
            long dateTimeBinary = dateTime.ToBinary();
            return BitConverter.GetBytes(dateTimeBinary);
        }

        public static DateTime ByteArrayToDateTime(byte[] byteArray)
        {
            long dateTimeBinary = BitConverter.ToInt64(byteArray, 0);
            return DateTime.FromBinary(dateTimeBinary);
        }
        public static void Initialize(GreenswampContext context)
        {
            if (!context.Users.Any())
            {
                var user = new User
                {
                    Username = "username",
                    DisplayName = "User Name",
                    Bio = "This is a test user.",
                    AvatarUrl = "https://i.pravatar.cc/100?u=dorm23_frogs@pravatar.com"
                };
                context.Users.Add(user);
                context.SaveChanges();
            }

            if (!context.Posts.Any())
            {
                var posts = new List<Post>
        {
            new Post
            {
                PostId = 1,
                UserId = 1,
                Content = "Hello",
                CreatedAt = DateTimeToByteArray(DateTime.UtcNow),
                MediaType = "image",
                MediaUrl = "https://i.pravatar.cc/100?u=dorm23_frogs@pravatar.com",
                PostType = "text"
            },
            new Post
            {
                PostId = 2,
                UserId = 1,
                Content = "Image!",
                CreatedAt = DateTimeToByteArray(DateTime.UtcNow.AddMinutes(-5).AddHours(-5).AddDays(-2)),
                MediaType = "image", 
                MediaUrl = "https://i.ytimg.com/vi/8HGGZ1JnE5Y/maxresdefault.jpg",
                PostType = "image"
            }
        };

                context.Posts.AddRange(posts);
                context.SaveChanges();
            }

            if (!context.Events.Any())
            {
                var events = new List<Event>
        {
            new Event
            {
                EventId = 1,
                PostId = 1,
                EventTime = DateTimeToByteArray(DateTime.UtcNow.AddDays(1)),
                HostOrg = "Greenswamp Society",
                Location = "Pond Park",
                MaxCapacity = 50,
                RsvpCount = 0
            },
            new Event
            {
                EventId = 2,
                PostId = 2,
                EventTime = DateTimeToByteArray(DateTime.UtcNow.AddDays(-12)),
                HostOrg = "Frog Fest",
                Location = "Lily Pad Plaza",
                MaxCapacity = 100,
                RsvpCount = 0
            }
        };

                context.Events.AddRange(events);
                context.SaveChanges();
            }
        }
    }
}