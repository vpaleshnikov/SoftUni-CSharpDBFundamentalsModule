using System;

using Instagraph.Data;
using System.Linq;
using System.Xml.Linq;
using Instagraph.DataProcessor.DtoModels;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.
                Posts.
                Where(p => p.Comments.Count == 0)
                .Select(p => new
                {
                    Id = p.Id,
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .OrderBy(p => p.Id)
                .ToArray();
            
            var jsonString = JsonConvert.SerializeObject(posts, Formatting.Indented);

            return jsonString;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context
                .Users
                .Where(u => u.Posts
                                .Any(p => p.Comments
                                    .Any(c => u.Followers
                                        .Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .Select(u => new
                {
                    Username = u.Username,
                    Followers = u.Followers.Count
                })
                .ToArray();

            var jsonString = JsonConvert.SerializeObject(users, Formatting.Indented);

            return jsonString;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context
                .Users
                .Select(u => new
                {
                    u.Username,
                    PostCommentCount = u.Posts.Select(p => p.Comments.Count)
                })
                .ToArray();

            var usersDtos = new List<UserTopPostDto>();

            foreach (var user in users)
            {
                var mostComments = 0;

                if (user.PostCommentCount.Any())
                {
                    mostComments = user.PostCommentCount.OrderByDescending(c => c).First();
                }

                var userDto = new UserTopPostDto()
                {
                    Username = user.Username,
                    MostComments = mostComments
                };

                usersDtos.Add(userDto);
            }

            usersDtos = usersDtos
                .OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username)
                .ToList();

            var xDoc = new XDocument(new XElement("users"));

            foreach (var user in usersDtos)
            {
                xDoc
                    .Root
                    .Add(new XElement("user",
                            new XElement("Username", user.Username),
                            new XElement("MostComments", user.MostComments)));
            }

            return xDoc.ToString();
        }
    }
}
