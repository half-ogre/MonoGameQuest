using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameFoundation;

namespace MonoGameQuest
{
    public class Cursor : MonoGameQuestComponent
    {
        CursorSprite _sprite;
        
        public Cursor(MonoGameQuest game) : base(game)
        {
            UpdateOrder = Constants.UpdateOrder.Cursor;
            
            _sprite = new Hand(game);
            Game.Components.Add(_sprite);
        }

        public Vector2 PixelPosition { get; private set; }

        public override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();

            PixelPosition = new Vector2(
                mouse.X,
                mouse.Y);

            if (_sprite != null)
                _sprite.PixelPosition = PixelPosition;
            
            base.Update(gameTime);
        }
    }
}
