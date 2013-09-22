using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest
{
    public abstract class MonoGameQuestDrawableComponent : DrawableGameComponent
    {
        protected MonoGameQuestDrawableComponent(MonoGameQuest game) : base(game)
        {
        }

        public new MonoGameQuest Game { get { return base.Game as MonoGameQuest; } }

        protected override void LoadContent()
        {
            base.LoadContent();

            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public SpriteBatch SpriteBatch { get; private set; }
    }
}
