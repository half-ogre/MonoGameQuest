using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace MonoGameQuest
{
    public class Map : MonoGameQuestComponent
    {
        int _displayHeight;
        int _displayWidth;
        readonly Dictionary<Vector2, List<int>> _terrain;
        readonly int _tileHeight;
        readonly int _tileWidth;
        readonly TmxMap _tmxMap;
        int _lastScale = 1;
        int _scaledTileHeight;
        int _scaledTileWidth;

        public Map(MonoGameQuest game) : base(game)
        {
            _tmxMap = new TmxMap(@"Content\map\map.tmx");

            _tileHeight = _scaledTileHeight = _tmxMap.TileHeight;
            _tileWidth = _scaledTileWidth = _tmxMap.TileWidth;
            
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

            BackgroundColor = new Color(
                r: _tmxMap.BackgroundColor.R,
                g: _tmxMap.BackgroundColor.G,
                b: _tmxMap.BackgroundColor.R);
        }

        public Color BackgroundColor { get; private set; }

        public int DisplayHeight { get { return _displayHeight; } }

        public int DisplayWidth { get { return _displayWidth; } }

        public int Height { get { return _tmxMap.Height; } }

        public Dictionary<Vector2, List<int>> Locations { get { return _terrain; } }

        public int TileHeight { get { return _scaledTileHeight; } }

        public int TileWidth { get { return _scaledTileWidth; } }

        public override void Update(GameTime gameTime)
        {
            if (_lastScale != Game.Scale)
                UpdateScale();
        }

        void UpdateScale()
        {
            _lastScale = Game.Scale;

            _scaledTileHeight = _tileHeight * _lastScale;
            _scaledTileWidth = _tileWidth * _lastScale;

            // TODO: this assumes the display size a multiple of the tile size. Eventually we'll need to handle the offset.
            _displayHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / _scaledTileHeight;
            _displayWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / _scaledTileWidth;
        }

        public int Width { get { return _tmxMap.Width; } }
    }
}
