using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace MonoGameQuest
{
    public class Map
    {
        readonly int _columns;
        readonly Dictionary<Vector2, List<int>> _terrain;
        readonly TmxMap _tiledMap;
        readonly Texture2D _tileSheet;

        public Map(ContentManager contentManager)
        {
            _tiledMap = new TmxMap(@"Content\map\map.tmx");
            _tileSheet = contentManager.Load<Texture2D>(@"images\1\tilesheet");

            _columns = _tileSheet.Width / _tiledMap.TileWidth;

            _terrain = new Dictionary<Vector2, List<int>>();
            
            foreach (var layer in _tiledMap.Layers)
            {
                if (!layer.Visible || layer.Name.Equals("entities", StringComparison.OrdinalIgnoreCase))
                    continue;
                
                foreach (var tile in layer.Tiles)
                {
                    if (tile.Gid > 0)
                    {
                        var position = new Vector2(tile.X, tile.Y);

                        if (!_terrain.ContainsKey(position))
                            _terrain.Add(position, new List<int>());

                        _terrain[position].Add(tile.Gid);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(new Color(
                r: _tiledMap.BackgroundColor.R,
                g: _tiledMap.BackgroundColor.G,
                b: _tiledMap.BackgroundColor.R));
            
            var columns = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth / _tiledMap.TileWidth;
            if (spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth % _tiledMap.TileWidth > 0)
                columns++;

            var rows = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight / _tiledMap.TileHeight;
            if (spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight % _tiledMap.TileHeight > 0)
                columns++;

            for (var x = 0; x < columns; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    var position = new Vector2(x, y);
                    List<int> tileIndices;
                    if (_terrain.TryGetValue(position, out tileIndices))
                    {
                        foreach (var tileIndex in tileIndices)
                            spriteBatch.Draw(
                                texture: _tileSheet,
                                position: new Vector2(x * _tiledMap.TileWidth, y *_tiledMap.TileHeight),
                                sourceRectangle: GetSourceRectangleForTileIndex(tileIndex),
                                color: Color.White /* tint */);
                    }
                }
            }
        }

        private Rectangle GetSourceRectangleForTileIndex(int index)
        {
            int row = 1;
            int column;

            while (index > _columns)
            {
                index -= _columns;
                row++;
            }
            column = index;

            return new Rectangle(
                x: (column - 1) * _tiledMap.TileWidth,
                y: (row - 1) * _tiledMap.TileHeight,
                width: _tiledMap.TileWidth,
                height: _tiledMap.TileHeight);
        }
    }
}
