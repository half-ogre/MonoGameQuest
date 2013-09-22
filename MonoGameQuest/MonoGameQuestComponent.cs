using Microsoft.Xna.Framework;

namespace MonoGameQuest
{
    public abstract class MonoGameQuestComponent : GameComponent
    {
        protected MonoGameQuestComponent(MonoGameQuest game) : base(game)
        {
        }

        public new MonoGameQuest Game { get { return base.Game as MonoGameQuest; } }
    }
}
