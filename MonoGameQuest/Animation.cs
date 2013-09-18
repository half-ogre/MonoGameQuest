using System;

namespace MonoGameQuest
{
    public class Animation
    {
        public const int DefaultAttackSpeed = 50;
        public const int DefaultIdleSpeed = 450;
        public const int DefaultWalkSpeed = 100;

        public Animation(
            AnimationIdentifier animationId,
            int row,
            int length,
            int speed)
        {
            if (animationId == AnimationIdentifier.None)
                throw new ArgumentException("Animation identifier is required.", "animationId");

            if (row < 0)
                throw new ArgumentException("Animation row must be at least zero.", "row");

            if (length < 1)
                throw new ArgumentException("Animation length must be greater than zero.", "length");

            if (speed < 1)
                throw new ArgumentException("Animation speed must be greater than zero.", "speed");

            AnimationId = animationId;
            Row = row;
            Length = length;
            Speed = speed;
        }

        public AnimationIdentifier AnimationId { get; private set; }
        public int Length { get; private set; }
        public int Speed { get; set; }
        public int Row { get; private set; }
    }
}
