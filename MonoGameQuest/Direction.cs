using System;

namespace MonoGameQuest
{
    [Flags]
    public enum Direction
    {
        None = 0x0,
        Up = 0x1,
        Down = 0x2,
        Left = 0x4,
        Right = 0x8
    }
}
