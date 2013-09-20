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
            AnimationType type,
            Direction direction,
            int row,
            int length,
            int speed,
            bool flipHorizontally = false)
        {
            if (row < 0)
                throw new ArgumentException("Animation row must be at least zero.", "row");

            if (length < 1)
                throw new ArgumentException("Animation length must be greater than zero.", "length");

            if (speed < 1)
                throw new ArgumentException("Animation speed must be greater than zero.", "speed");

            Type = type;
            Direction = direction;
            Row = row;
            Length = length;
            Speed = speed;
            FlipHorizontally = flipHorizontally;
        }

        public Direction Direction { get; private set; }
        
        public bool FlipHorizontally { get; private set; }
        
        public int Length { get; private set; }

        public int Row { get; private set; }
        
        public int Speed { get; private set; }

        public AnimationType Type { get; private set; }
    }
}
