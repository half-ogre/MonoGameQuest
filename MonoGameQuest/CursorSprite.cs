using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameQuest.Foundation;

namespace MonoGameQuest
{
    public class CursorSprite : Sprite
    {
        public CursorSprite(
            MonoGameQuest game,
            Texture2D spriteSheet,
            int pixelWidth,
            int pixelHeight) : base(
                game: game, 
                spriteSheet: spriteSheet, 
                pixelWidth: pixelWidth, 
                pixelHeight: pixelHeight, 
                pixelOffsetX: 0, 
                pixelOffsetY: 0)
        {
            DrawOrder = Constants.DrawOrder.Cursor;
            UpdateOrder = Constants.UpdateOrder.Sprites;
        }

        protected override void BeginSpriteBatch()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }

        public override void Draw(GameTime gameTime)
        {
            if (PixelPosition.X < 0
                || PixelPosition.Y < 0
                || PixelPosition.X > GraphicsDevice.PresentationParameters.BackBufferWidth
                || PixelPosition.Y > GraphicsDevice.PresentationParameters.BackBufferHeight)
                return;

            base.Draw(gameTime);
        }

        public new MonoGameQuest Game
        {
            get { return base.Game as MonoGameQuest; }
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Game.Display.Scale;
            
            base.Update(gameTime);
        }
    }
}
