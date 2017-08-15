using Microsoft.Xna.Framework.Graphics;
using MonoGameQuest.Foundation;

namespace MonoGameQuest
{
    public class Hand : CursorSprite
    {
        public Hand(MonoGameQuest game)
            : base(
                game: game,
                spriteSheet: game.Content.Load<Texture2D>("images/hand"),
                pixelWidth: 14,
                pixelHeight: 14)
        {
            CurrentAnimation = new Animation(
                spriteSheet: SpriteSheet,
                spriteSheetRow: 0,
                framePixelWidth: 14, 
                framePixelHeight: 14, 
                framesLength: 1);
        }
    }
}
