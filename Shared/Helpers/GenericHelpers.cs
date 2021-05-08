using Shared.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Shared.Helpers {
    public static class GenericHelpers {

        public static GameMode GameModeToEnum(string gameModeString)
        {
            if (string.IsNullOrEmpty(gameModeString))
            {
                throw new ArgumentOutOfRangeException("Illegal gameMode string");
            }

            gameModeString = gameModeString.ToUpperInvariant();
            switch (gameModeString)
            {
                case "UNKNOWN":
                case "NONE":
                case "CONQUESTSMALL":
                case "CONQUEST":
                    return GameMode.CONQUEST;
                case "RUSH":
                case "RUSHLARGE":
                case "RUSHSMALL":
                    return GameMode.RUSH;
                case "SQDM":
                case "SQUADDEATHMATCH":
                    return GameMode.SQDM;
                case "SQRUSH":
                case "SQUADRUSH":
                    return GameMode.SQRUSH;
                case "CARRIERASSAULT":
                case "CARRIERASSAULTLARGE":
                case "CARRIERASSAULTSMALL":
                    return GameMode.CARRIERASSAULT;
                default:
                    if (Enum.TryParse<GameMode>(gameModeString, out var gameMode))
                    {
                        return gameMode;
                    }
                    throw new ArgumentOutOfRangeException("Unknown gameMode: " + gameModeString);
            }
        }

        public static string GameServerRoleTypeToBlazeString(string gameServerRoleType)
        {
            if (string.IsNullOrEmpty(gameServerRoleType))
            {
                throw new ArgumentOutOfRangeException("Illegal RoleType enum");
            }

            return gameServerRoleType switch {
                nameof(RoleType.SOLDIER) => "soldier",
                nameof(RoleType.COMMANDER) => "commander",
                nameof(RoleType.SPECTATOR) => "spectator",
                _ => throw new ArgumentOutOfRangeException("Unknown RoleType: " + gameServerRoleType),
            };
        }

        public static IEnumerable<List<T>> SplitList<T>(this List<T> items, int nSize = 20) {
            for (int i = 0; i < items.Count; i += nSize) {
                yield return items.GetRange(i, Math.Min(nSize, items.Count - i));
            }
        }

        public static string Combine(string path1, string path2, string path3, string path4) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return Path.Combine(path1, path2, path3, path4).Replace("\\", "/");
            }
            else {
                return Path.Combine(path1, path2, path3, path4);
            }
        }
        public static string Combine(params string[] paths) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return Path.Combine(paths).Replace("\\", "/");
            }
            else {
                return Path.Combine(paths);
            }
        }
        public static string Combine(string path1, string path2) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return Path.Combine(path1, path2).Replace("\\", "/");
            }
            else {
                return Path.Combine(path1, path2);
            }
        }
        public static string Combine(string path1, string path2, string path3) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                return Path.Combine(path1, path2, path3).Replace("\\", "/");
            }
            else {
                return Path.Combine(path1, path2, path3);
            }
        }
    }
}
