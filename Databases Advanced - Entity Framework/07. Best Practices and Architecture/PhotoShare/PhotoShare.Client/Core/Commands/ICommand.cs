﻿namespace PhotoShare.Client.Core.Commands
{
    public interface ICommand
    {
        string Execute(string[] data);
    }
}