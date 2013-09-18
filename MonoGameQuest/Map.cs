using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace MonoGameQuest
{
    public class Map
    {
        readonly Dictionary<Vector2, List<int>> _terrain;
        readonly Texture2D _tileSheet;
        readonly int _tilesheetColumns;
        readonly TmxMap _tmxMap;
        int _screenColumns = 0;
        int _screenHeight = 0;
        int _screenRows = 0;
        int _screenWidth = 0;

        public Map(ContentManager contentManager)
        {
            _tileSheet = contentManager.Load<Texture2D>(@"images\1\tilesheet");
            _tmxMap = new TmxMap(@"Content\map\map.tmx");

            TileHeight = _tmxMap.TileHeight;
            _tilesheetColumns = _tileSheet.Width / _tmxMap.TileWidth;
            TileWidth = _tmxMap.TileWidth;
            _terrain = new Dictionary<Vector2, List<int>>();
            
            foreach (var layer in _tmxMap.Layers)
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
            if (_screenRows == 0 || _screenColumns == 0)
                return; // the map hasn't been updated even once, and so can't be drawn
            
            spriteBatch.GraphicsDevice.Clear(new Color(
                r: _tmxMap.BackgroundColor.R,
                g: _tmxMap.BackgroundColor.G,
                b: _tmxMap.BackgroundColor.R));

            for (var x = 0; x < _screenColumns; x++)
            {
                for (var y = 0; y < _screenRows; y++)
                {
                    var position = new Vector2(x, y);
                    List<int> tileIndices;
                    if (_terrain.TryGetValue(position, out tileIndices))
                    {
                        foreach (var tileIndex in tileIndices)
                            spriteBatch.Draw(
                                texture: _tileSheet,
                                position: new Vector2(x * _tmxMap.TileWidth, y *_tmxMap.TileHeight),
                                sourceRectangle: GetSourceRectangleForTileIndex(tileIndex),
                                color: Color.White /* tint */);
                    }
                }
            }
        }

        private Rectangle GetSourceRectangleForTileIndex(int index)
        {
            int tilesheetRow = 1;
            int tilesheetColumn;

            while (index > _tilesheetColumns)
            {
                index -= _tilesheetColumns;
                tilesheetRow++;
            }

            tilesheetColumn = index;

            return new Rectangle(
                x: (tilesheetColumn - 1) * _tmxMap.TileWidth,
                y: (tilesheetRow - 1) * _tmxMap.TileHeight,
                width: _tmxMap.TileWidth,
                height: _tmxMap.TileHeight);
        }

        public int TileHeight { get; private set; }
        
        public int TileWidth { get; private set; }

        public void Update(GraphicsDeviceManager graphics)
        {
            if (_screenWidth != graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)
            {
                _screenWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
                _screenColumns = _screenWidth / _tmxMap.TileWidth;
                if (_screenWidth % _tmxMap.TileWidth > 0)
                    _screenColumns++;
            }

            if (_screenHeight != graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)
            {
                _screenHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
                _screenRows = _screenHeight / _tmxMap.TileHeight;
                if (_screenHeight % _tmxMap.TileHeight > 0)
                    _screenRows++;
            }
        }
    }
}
