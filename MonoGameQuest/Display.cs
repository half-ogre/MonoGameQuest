using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest
{
    public class Display : MonoGameQuestComponent
    {
        public Display(MonoGameQuest game) : base(game)
        {
            Scale = 1;
            UpdateOrder = Constants.UpdateOrder.Display;
        }

        public int CoordinateHeight { get; private set; }

        public int CoordinateWidth { get; private set; }

        public int Scale { get; private set; }

        void UpdateScale(PresentationParameters presentationParameters)
        {
            if (presentationParameters.BackBufferWidth <= 1500 || presentationParameters.BackBufferHeight <= 870)
                Scale = 2;
            else
                Scale = 3;

            // TODO: this assumes the display size a multiple of the tile size. Eventually we'll need to handle the offset.
            CoordinateHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / (Game.Map.PixelTileHeight * Scale);
            CoordinateWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / (Game.Map.PixelTileWidth * Scale);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateScale(GraphicsDevice.PresentationParameters);
        }
    }
}
