using MonoGameQuest.Foundation;

namespace MonoGameQuest
{
    public class PlayerSpriteAnimation : Animation
    {
        public PlayerSpriteAnimation(
            PlayerSprite sprite,
            AnimationType type,
            Direction direction,
            int spriteSheetRow,
            int framesLength,
            int frameDuration,
            bool flipHorizontally = false) 
            : base(
                sprite.SpriteSheet, 
                spriteSheetRow, 
                sprite.PixelWidth, 
                sprite.PixelHeight, 
                framesLength, 
                frameDuration, 
                flipHorizontally)
        {
            Type = type;
            Direction = direction;
        }

        public Direction Direction { get; private set; }
        
        public AnimationType Type { get; private set; }
    }
}
