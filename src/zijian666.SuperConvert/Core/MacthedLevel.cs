using System;

namespace zijian666.SuperConvert.Core
{
    [Flags]
    public enum MacthedLevel
    {
        Contravariant = 1,
        Interface = 2,
        Subclass = 4,
        Full = Subclass | Interface | Contravariant,
    }
}
