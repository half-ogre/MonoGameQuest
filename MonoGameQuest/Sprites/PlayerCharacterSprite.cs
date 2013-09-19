using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Sprites
{
    public abstract class PlayerCharacterSprite
    {
        readonly Stack<Action<SpriteBatch>> _animation;
        int _animationSpeed;
        int _animationTimeAtCurrentFrame;
        private readonly ContentManager _contentManager;
        private readonly int _height;
        private readonly Animation _idleDownAnimation;
        private readonly int _offsetX;
        private readonly int _offsetY;
        private readonly Vector2 _position;
        int _scale = 1;
        int _scaledHeight;
        int _scaledOffsetX;
        int _scaledOffsetY;
        int _scaledWidth;
        private Texture2D _spritesheet;
        readonly string _spritesheetName;
        private readonly int _width;

        protected PlayerCharacterSprite(
            ContentManager contentManager,
            string spritesheetName,
            int height,
            int width,
            int offsetX,
            int offsetY,
            Vector2 position,
            Animation idleDownAnimation)
        {
            _contentManager = contentManager;
            _spritesheetName = spritesheetName;
            _height = _scaledHeight = height;
            _width = _scaledWidth = width;
            _offsetX = _scaledOffsetX = offsetX;
            _offsetY = _scaledOffsetY = offsetY;
            _position = position;
            _idleDownAnimation = idleDownAnimation;

            _spritesheet = _contentManager.Load<Texture2D>(string.Concat(@"images\1\", _spritesheetName));
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
                        index * _scaledWidth,
                        _idleDownAnimation.Row * _scaledHeight,
                        _scaledWidth,
                        _scaledHeight);

                    var offsetPosition = new Vector2(
                        _position.X + _scaledOffsetX,
                        _position.Y + _scaledOffsetY);

                    spriteBatch.Draw(_spritesheet, offsetPosition, sourceRectangle, Color.White);
                });
            }
        }

        void SetScale(int scale)
        {
            _scale = scale;

            _spritesheet = _contentManager.Load<Texture2D>(string.Concat(@"images\", _scale, @"\", _spritesheetName));
            
            _scaledHeight = _height * _scale;
            _scaledWidth = _width * _scale;
            
            _scaledOffsetX = _offsetX * _scale;
            _scaledOffsetY = _offsetY * _scale;
        }
        
        public void Update(UpdateContext context)
        {
            if (context.Scale != _scale)
                SetScale(context.Scale);

            _animationTimeAtCurrentFrame += context.GameTime.ElapsedGameTime.Milliseconds;

            if (_animation.Count < 1)
                Idle();
        }
    }
}
