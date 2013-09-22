using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest
{
    public class Terrain : MonoGameQuestDrawableComponent
    {
        int _lastScale;
        Texture2D _tileSheet;

        public Terrain(MonoGameQuest game) : base(game)
        {   
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game.Map.DisplayCoordinateHeight == 0 || Game.Map.DisplayCoordinateWidth == 0)
                return; // the map hasn't been updated even once, and so can't be drawn

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            SpriteBatch.GraphicsDevice.Clear(Game.Map.BackgroundColor);

            for (var x = 0; x < Game.Map.DisplayCoordinateWidth; x++)
            {
                for (var y = 0; y < Game.Map.DisplayCoordinateHeight; y++)
                {
                    var mapIndex = new Vector2(x, y);
                    List<int> tileIndices;
                    if (Game.Map.Locations.TryGetValue(mapIndex, out tileIndices))
                    {
                        foreach (var tileIndex in tileIndices)
                            SpriteBatch.Draw(
                                texture: _tileSheet,
                                position: new Vector2(x * Game.Map.ScaledTilePixelWidth, y * Game.Map.ScaledTilePixelHeight),
                                sourceRectangle: GetSourceRectangleForTileIndex(tileIndex),
                                color: Color.White /* tint */);
                    }
                }
            }

            SpriteBatch.End();
        }

        private Rectangle GetSourceRectangleForTileIndex(int index)
        {
            var tileSheetColumns = _tileSheet.Width/Game.Map.ScaledTilePixelWidth;
            var tilesheetRow = 1;

            while (index > tileSheetColumns)
            {
                index -= tileSheetColumns;
                tilesheetRow++;
            }

            var tilesheetColumn = index;

            return new Rectangle(
                x: (tilesheetColumn - 1) * Game.Map.ScaledTilePixelWidth,
                y: (tilesheetRow - 1) * Game.Map.ScaledTilePixelHeight,
                width: Game.Map.ScaledTilePixelWidth,
                height: Game.Map.ScaledTilePixelHeight);
        }

        protected override void LoadContent()
        {
            _tileSheet = Game.Content.Load<Texture2D>(@"images\1\tilesheet");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_lastScale != Game.Scale)
                UpdateScale();
        }

        void UpdateScale()
        {
            _lastScale = Game.Scale;

            _tileSheet = Game.Content.Load<Texture2D>(string.Concat(@"images\", _lastScale, @"\tilesheet"));
        }
    }
}
