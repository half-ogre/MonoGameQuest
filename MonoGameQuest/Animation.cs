using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameQuest.Sprites;

namespace MonoGameQuest
{
    public class Animation
    {
        int _currentIndex;
        int _timeAtCurrentIndex;
        readonly PlayerCharacterSprite _sprite;

        public Animation(
            PlayerCharacterSprite sprite,
            AnimationType type,
            Direction direction,
            int row,
            int length,
            int speed,
            bool flipHorizontally = false)
        {
            _sprite = sprite;
            if (row < 0)
                throw new ArgumentException("Animation row must be at least zero.", "row");

            if (length < 1)
                throw new ArgumentException("Animation length must be greater than zero.", "length");

            if (speed < 1)
                throw new ArgumentException("Animation speed must be greater than zero.", "speed");

            Type = type;
            Direction = direction;
            Row = row;
            Length = length;
            Speed = speed;
            FlipHorizontally = flipHorizontally;
        }

        public Direction Direction { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = new Rectangle(
                _currentIndex * _sprite.Width,
                Row * _sprite.Height,
                _sprite.Width,
                _sprite.Height);

            var offsetPosition = new Vector2(
                (_sprite.Position.X * _sprite.Map.TileWidth) + _sprite.OffsetX,
                (_sprite.Position.Y * _sprite.Map.TileHeight) + _sprite.OffsetY);

            spriteBatch.Draw(
                texture: _sprite.SpriteSheet,
                position: offsetPosition,
                sourceRectangle: sourceRectangle,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: Vector2.One,
                effect: FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                depth: 0f);
        }
        
        public bool FlipHorizontally { get; private set; }
        
        public int Length { get; private set; }

        public void Reset()
        {
            _timeAtCurrentIndex = 0;
            _currentIndex = 0;
        }

        public int Row { get; private set; }
        
        public int Speed { get; private set; }

        public AnimationType Type { get; private set; }

        public void Update(GameTime gameTime)
        {
            _timeAtCurrentIndex += gameTime.ElapsedGameTime.Milliseconds;

            if (_timeAtCurrentIndex > Speed)
            {
                _timeAtCurrentIndex = 0;

                if (++_currentIndex >= Length)
                    _currentIndex = 0;
            }
        }
    }
}
