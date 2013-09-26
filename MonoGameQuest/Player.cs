using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class Player : MonoGameQuestComponent
    {
        PlayerSprite _sprite;

        public Player(MonoGameQuest game) : base(game)
        {
            UpdateOrder = Constants.UpdateOrder.Models;
        }

        public override void Initialize()
        {
            _sprite = new ClothArmor(Game, new Vector2(15, 222));
            Game.Components.Add(_sprite);
            
            base.Initialize();
        }

        public Vector2 CoordinatePosition { get { return _sprite.CoordinatePosition; } }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // if the sprite isn't already moving, accept new movement:
            if (!_sprite.IsMoving)
            {
                // move up:
                if (keyboardState.IsKeyDown(Keys.Up))
                    _sprite.Move(Direction.Up);
                // move down:
                else if (keyboardState.IsKeyDown(Keys.Down))
                    _sprite.Move(Direction.Down);
                // move the the left
                else if (keyboardState.IsKeyDown(Keys.Left))
                    _sprite.Move(Direction.Left);
                // move to the right:
                else if (keyboardState.IsKeyDown(Keys.Right))
                    _sprite.Move(Direction.Right);   
            }
        }
    }
}
