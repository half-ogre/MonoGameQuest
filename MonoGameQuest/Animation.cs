using System;

namespace MonoGameQuest
{
    public class Animation
    {
        public const int DefaultAttackSpeed = 50;
        public const int DefaultIdleSpeed = 450;
        public const int DefaultMoveLength = 4;
        public const int DefaultMoveSpeed = 120;
        public const int DefaultWalkSpeed = 100;

        public Animation(
            int row,
            int length)
        {
            if (row < 0)
                throw new ArgumentException("Animation row must be at least zero.", "row");

            if (length < 1)
                throw new ArgumentException("Animation length must be greater than zero.", "length");

            Row = row;
            Length = length;
        }

        public int Length { get; private set; }
        public int Row { get; private set; }
    }
}
