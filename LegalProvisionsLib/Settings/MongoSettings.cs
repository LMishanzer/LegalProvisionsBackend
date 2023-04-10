﻿namespace LegalProvisionsLib.Settings;

public class MongoSettings
{
    public string ConnectionUri { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string CollectionName { get; set; } = string.Empty;
}