﻿namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;

    public class AddFriendCommand : ICommand
    {
        public string Execute(string[] data)
        {
            var requesterUsername = data[0];
            var addedFriendUsername = data[1];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null)
                {
                    throw new ArgumentException("Invalid credentials!");
                }

                var requestingUser =
                    context
                    .Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .SingleOrDefault(u => u.Username == requesterUsername);
                if (requestingUser == null)
                {
                    throw new ArgumentException($"{requesterUsername} not found!");
                }

                var addedFriend =
                    context
                    .Users
                    .Include(u => u.FriendsAdded)
                        .ThenInclude(fa => fa.Friend)
                    .SingleOrDefault(u => u.Username == addedFriendUsername);

                if (addedFriend == null)
                {
                    throw new ArgumentException($"{addedFriendUsername} not found!");
                }

                bool alreadyAdded = requestingUser.FriendsAdded.Any(u => u.Friend == addedFriend);

                bool accepted = addedFriend.FriendsAdded.Any(u => u.Friend == requestingUser);

                if (alreadyAdded && !accepted)
                {
                    throw new InvalidOperationException("Friend request already sent!");
                }

                if (alreadyAdded && accepted)
                {
                    throw new InvalidOperationException($"{addedFriendUsername} is already a friend to {requesterUsername}");
                }

                if (!alreadyAdded && accepted)
                {
                    throw new InvalidOperationException($"{requesterUsername} has already received a friend request from {addedFriendUsername}");
                }

                requestingUser
                    .FriendsAdded.Add(new Friendship
                    {
                        User = requestingUser,
                        Friend = addedFriend
                    });

                context.SaveChanges();

                return $"Friend {addedFriendUsername} added to {requesterUsername}";
            }
        }
    }
}
