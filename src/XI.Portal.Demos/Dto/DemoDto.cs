﻿using System;

namespace XI.Portal.Demos.Dto
{
    public class DemoDto
    {
        public Guid DemoId { get; set; }
        public string Game { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public string Map { get; set; }
        public string Mod { get; set; }
        public string GameType { get; set; }
        public string Server { get; set; }
        public long Size { get; set; }

        [Obsolete("This links back to the AspNetUsers")]
        public string UserId { get; set; }
        public string UploadedBy { get; set; }
    }
}