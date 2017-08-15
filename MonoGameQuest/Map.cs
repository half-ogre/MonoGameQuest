using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest
{
    public class Map : MonoGameQuestDrawableComponent
    {
        Dictionary<Vector2, List<int>> _locations;
        TiledMap _tileMap;
        Texture2D _tileSheet;

        public Map(MonoGameQuest game) : base(game)
        {
            UpdateOrder = Constants.UpdateOrder.Map;
            DrawOrder = Constants.DrawOrder.Map;
        }

        public Color BackgroundColor { get; private set; }

        public int CoordinateHeight { get; private set; }

        public int CoordinateWidth { get; private set; }

        public Vector2 CoordinateTerminus { get; set; }

        public int PixelTileHeight { get; private set; }

        public int PixelTileWidth { get; private set; }

        public override void Draw(GameTime gameTime)
        {
            if (Game.Display.CoordinateHeight == 0 || Game.Display.CoordinateWidth == 0)
                return; // the map hasn't been updated even once, and so can't be drawn

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            SpriteBatch.GraphicsDevice.Clear(Game.Map.BackgroundColor);

            for (var x = 0; x < Game.Display.CoordinateWidth + 1; x++)
            {
                for (var y = 0; y < Game.Display.CoordinateHeight + 1; y++)
                {
                    var displayCoordinate = new Vector2(x, y);

                    var mapIndex = Game.Display.CalculateMapCoordinateFromDisplayCoordinate(displayCoordinate);

                    List<int> tileIndices;
                    if (_locations.TryGetValue(mapIndex, out tileIndices))
                    {
                        foreach (var tileIndex in tileIndices)
                            SpriteBatch.Draw(
                                texture: _tileSheet,
                                position: Game.Display.CalculatePixelPosition(displayCoordinate),
                                sourceRectangle: GetSourceRectangleForTileIndex(tileIndex),
                                color: Color.White, /* tint */
                                rotation: 0f,
                                origin: Vector2.Zero,
                                scale: Game.Display.Scale,
                                effects: SpriteEffects.None,
                                layerDepth: 0f);
                    }
                }
            }

            SpriteBatch.End();
        }

        private Rectangle GetSourceRectangleForTileIndex(int index)
        {
            var tileSheetColumns = _tileSheet.Width / Game.Map.PixelTileWidth;
            var tilesheetRow = 1;

            while (index > tileSheetColumns)
            {
                index -= tileSheetColumns;
                tilesheetRow++;
            }

            var tilesheetColumn = index;

            return new Rectangle(
                x: (tilesheetColumn - 1) * Game.Map.PixelTileWidth,
                y: (tilesheetRow - 1) * Game.Map.PixelTileHeight,
                width: Game.Map.PixelTileWidth,
                height: Game.Map.PixelTileHeight);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _tileMap = Game.Content.Load<TiledMap>(@"map\map");
            _locations = new Dictionary<Vector2, List<int>>();
            _tileSheet = Game.Content.Load<Texture2D>(@"images\tilesheet");

            foreach (var layer in _tileMap.TileLayers)
            {
                if (!layer.IsVisible)
                    continue;

                for (var i = 0; i < layer.Tiles.Count; i++)
                {
                    var tile = layer.Tiles[i];

                    if (tile.GlobalIdentifier <= 0)
                    {
                        continue;
                    }

                    var position = new Vector2(i % _tileMap.Width, i / _tileMap.Width);

                    if (!_locations.ContainsKey(position))
                        _locations.Add(position, new List<int>());

                    _locations[position].Add(tile.GlobalIdentifier);
                }
            }

            CoordinateHeight = _tileMap.Height;
            CoordinateWidth = _tileMap.Width;

            CoordinateTerminus = new Vector2(
                CoordinateWidth - 1f,
                CoordinateHeight - 1f);

            PixelTileHeight = _tileMap.TileHeight;
            PixelTileWidth = _tileMap.TileWidth;

            BackgroundColor = new Color(
                r: _tileMap.BackgroundColor.Value.R,
                g: _tileMap.BackgroundColor.Value.G,
                b: _tileMap.BackgroundColor.Value.R);
        }
    }
}
