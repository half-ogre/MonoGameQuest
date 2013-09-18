using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Sprites
{
    public class ClothArmor : Sprite
    {
        public ClothArmor(ContentManager contentManager)
            : base(contentManager.Load<Texture2D>("images/1/clotharmor"), 32, 32, -8, -12)
        {
            AddAnimation(new Animation(AnimationIdentifier.IdleDown, 8, 2, Animation.DefaultIdleSpeed));
        }
    }
}
