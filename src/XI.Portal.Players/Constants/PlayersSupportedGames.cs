﻿using System.Collections.Generic;
using XI.CommonTypes;

namespace XI.Portal.Players.Constants
{
    public static class PlayersSupportedGames
    {
        public static IEnumerable<GameType> Games
        {
            get
            {
                yield return GameType.CallOfDuty2;
                yield return GameType.CallOfDuty4;
                yield return GameType.CallOfDuty5;
                yield return GameType.Insurgency;
            }
        }
    }
}