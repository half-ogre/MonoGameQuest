using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameFoundation;

namespace MonoGameQuest
{
    public class Cursor : MonoGameQuestDrawableComponent
    {
        const int _lineWidthForCursorBox = 2;
        Texture2D _pixelForCursorBox;
        readonly CursorSprite _sprite;

        public Cursor(MonoGameQuest game) : base(game)
        {
            DrawOrder = Constants.DrawOrder.CursorBox;
            UpdateOrder = Constants.UpdateOrder.Cursor;
            
            _sprite = new Hand(game);
            Game.Components.Add(_sprite);
        }

        public Vector2 CoordinatePosition { get; private set; }

        public override void Draw(GameTime gameTime)
        {
            var pixelPositionForCoordinate = Game.Display.CalculatePixelPosition(CoordinatePosition);

            var lineWidth = _lineWidthForCursorBox * Game.Display.Scale;

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            // draw the top line:
            SpriteBatch.DrawLine(
                _pixelForCursorBox,
                pixelPositionForCoordinate,
                (Game.Map.PixelTileWidth * Game.Display.Scale),
                0,
                lineWidth);

            // draw the left line:
            SpriteBatch.DrawLine(
                _pixelForCursorBox,
                pixelPositionForCoordinate + new Vector2(lineWidth, lineWidth),
                Game.Map.PixelTileWidth * Game.Display.Scale - (lineWidth * 2),
                (float)Math.PI / 2.0f,
                lineWidth);

            // draw the bottom line:
            SpriteBatch.DrawLine(
                _pixelForCursorBox,
                pixelPositionForCoordinate + new Vector2(Game.Map.PixelTileWidth * Game.Display.Scale, Game.Map.PixelTileHeight * Game.Display.Scale),
                Game.Map.PixelTileWidth * Game.Display.Scale,
                (float)Math.PI,
                lineWidth);

            // draw the bottom line:
            SpriteBatch.DrawLine(
                _pixelForCursorBox,
                pixelPositionForCoordinate + new Vector2(Game.Map.PixelTileWidth * Game.Display.Scale - lineWidth, Game.Map.PixelTileHeight * Game.Display.Scale - lineWidth),
                Game.Map.PixelTileWidth * Game.Display.Scale - (lineWidth * 2),
                (float)Math.PI * 1.5f,
                lineWidth);

            SpriteBatch.End();
            
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _pixelForCursorBox = new Texture2D(GraphicsDevice, 1, 1);
            _pixelForCursorBox.SetData(new[] { new Color(255, 50, 50, .5f) });
        }

        public Vector2 PixelPosition { get; private set; }

        public override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();

            PixelPosition = new Vector2(
                mouse.X,
                mouse.Y);

            CoordinatePosition = Game.Display.CalculateCoordinateFromPixelPosition(PixelPosition);

            if (_sprite != null)
                _sprite.PixelPosition = PixelPosition;
            
            base.Update(gameTime);
        }
    }
}
