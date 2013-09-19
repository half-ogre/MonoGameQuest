using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameQuest.Sprites;

namespace MonoGameQuest
{
    public class PlayerCharacter
    {
        readonly PlayerCharacterSprite _sprite;

        public PlayerCharacter(
            ContentManager contentManager,
            Vector2 position)
        {
            if (contentManager == null)
                throw new ArgumentNullException("contentManager");

            Position = position;
            
            _sprite = new ClothArmor(contentManager);
            _sprite.SetAnimation(AnimationIdentifier.IdleDown);
        }

        public void Draw(
            SpriteBatch spriteBatch,
            int mapTileHeight,
            int mapTileWidth)
        {
            _sprite.Draw(spriteBatch, Position, mapTileWidth, mapTileWidth);
        }

        public Vector2 Position { get; set; }

        public void Update(GameTime gameTime)
        {
            _sprite.Update(gameTime);
        }
    }
}
