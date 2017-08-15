using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameQuest.Foundation;

namespace MonoGameQuest
{
    public class PathfindingBox : MonoGameQuestComponent
    {
        readonly Sprite _sprite;
        
        public PathfindingBox(MonoGameQuest game) : base(game)
        {
            _sprite = new Sprite(
                game,
                game.Content.Load<Texture2D>("images/target"),
                16,
                16,
                0,
                0);

            _sprite.CurrentAnimation = new Animation(
                _sprite.SpriteSheet,
                0,
                16,
                16,
                4,
                50);

            _sprite.BlendState = BlendState.NonPremultiplied;
            _sprite.DrawOrder = Constants.DrawOrder.CursorBox;
            _sprite.Enabled = false;
            _sprite.SamplerState = SamplerState.PointClamp;
            _sprite.UpdateOrder = Constants.UpdateOrder.Cursor;
            _sprite.Visible = false;

            game.Components.Add(_sprite);

            UpdateOrder = Constants.UpdateOrder.Cursor;
        }

        public override void Update(GameTime gameTime)
        {
            _sprite.Scale = Game.Display.Scale;
            
            if (Game.Player.Path.Count > 0)
            {
                var destination = Game.Player.Path.Last();

                var displayCoordinate = Game.Display.CalculateDisplayCoordinateFromMapCoordinate(destination);

                _sprite.PixelPosition = new Vector2(
                    displayCoordinate.X * (Game.Map.PixelTileWidth * Game.Display.Scale),
                    displayCoordinate.Y * (Game.Map.PixelTileHeight * Game.Display.Scale));

                _sprite.Enabled = true;
                _sprite.Visible = true;
            }
            else
            {
                _sprite.Enabled = false;
                _sprite.Visible = false;
            }
            
            base.Update(gameTime);
        }
    }
}
