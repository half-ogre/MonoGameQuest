using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest
{
    public class Display : MonoGameQuestComponent
    {
        Vector2? _mapCoordinateOffset;

        public Display(MonoGameQuest game) : base(game)
        {
            Scale = 1;
            UpdateOrder = Constants.UpdateOrder.Display;

            UpdateScale(GraphicsDevice.PresentationParameters);
        }

        public Vector2 CalculateMapCoordinate(Vector2 displayCoordinate)
        {
            UpdateMapCoordinateOffset();

            Debug.Assert(_mapCoordinateOffset != null);

            return new Vector2(
                (float)Math.Floor(displayCoordinate.X + _mapCoordinateOffset.Value.X),
                (float)Math.Floor(displayCoordinate.Y + _mapCoordinateOffset.Value.Y));
        }

        public Vector2 CalculatePixelPosition(Vector2 displayCoordinate)
        {
            UpdateMapCoordinateOffset();

            Debug.Assert(_mapCoordinateOffset != null);

            var xPixelOffset = (_mapCoordinateOffset.Value.X % 1) * Game.Map.PixelTileWidth;
            var yPixelOffset = (_mapCoordinateOffset.Value.Y % 1) * Game.Map.PixelTileHeight;

            return new Vector2(
                (displayCoordinate.X * Game.Map.PixelTileWidth) - xPixelOffset,
                (displayCoordinate.Y * Game.Map.PixelTileHeight) - yPixelOffset)
                   * Game.Display.Scale;
        }

        public int CoordinateHeight { get; private set; }

        public Vector2 CoordinateMidpoint { get; set; }

        public Vector2 CoordinateTerminus { get; set; }

        public int CoordinateWidth { get; private set; }

        public int Scale { get; private set; }

        private void UpdateMapCoordinateOffset()
        {
            if (!_mapCoordinateOffset.HasValue)
            {
                var mapCoordinateOffsetX = Game.Player.CoordinatePosition.X - CoordinateMidpoint.X;
                if (mapCoordinateOffsetX < 0)
                    mapCoordinateOffsetX = 0;
                if (mapCoordinateOffsetX > Game.Map.CoordinateWidth - Game.Display.CoordinateWidth)
                    mapCoordinateOffsetX = Game.Map.CoordinateWidth - Game.Display.CoordinateWidth;

                var mapCoordinateOffsetY = Game.Player.CoordinatePosition.Y - CoordinateMidpoint.Y;
                if (mapCoordinateOffsetY < 0)
                    mapCoordinateOffsetY = 0;
                if (mapCoordinateOffsetY > Game.Map.CoordinateHeight - Game.Display.CoordinateHeight)
                    mapCoordinateOffsetY = Game.Map.CoordinateHeight - Game.Display.CoordinateHeight;

                _mapCoordinateOffset = new Vector2(
                    mapCoordinateOffsetX,
                    mapCoordinateOffsetY);
            }
        }

        void UpdateScale(PresentationParameters presentationParameters)
        {
            if (presentationParameters.BackBufferWidth <= 1500 || presentationParameters.BackBufferHeight <= 870)
                Scale = 2;
            else
                Scale = 3;

            // TODO: this assumes the display size a multiple of the tile size. Eventually we'll need to handle the offset.
            CoordinateHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / (Game.Map.PixelTileHeight * Scale);
            CoordinateWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / (Game.Map.PixelTileWidth * Scale);

            CoordinateTerminus = new Vector2(
                CoordinateWidth - 1f,
                CoordinateHeight - 1f);
        }

        public override void Update(GameTime gameTime)
        {
            _mapCoordinateOffset = null;
            
            UpdateScale(GraphicsDevice.PresentationParameters);

            CoordinateMidpoint = new Vector2(
                (Game.Display.CoordinateWidth - 1) / 2f,
                (Game.Display.CoordinateHeight - 1) / 2f);
        }
    }
}
