using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.Models;
using Instagraph.DataProcessor.DtoModels;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var pictures = JsonConvert.DeserializeObject<Picture[]>(jsonString);

            var validPictures = new List<Picture>();
            var sb = new StringBuilder();

            foreach (var picture in pictures)
            {
                var isEmpty = String.IsNullOrWhiteSpace(picture.Path);
                var isNotUnique = validPictures.Any(p => p.Path == picture.Path);
                var biggerThanZero = picture.Size <= 0;

                if ((isNotUnique || isEmpty) || biggerThanZero)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                validPictures.Add(picture);
                sb.AppendLine($"Successfully imported Picture {picture.Path}.");
            }

            context.Pictures.AddRange(validPictures);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var users = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            var validUsers = new List<User>();
            var sb = new StringBuilder();

            foreach (var currentUser in users)
            {
                var validPictures = context.Pictures.Select(p => p.Path).Any(p => p == currentUser.ProfilePicture);

                if (string.IsNullOrWhiteSpace(currentUser.Password) || currentUser.Password.Length > 20 ||
                    string.IsNullOrWhiteSpace(currentUser.Username) || currentUser.Username.Length > 30 ||
                    !validPictures)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                sb.AppendLine($"Successfully imported User {currentUser.Username}.");

                var user = new User
                {
                    Username = currentUser.Username,
                    Password = currentUser.Password,
                    ProfilePicture = new Picture { Path = currentUser.ProfilePicture }
                };

                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var usersFollowersDto = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);

            var validUsersFollowers = new List<UserFollower>();
            var sb = new StringBuilder();

            foreach (var dto in usersFollowersDto)
            {
                var userId = context.Users.FirstOrDefault(u => u.Username == dto.User)?.Id;
                var followerId = context.Users.FirstOrDefault(u => u.Username == dto.Follower)?.Id;

                if (userId == null || followerId == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                if (validUsersFollowers.Any(u => u.UserId == userId && u.FollowerId == followerId))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                sb.AppendLine($"Successfully imported Follower {dto.Follower} to User {dto.User}.");

                var userFollower = new UserFollower
                {
                    UserId = userId.Value,
                    FollowerId = followerId.Value
                };

                validUsersFollowers.Add(userFollower);
            }

            context.UsersFollowers.AddRange(validUsersFollowers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);

            var validPosts = new List<Post>();
            var sb = new StringBuilder();

            foreach (var element in xDoc.Root.Elements())
            {
                var caption = element.Element("caption")?.Value;
                var username = element.Element("user")?.Value;
                var picturePath = element.Element("picture")?.Value;

                if (string.IsNullOrWhiteSpace(caption) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(picturePath))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var userId = context.Users.FirstOrDefault(u => u.Username == username)?.Id;
                var pictureId = context.Pictures.FirstOrDefault(p => p.Path == picturePath)?.Id;

                if (userId == null || pictureId == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                sb.AppendLine($"Successfully imported Post {caption}.");

                var post = new Post()
                {
                    Caption = caption,
                    UserId = userId.Value,
                    PictureId = pictureId.Value
                };

                validPosts.Add(post);
            }

            context.Posts.AddRange(validPosts);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);

            var validComments = new List<Comment>();
            var sb = new StringBuilder();

            foreach (var element in xDoc.Root.Elements())
            {
                var content = element.Element("content")?.Value;
                var username = element.Element("user")?.Value;
                var postIdString = element.Element("post")?.Attribute("id")?.Value;

                if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(postIdString))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                var postId = context.Posts.FirstOrDefault(p => p.Id == int.Parse(postIdString))?.Id;
                var userId = context.Users.FirstOrDefault(u => u.Username == username)?.Id;

                if (userId == null || postId == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }

                sb.AppendLine($"Successfully imported Comment {content}.");

                var comment = new Comment()
                {
                    Content = content,
                    PostId = postId.Value,
                    UserId = userId.Value
                };

                validComments.Add(comment);
            }

            context.Comments.AddRange(validComments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }
    }
}
