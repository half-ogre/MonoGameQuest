using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameQuest.Sprites;

namespace MonoGameQuest
{
    public class PlayerCharacter : MonoGameQuestComponent
    {
        PlayerCharacterSprite _sprite;

        public PlayerCharacter(MonoGameQuest game) : base(game)
        {
            UpdateOrder = Constants.UpdateOrder.Models;
        }

        public override void Initialize()
        {
            _sprite = new ClothArmor(Game, Game.Content, Vector2.Zero, Game.Map);
            Game.Components.Add(_sprite);
            
            base.Initialize();
        }

        public Vector2 Position { get { return _sprite.Position; } }

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
