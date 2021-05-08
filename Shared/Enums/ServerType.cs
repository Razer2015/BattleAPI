using System;

namespace Shared.Enums
{
    [Flags]
    public enum ServerTypes
    {
        None = 0,                 // 0 - Should never happen but let's leave it as fallback so it doesn't show Official when it's not
        Official = 1 << 0,        // 1 - Official
        Ranked = 1 << 1,          // 2 - Ranked
        Unranked = 1 << 2,        // 4 - Unranked
        Private = 1 << 3          // 8 - Private
    }
}
