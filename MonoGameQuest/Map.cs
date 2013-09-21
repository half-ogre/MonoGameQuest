﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace MonoGameQuest
{
    public class Map
    {
        readonly ContentManager _contentManager;
        int _displayHeight;
        int _displayWidth;
        readonly Dictionary<Vector2, List<int>> _terrain;
        readonly int _tileHeight;
        Texture2D _tileSheet;
        readonly int _tileWidth;
        int _tilesheetColumns;
        readonly TmxMap _tmxMap;
        int _scale = 1;
        int _scaledTileHeight;
        int _scaledTileWidth;

        public Map(ContentManager contentManager)
        {
            _contentManager = contentManager;

            _tileSheet = contentManager.Load<Texture2D>(@"images\1\tilesheet");
            
            _tmxMap = new TmxMap(@"Content\map\map.tmx");

            _tileHeight = _scaledTileHeight = _tmxMap.TileHeight;
            _tileWidth = _scaledTileWidth = _tmxMap.TileWidth;
            
            _tilesheetColumns = _tileSheet.Width / _scaledTileWidth;
            
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

        public int DisplayHeight { get { return _displayHeight; } }

        public int DisplayWidth { get { return _displayWidth; } }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (_displayHeight == 0 || _displayWidth == 0)
                return; // the map hasn't been updated even once, and so can't be drawn
            
            spriteBatch.GraphicsDevice.Clear(new Color(
                r: _tmxMap.BackgroundColor.R,
                g: _tmxMap.BackgroundColor.G,
                b: _tmxMap.BackgroundColor.R));

            for (var x = 0; x < _displayWidth; x++)
            {
                for (var y = 0; y < _displayHeight; y++)
                {
                    var mapIndex = new Vector2(x, y);
                    List<int> tileIndices;
                    if (_terrain.TryGetValue(mapIndex, out tileIndices))
                    {
                        foreach (var tileIndex in tileIndices)
                            spriteBatch.Draw(
                                texture: _tileSheet,
                                position: new Vector2(x * _scaledTileWidth, y * _scaledTileHeight),
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
                x: (tilesheetColumn - 1) * _scaledTileWidth,
                y: (tilesheetRow - 1) * _scaledTileHeight,
                width: _scaledTileWidth,
                height: _scaledTileHeight);
        }

        public int Height { get { return _tmxMap.Height; } }

        void SetScale(int scale, int displayHeight, int displayWidth)
        {
            _scale = scale;

            _tileSheet = _contentManager.Load<Texture2D>(string.Concat(@"images\", _scale, @"\tilesheet"));

            _scaledTileHeight = _tileHeight * _scale;
            _scaledTileWidth = _tileWidth * _scale;

            _tilesheetColumns = _tileSheet.Width / _scaledTileWidth;

            // TODO: this assumes the display size a multiple of the tile size. Eventually we'll need to handle the offset.
            _displayHeight = displayHeight / _scaledTileHeight;
            _displayWidth = displayWidth / _scaledTileWidth;
        }

        public int TileHeight { get { return _scaledTileHeight; } }

        public int TileWidth { get { return _scaledTileWidth; } }

        public void Update(UpdateContext context)
        {
            if (context.MapScale != _scale)
                SetScale(
                    context.MapScale, 
                    context.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight,
                    context.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth);
        }

        public int Width { get { return _tmxMap.Width; } }
    }
}
