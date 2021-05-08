using System;

namespace Shared.Enums
{
    [Flags]
    public enum GameMode : ulong
    {
        CONQUEST = 1L << 0,                              // 1
        RUSH = 1L << 1,                                  // 2
        SQRUSH = 1L << 2,                                // 4
        SQDM = 1L << 3,                                  // 8
        ONSLAUGHT = 1L << 4,                             // 16 - In which game?
        TEAMDEATHMATCH = 1L << 5,                        // 32
        CONQUESTLARGE = 1L << 6,                         // 64
        CONQUESTASSAULTLARGE = 1L << 7,                  // 128
        CONQUESTASSAULTSMALL = 1L << 8,                  // 256
        GUNMASTER = 1L << 9,                             // 512
        DOMINATION = 1L << 10,                           // 1024
        TEAMDEATHMATCHC = 1L << 11,                      // 2048
        TEAMDEATHMATCH_FIRETEAM = 1L << 12,              // 4096 - In which game?
        COMBATMISSION = 1L << 13,                        // 8192 - In which game?
        SECTORCONTROL = 1L << 14,                        // 16384 - In which game?
        FIRETEAM_SURVIVOR = 1L << 15,                    // 32768 - In which game?
        SPORT = 1L << 16,                                // 65536 - In which game?
        TANKSUPERIORITY = 1L << 17,                      // 131072
        OBJECTIVERAID = 1L << 18,                        // 262144 - In which game?
        CAPTURETHEFLAG = 1L << 19,                       // 524288 - Capture The Bag in BFH
        BOMBSQUAD = 1L << 20,                            // 1048576 - In which game?
        OBLITERATION = 1L << 21,                         // 2097152
        SCAVENGER = 1L << 22,                            // 4194304
        AIRSUPERIORITY = 1L << 23,                       // 8388608
        ELIMINATION = 1L << 24,                          // 1677216
        CARRIERASSAULT = 1L << 25,                       // 33554432 - In which game?
        CARRIERASSAULTLARGE = 1L << 26,                  // 67108864
        CARRIERASSAULTSMALL = 1L << 27,                  // 134217728
        BLOODMONEY = 1L << 28,                           // 268435456 - BFH
        TURFWARSMALL = 1L << 29,                         // 536870912 - BFH
        TURFWARLARGE = 1L << 30,                         // 1073741824 - BFH
        HEIST = 1L << 31,                                // 2147483648 - BFH
        HOTWIRE = 1L << 32,                              // 4294967296 - BFH
        HIT = 1L << 33,                                  // 8589934592 - BFH
        HOSTAGE = 1L << 34,                              // 17179869184 - BFH
        CHAINLINK = 1L << 35,                            // 34359738368
        CONQUESTLADDER = 1L << 36,                       // 68719476736 - In which game?
        SQUADOBLITERATION = 1L << 37,                    // 137438953472
        BountyHunter = 1L << 38,                         // 274877906944
        SquadHeist = 1L << 39,                           // 549755813888
    }
}
