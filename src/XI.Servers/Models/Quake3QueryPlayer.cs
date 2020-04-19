﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XI.Servers.Interfaces;
using XI.Servers.Interfaces.Models;

namespace XI.Servers.Models
{
    internal class Quake3QueryPlayer : IQueryPlayer
    {
        public int Ping { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }

        public string NormalizedName
        {
            get
            {
                var toRemove = new List<string> {"^0", "^1", "^2", "^3", "^4", "^5", "^6", "^7", "^8", "^9"};

                var toReturn = Name.ToUpper();
                toReturn = toRemove.Aggregate(toReturn, (current, val) => current.Replace(val, ""));

                if (toReturn.StartsWith("["))
                {
                    var regex = new Regex("^(\\[.*\\])");
                    var match = regex.Match(toReturn);

                    if (match.Success)
                    {
                        var matchedTag = match.Groups[1];
                        toReturn = toReturn.Replace(matchedTag.ToString(), "");
                    }
                }

                toReturn = toReturn.Trim();
                return toReturn;
            }
        }
    }
}