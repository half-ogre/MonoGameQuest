using Microsoft.Xna.Framework;

namespace MonoGameQuest
{
    public class UpdateContext
    {
        public GameTime GameTime { get; set; }

        public GraphicsDeviceManager Graphics { get; set; }

        public int Scale { get; set; }
    }
}
