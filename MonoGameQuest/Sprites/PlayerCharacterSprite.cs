using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Sprites
{
    public abstract class PlayerCharacterSprite
    {
        readonly Stack<Action<SpriteBatch>> _animation;
        int _animationSpeed;
        int _animationTimeAtCurrentFrame;
        private readonly int _height;
        private readonly int _offsetX;
        private readonly int _offsetY;
        private readonly Vector2 _position;
        private readonly Animation _idleDownAnimation;
        private readonly Texture2D _spritesheet;
        private readonly int _width;

        protected PlayerCharacterSprite(
            Texture2D spritesheet,
            int height,
            int width,
            int offsetX,
            int offsetY,
            Vector2 position,
            Animation idleDownAnimation)
        {
            _spritesheet = spritesheet;
            _height = height;
            _width = width;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _position = position;
            _idleDownAnimation = idleDownAnimation;

            _animation = new Stack<Action<SpriteBatch>>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_animationTimeAtCurrentFrame > _animationSpeed)
            {
                _animationTimeAtCurrentFrame = 0;
                _animation.Pop()(spriteBatch);
            }
            else
            {
                _animation.Peek()(spriteBatch);
            }
        }

        public void Idle()
        {
            _animation.Clear();

            _animationSpeed = _idleDownAnimation.Speed;

            for (var n = _idleDownAnimation.Length - 1; n >= 0; n--)
            {
                var index = n;
                _animation.Push(spriteBatch =>
                {
                    var sourceRectangle = new Rectangle(
                        index * _width,
                        _idleDownAnimation.Row * _height,
                        _width,
                        _height);

                    var offsetPosition = new Vector2(
                        _position.X + _offsetX,
                        _position.Y + _offsetY);

                    spriteBatch.Draw(_spritesheet, offsetPosition, sourceRectangle, Color.White);
                });
            }
        }
        
        public void Update(GameTime gameTime)
        {
            _animationTimeAtCurrentFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (_animation.Count < 1)
                Idle();
        }
    }
}
