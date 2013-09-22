using Microsoft.Xna.Framework;

namespace MonoGameQuest
{
    public abstract class MonoGameQuestDrawableComponent : DrawableGameComponent
    {
        protected MonoGameQuestDrawableComponent(MonoGameQuest game) : base(game)
        {
        }

        public new MonoGameQuest Game { get { return base.Game as MonoGameQuest; } }
    }
}
