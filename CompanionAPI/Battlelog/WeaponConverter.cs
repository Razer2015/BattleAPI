using CompanionAPI.Battlelog.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace CompanionAPI.Battlelog
{
    public static class WeaponConverter
    {
        const string FILE_NAME = "weaponInfos.json";
        private static WeaponInfo[] _weaponInfos;

        public static WeaponInfo WeaponInfo(string guid)
        {
            LoadWeaponInfos();

            return _weaponInfos?
                .FirstOrDefault(x => x.Guid.Equals(guid, StringComparison.OrdinalIgnoreCase));
        }

        private static void LoadWeaponInfos()
        {
            try
            {
                if (_weaponInfos != null)
                {
                    return;
                }

                if (!File.Exists(FILE_NAME))
                {
                    Console.WriteLine($"File doesn't exist: {FILE_NAME}");
                    return;
                }

                var content = File.ReadAllText(FILE_NAME);
                _weaponInfos = JsonConvert.DeserializeObject<WeaponInfo[]>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
