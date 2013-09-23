using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest
{
    public class Terrain : MonoGameQuestDrawableComponent
    {
        Texture2D _tileSheet;

        public Terrain(MonoGameQuest game) : base(game)
        {
            DrawOrder = Constants.DrawOrder.Terrain;
            UpdateOrder = Constants.UpdateOrder.Terrain;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game.Display.CoordinateHeight == 0 || Game.Display.CoordinateWidth == 0)
                return; // the map hasn't been updated even once, and so can't be drawn

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            SpriteBatch.GraphicsDevice.Clear(Game.Map.BackgroundColor);

            for (var x = 0; x < Game.Display.CoordinateWidth; x++)
            {
                for (var y = 0; y < Game.Display.CoordinateHeight; y++)
                {
                    var mapIndex = new Vector2(x, y);
                    List<int> tileIndices;
                    if (Game.Map.Locations.TryGetValue(mapIndex, out tileIndices))
                    {
                        foreach (var tileIndex in tileIndices)
                            SpriteBatch.Draw(
                                texture: _tileSheet,
                                position: new Vector2(x * Game.Map.PixelTileWidth, y * Game.Map.PixelTileHeight) * Game.Display.Scale,
                                sourceRectangle: GetSourceRectangleForTileIndex(tileIndex),
                                color: Color.White, /* tint */
                                rotation: 0f,
                                origin: Vector2.Zero,
                                scale: Game.Display.Scale,
                                effect: SpriteEffects.None,
                                depth: 0f);
                    }
                }
            }

            SpriteBatch.End();
        }

        private Rectangle GetSourceRectangleForTileIndex(int index)
        {
            var tileSheetColumns = _tileSheet.Width/Game.Map.PixelTileWidth;
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

        protected override void LoadContent()
        {
            _tileSheet = Game.Content.Load<Texture2D>(@"images\tilesheet");
            
            base.LoadContent();
        }
    }
}
