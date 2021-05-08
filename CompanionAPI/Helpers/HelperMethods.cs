using CompanionAPI.Models;
using Shared.Helpers;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CompanionAPI.Helpers
{
    public static class HelperMethods
    {
        public static BattlelogPlayerCountsViewModel CompanionPlayerCountsToBattlelog(this ServerDetailsViewModel serverDetails)
        {
            return new BattlelogPlayerCountsViewModel {
                Map = serverDetails.MapName,
                MapMode = (ulong)GenericHelpers.GameModeToEnum(serverDetails.MapMode),
                Players = 0,
                Queued = 0,
                Slots = new BattlelogSlotTypesViewModel {
                    Queue = new BattlelogSlotsViewModel {
                        Current = serverDetails.Slots.Queue.Current,
                        Max = serverDetails.Slots.Queue.Max
                    },
                    Soldier = new BattlelogSlotsViewModel {
                        Current = serverDetails.Slots.Soldier.Current,
                        Max = serverDetails.Slots.Soldier.Max
                    },
                    Spectator = new BattlelogSlotsViewModel {
                        Current = serverDetails.Slots.Spectator.Current,
                        Max = serverDetails.Slots.Spectator.Max
                    }
                }
            };
        }

        /// <summary>
        /// Create a new UUID for each calls
        /// </summary>
        /// <returns></returns>
        public static string CreateUUID() {
            //http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript
            string input = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx";
            char[] temp = input.ToCharArray();
            Random rand = new Random();
            for (int i = 0; i < input.Length; i++) {
                if (temp[i] == 'x' || temp[i] == 'y') {
                    int val = rand.Next(48, 57);
                    temp[i] = (char)val;
                }
            }
            input = new string(temp);
            using (MD5 md5 = MD5.Create()) {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                Guid result = new Guid(hash);
                return (result.ToString());
            }
        }
    }
}
