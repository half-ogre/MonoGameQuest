using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameQuest.Sprites;

namespace MonoGameQuest
{
    public class PlayerCharacter
    {
        readonly PlayerCharacterSprite _sprite;

        public PlayerCharacter(
            ContentManager contentManager,
            Vector2 position,
            Map map)
        {
            if (contentManager == null)
                throw new ArgumentNullException("contentManager");

            Position = position;
            
            _sprite = new ClothArmor(contentManager, new Vector2(0, 0), map);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
        }

        public Vector2 Position { get; set; }

        public void Update(UpdateContext context)
        {
            _sprite.Update(context);

            // if the sprite isn't already moving, accept new movement:
            if (!_sprite.IsMoving)
            {
                // move up:
                if (context.KeyboardState.IsKeyDown(Keys.Up))
                    _sprite.Move(Direction.Up);
                // move down:
                else if (context.KeyboardState.IsKeyDown(Keys.Down))
                    _sprite.Move(Direction.Down);
                // move the the left
                else if (context.KeyboardState.IsKeyDown(Keys.Left))
                    _sprite.Move(Direction.Left);
                // move to the right:
                else if (context.KeyboardState.IsKeyDown(Keys.Right))
                    _sprite.Move(Direction.Right);   
            }
        }
    }
}
