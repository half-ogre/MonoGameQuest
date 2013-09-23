using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace MonoGameQuest
{
    public class Map : MonoGameQuestComponent
    {
        readonly Dictionary<Vector2, List<int>> _locations;

        public Map(MonoGameQuest game) : base(game)
        {
            var tmxMap = new TmxMap(@"Content\map\map.tmx");

            CoordinateHeight = tmxMap.Height;
            CoordinateWidth = tmxMap.Width;

            PixelTileHeight = tmxMap.TileHeight;
            PixelTileWidth = tmxMap.TileWidth;
            
            _locations = new Dictionary<Vector2, List<int>>();
            
            foreach (var layer in tmxMap.Layers)
            {
                if (!layer.Visible || layer.Name.Equals("entities", StringComparison.OrdinalIgnoreCase))
                    continue;
                
                foreach (var tile in layer.Tiles)
                {
                    if (tile.Gid > 0)
                    {
                        var position = new Vector2(tile.X, tile.Y);

                        if (!_locations.ContainsKey(position))
                            _locations.Add(position, new List<int>());

                        _locations[position].Add(tile.Gid);
                    }
                }
            }

            BackgroundColor = new Color(
                r: tmxMap.BackgroundColor.R,
                g: tmxMap.BackgroundColor.G,
                b: tmxMap.BackgroundColor.R);
            UpdateOrder = Constants.UpdateOrder.Map;
        }

        public Color BackgroundColor { get; private set; }

        public int CoordinateHeight { get; private set; }

        public int CoordinateWidth { get; private set; }        

        public Dictionary<Vector2, List<int>> Locations { get { return _locations; } }

        public int PixelTileHeight { get; private set; }

        public int PixelTileWidth { get; private set; }
    }
}
