﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameQuest.Foundation;

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
                SpriteBatch.DrawString(_spriteFont, debugInfo, new Vector2(1, 101), Color.Black, 0, Vector2.Zero, .5f, SpriteEffects.None, 0f);
                SpriteBatch.DrawString(_spriteFont, debugInfo, new Vector2(0, 100), Color.Yellow, 0, Vector2.Zero, .5f, SpriteEffects.None, 0f);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawGrid(SpriteBatch spriteBatch)
        {
            var offsetForPrecedingRowBottomGridLine = new Vector2(0, -1);

            // draw the row grid lines:
            for (var y = 0; y <= Game.Display.CoordinateHeight; y++)
            {
                var start = Game.Display.CalculatePixelPosition(new Vector2(0, y));
                var end = Game.Display.CalculatePixelPosition(new Vector2(Game.Map.CoordinateWidth, y));

                // draw the row's top grid line:
                spriteBatch.DrawLine(
                    _pixelForGrid,
                    start,
                    end);

                // draw the preceding row's bottom grid line:
                spriteBatch.DrawLine(
                    _pixelForGrid,
                    start + offsetForPrecedingRowBottomGridLine,
                    end + offsetForPrecedingRowBottomGridLine);
            }

            var offsetForLeftGridLine = new Vector2(1, 0);
            
            // draw the column grid lines:
            for (var x = 0; x <= Game.Display.CoordinateWidth; x++)
            {
                var start = Game.Display.CalculatePixelPosition(new Vector2(x, 0));
                var end = Game.Display.CalculatePixelPosition(new Vector2(x, Game.Map.CoordinateHeight));

                // draw the column's left grid line
                spriteBatch.DrawLine(
                    _pixelForGrid,
                    start + offsetForLeftGridLine,
                    end + offsetForLeftGridLine);
                
                // draw the preceding column's right grid line
                spriteBatch.DrawLine(
                    _pixelForGrid,
                    start, 
                    end);
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _pixelForGrid = new Texture2D(GraphicsDevice, 1, 1);
            _pixelForGrid.SetData(new[] { new Color(Color.Yellow, .5f) });

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
