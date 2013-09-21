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

            float adjustedX;
            float adjustedY;

            float zeroBasedDisplayWidth = _sprite.Map.DisplayWidth - 1f;
            float zeroBasedDisplayHeight = _sprite.Map.DisplayHeight - 1f;
            float zeroBasedDisplayMidpointX = (_sprite.Map.DisplayWidth - 1f) / 2f;
            float zeroBasedDisplayMidpointY = (_sprite.Map.DisplayHeight - 1f) / 2f;
            
            // adjust sprite position to center, unless the sprite is at the map's edge:
            if (_sprite.Position.X < zeroBasedDisplayMidpointX)
                adjustedX = _sprite.Position.X;
            else if (_sprite.Position.X > _sprite.Map.Width - zeroBasedDisplayMidpointX)
                adjustedX = zeroBasedDisplayWidth - (_sprite.Map.Width - _sprite.Position.X);
            else
                adjustedX = zeroBasedDisplayMidpointX;
            if (_sprite.Position.Y < zeroBasedDisplayMidpointY)
                adjustedY = _sprite.Position.Y;
            else if (_sprite.Position.Y > _sprite.Map.Height - zeroBasedDisplayMidpointY)
                adjustedY = zeroBasedDisplayHeight - (_sprite.Map.Height - _sprite.Position.Y);
            else
                adjustedY = zeroBasedDisplayMidpointY;

            // adjust sprite position for the specified offset
            adjustedX += (adjustedX * _sprite.Map.TileWidth) + _sprite.OffsetX;
            adjustedY += (adjustedY * _sprite.Map.TileHeight) + _sprite.OffsetY;

            var adjustedPosition = new Vector2(
                adjustedX,
                adjustedY);

            spriteBatch.Draw(
                texture: _sprite.SpriteSheet,
                position: adjustedPosition,
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
