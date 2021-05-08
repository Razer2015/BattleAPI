using System;

namespace Shared.Enums
{
    [Flags]
    public enum RoleType
    {
        SOLDIER = 1 << 0,                              // 1
        COMMANDER = 1 << 1,                            // 2
        SPECTATOR = 1 << 2,                            // 4
    }
}
