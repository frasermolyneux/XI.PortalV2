﻿using System;
using Newtonsoft.Json;

namespace XI.Forums.Models
{
    public class Category
    {
        [JsonProperty("id")] public long Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("url")] public Uri Url { get; set; }
        [JsonProperty("class")] public string Class { get; set; }
        [JsonProperty("permissions")] public Permissions Permissions { get; set; }
    }
}