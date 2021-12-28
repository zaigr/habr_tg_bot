﻿namespace HabrTelegramBot.Data.Entities;

public class ChannelItemIdentity
{
    internal ChannelItemIdentity(string itemLink)
    {
        ItemLink = itemLink;
    }

    public string ItemLink { get; }
}
