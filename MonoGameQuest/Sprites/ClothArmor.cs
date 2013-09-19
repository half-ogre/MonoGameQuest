using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Sprites
{
    public class ClothArmor : PlayerCharacterSprite
    {
        public ClothArmor(ContentManager contentManager, Vector2 position) : base(
            spritesheet: contentManager.Load<Texture2D>("images/1/clotharmor"), 
            height: 32, 
            width: 32, 
            offsetX: -8, 
            offsetY: -12, 
            position: position, 
            idleDownAnimation: new Animation(AnimationIdentifier.IdleDown, 8, 2, Animation.DefaultIdleSpeed))
        {
        }
    }
}
