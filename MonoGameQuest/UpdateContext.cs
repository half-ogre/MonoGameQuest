using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class UpdateContext
    {
        public GameTime GameTime { get; set; }

        public GraphicsDeviceManager Graphics { get; set; }

        public KeyboardState KeyboardState { get; set; }

        public int MapScale { get; set; }

        public int MapTileHeight { get; set; }

        public int MapTileWidth { get; set; }
    }
}
