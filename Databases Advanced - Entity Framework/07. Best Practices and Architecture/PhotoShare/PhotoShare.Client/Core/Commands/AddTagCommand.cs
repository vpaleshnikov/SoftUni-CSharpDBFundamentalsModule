﻿namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using Utilities;

    public class AddTagCommand : ICommand
    {
        public string Execute(string[] data)
        {
            string tag = data[0].ValidateOrTransform();

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (Session.User == null)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }

                if (context.Tags.SingleOrDefault(t => t.Name == tag) != null)
                {
                    throw new ArgumentException($"Tag {tag} exists!");

                }

                context.Tags.Add(new Tag
                {
                    Name = tag
                });

                context.SaveChanges();
            }

            return $"Tag {tag} was added successfully to database!";
        }
    }
}
