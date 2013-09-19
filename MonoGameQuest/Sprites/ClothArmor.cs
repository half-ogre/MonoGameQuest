using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameQuest.Sprites
{
    public class ClothArmor : PlayerCharacterSprite
    {
        public ClothArmor(ContentManager contentManager, Vector2 position) : base(
            contentManager: contentManager,
            spritesheetName: "clotharmor", 
            height: 32, 
            width: 32, 
            offsetX: -8, 
            offsetY: -12, 
            position: position, 
            idleDownAnimation: new Animation(row: 8, length: 2, speed: Animation.DefaultIdleSpeed))
        {
        }
    }
}
