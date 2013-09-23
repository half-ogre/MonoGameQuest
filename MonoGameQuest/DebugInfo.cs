using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class DebugInfo : MonoGameQuestDrawableComponent
    {
        int _fpsCounter;
        TimeSpan _fpsElapsedTime = TimeSpan.Zero;
        int _fpsRate;
        bool _showDebugInfo;
        SpriteFont _spriteFont;
        Texture2D _pixelForGrid;
        bool _ctrlDKeysWerePressed;

        public DebugInfo(MonoGameQuest game) : base(game)
        {
            DrawOrder = Constants.DrawOrder.Debug;
            UpdateOrder = Constants.UpdateOrder.Debug;
        }

        public override void Draw(GameTime gameTime)
        {
            _fpsCounter++;
            var debugInfo = string.Format("fps: {0}", _fpsRate);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            if (_showDebugInfo)
                DrawGrid(SpriteBatch);

            if (_showDebugInfo)
            {
                SpriteBatch.DrawString(_spriteFont, debugInfo, new Vector2(1, 1), Color.Black, 0, Vector2.Zero, .5f, SpriteEffects.None, 0f);
                SpriteBatch.DrawString(_spriteFont, debugInfo, new Vector2(0, 0), Color.Yellow, 0, Vector2.Zero, .5f, SpriteEffects.None, 0f);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawGrid(SpriteBatch spriteBatch)
        {
            var mapHeight = Game.Map.CoordinateHeight * (Game.Map.PixelTileHeight * Game.Display.Scale);
            var mapWidth = Game.Map.CoordinateWidth * (Game.Map.PixelTileWidth * Game.Display.Scale);

            DrawLine(spriteBatch, Vector2.Zero, new Vector2(mapWidth, 0));
            for (var y = 1; y <= Game.Display.CoordinateHeight; y++)
            {
                var adjustedY = y * (Game.Map.PixelTileHeight * Game.Display.Scale);
                DrawLine(spriteBatch, new Vector2(0, adjustedY - 1), new Vector2(mapWidth, adjustedY - 1));
                DrawLine(spriteBatch, new Vector2(0, adjustedY), new Vector2(mapWidth, adjustedY));
            }

            DrawLine(spriteBatch, Vector2.Zero, new Vector2(0, mapHeight));
            for (var x = 1; x <= Game.Display.CoordinateWidth; x++)
            {
                var adjustedX = x * (Game.Map.PixelTileWidth * Game.Display.Scale);
                DrawLine(spriteBatch, new Vector2(adjustedX - 1, 0), new Vector2(adjustedX - 1, mapHeight));
                DrawLine(spriteBatch, new Vector2(adjustedX, 0), new Vector2(adjustedX, mapHeight));
            }
        }

        void DrawLine(
            SpriteBatch spriteBatch,
            Vector2 start,
            Vector2 end)
        {
            var edge = end - start;
            var angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(
                _pixelForGrid,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(),
                    1),
                null,
                Color.White,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _pixelForGrid = new Texture2D(GraphicsDevice, 1, 1);
            _pixelForGrid.SetData(new[] { new Color(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B, 32) });

            _spriteFont = Game.Content.Load<SpriteFont>(@"fonts\Consolas");
        }

        public override void Update(GameTime gameTime)
        {
            _fpsElapsedTime += gameTime.ElapsedGameTime;
            if (_fpsElapsedTime > TimeSpan.FromSeconds(1))
            {
                _fpsElapsedTime -= TimeSpan.FromSeconds(1);
                _fpsRate = _fpsCounter;
                _fpsCounter = 0;
            }

            var keyboardState = Keyboard.GetState();
            var ctrlDKeysArePressed = (keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.LeftControl)) && keyboardState.IsKeyDown(Keys.D);
            if (!ctrlDKeysArePressed && _ctrlDKeysWerePressed)
                _showDebugInfo = !_showDebugInfo;
            _ctrlDKeysWerePressed = ctrlDKeysArePressed;
        }
    }
}
