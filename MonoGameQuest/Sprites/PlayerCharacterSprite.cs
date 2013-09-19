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
        readonly ContentManager _contentManager;
        readonly int _height;
        readonly int _idleAnimationSpeed;
        readonly Animation _idleDownAnimation;
        readonly Animation _idleRightAnimation;
        readonly Animation _idleUpAnimation;
        bool _isIdling;
        bool _isWalking;
        readonly Stack<Action> _movement;
        readonly int _movementLength;
        readonly int _movementSpeed;
        int _movementTimeAtCurrentPosition;
        readonly int _offsetX;
        readonly int _offsetY;
        Vector2 _mapPosition;
        int _mapTileHeight;
        int _mapTileWidth;
        Direction _orientation;
        int _scale = 1;
        int _scaledHeight;
        int _scaledOffsetX;
        int _scaledOffsetY;
        int _scaledWidth;
        Texture2D _spritesheet;
        readonly string _spritesheetName;
        readonly int _walkAnimationSpeed;
        readonly Animation _walkDownAnimation;
        readonly Animation _walkRightAnimation;
        readonly Animation _walkUpAnimation;
        readonly int _width;

        protected PlayerCharacterSprite(
            ContentManager contentManager,
            string spritesheetName,
            int height,
            int width,
            int offsetX,
            int offsetY,
            Vector2 mapPosition,
            int mapTileHeight,
            int mapTileWidth,
            int movementLength,
            int movementSpeed,
            int idleAnimationSpeed,
            Animation idleUpAnimation,
            Animation idleDownAnimation,
            Animation idleRightAnimation,
            int walkAnimationSpeed,
            Animation walkUpAnimation,
            Animation walkDownAnimation,
            Animation walkRightAnimation)
        {
            _contentManager = contentManager;
            _spritesheetName = spritesheetName;
            _height = _scaledHeight = height;
            _width = _scaledWidth = width;
            _offsetX = _scaledOffsetX = offsetX;
            _offsetY = _scaledOffsetY = offsetY;
            _mapPosition = mapPosition;
            _mapTileHeight = mapTileHeight;
            _mapTileWidth = mapTileWidth;
            _movementLength = movementLength;
            _movementSpeed = movementSpeed;
            _idleAnimationSpeed = idleAnimationSpeed;
            _idleUpAnimation = idleUpAnimation;
            _idleDownAnimation = idleDownAnimation;
            _idleRightAnimation = idleRightAnimation;
            _walkAnimationSpeed = walkAnimationSpeed;
            _walkUpAnimation = walkUpAnimation;
            _walkDownAnimation = walkDownAnimation;
            _walkRightAnimation = walkRightAnimation;

            _animation = new Stack<Action<SpriteBatch>>();
            _movement = new Stack<Action>();
            _spritesheet = _contentManager.Load<Texture2D>(string.Concat(@"images\1\", _spritesheetName));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAnimating)
            {
                _animation.Peek()(spriteBatch);
            }
        }

        public void Idle()
        {
            _animationTimeAtCurrentFrame = 0;
            _animation.Clear();

            _animationSpeed = _idleAnimationSpeed;

            Animation idleAnimation;
            bool flip = false;

            if (_orientation == Direction.Up)
            {
                idleAnimation = _idleUpAnimation;
            }
            else if (_orientation == Direction.Left)
            {
                idleAnimation = _idleRightAnimation;
                flip = true;
            }
            else if (_orientation == Direction.Right)
            {
                idleAnimation = _idleRightAnimation;
            }
            else // default to down:
            {
                idleAnimation = _idleDownAnimation;
            }

            for (var n = idleAnimation.Length - 1; n >= 0; n--)
            {
                var index = n;
                _animation.Push(spriteBatch =>
                {
                    _isWalking = false;
                    _isIdling = true;

                    var sourceRectangle = new Rectangle(
                        index * _scaledWidth,
                        idleAnimation.Row * _scaledHeight,
                        _scaledWidth,
                        _scaledHeight);

                    var offsetPosition = new Vector2(
                        _mapPosition.X + _scaledOffsetX,
                        _mapPosition.Y + _scaledOffsetY);

                    spriteBatch.Draw(_spritesheet, offsetPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                });
            }
        }

        public bool IsAnimating { get { return _animation.Count > 0; }}

        public bool IsMoving { get { return _movement.Count > 0; } }

        public void Move(Direction direction)
        {
            _movementTimeAtCurrentPosition = 0;
            _movement.Clear();

            _orientation = direction;

            var offsetX = 0;
            var offsetY = 0;

            if (direction == Direction.Up)
                offsetY = -1;
            if (direction == Direction.Down)
                offsetY = 1;
            if (direction == Direction.Left)
                offsetX = -1;
            if (direction == Direction.Right)
                offsetX = 1;

            for (var n = _movementLength - 1; n >= 0; n--)
            {
                _movement.Push(() =>
                {
                    _mapPosition = new Vector2(
                        _mapPosition.X + (offsetX * _mapTileWidth) / _movementLength,
                        _mapPosition.Y + (offsetY * _mapTileHeight) / _movementLength);
                });
            }
        }

        void SetScale(UpdateContext context)
        {
            _scale = context.MapScale;

            _mapTileHeight = context.MapTileHeight;
            _mapTileWidth = context.MapTileWidth;

            _spritesheet = _contentManager.Load<Texture2D>(string.Concat(@"images\", _scale, @"\", _spritesheetName));
            
            _scaledHeight = _height * _scale;
            _scaledWidth = _width * _scale;
            
            _scaledOffsetX = _offsetX * _scale;
            _scaledOffsetY = _offsetY * _scale;
        }
        
        public void Update(UpdateContext context)
        {
            if (context.MapScale != _scale)
                SetScale(context);

            // stash these values because they will change during update, 
            // and we need to know the value when the update started.
            var wasMoving = IsMoving;

            if (wasMoving)
            {
                _movementTimeAtCurrentPosition += context.GameTime.ElapsedGameTime.Milliseconds;

                if (_movementTimeAtCurrentPosition > _movementSpeed / _movementLength)
                {
                    _movementTimeAtCurrentPosition = 0;
                    _movement.Pop()();
                }
            }

            if (IsAnimating)
            {
                _animationTimeAtCurrentFrame += context.GameTime.ElapsedGameTime.Milliseconds;

                if (_animationTimeAtCurrentFrame > _animationSpeed)
                {
                    _animationTimeAtCurrentFrame = 0;
                    _animation.Pop();

                    if (_animation.Count < 1)
                    {
                        _isIdling = false;
                        _isWalking = false;
                    }
                }
            }

            // start walking if the sprite is moving and isn't already walking:
            if (wasMoving && !_isWalking)
                Walk();
            
            // stop walking if the sprite has stopped moving:
            if (!wasMoving && _isWalking)
                Idle();

            // if not moving and not already idling, start idling:
            if (!wasMoving && !_isIdling)
                Idle();
        }

        public void Walk()
        {
            _animationTimeAtCurrentFrame = 0;
            _animation.Clear();

            _animationSpeed = _walkAnimationSpeed;

            Animation walkAnimation;
            bool flip = false;

            if (_orientation == Direction.Up)
            {
                walkAnimation = _walkUpAnimation;
            }
            else if (_orientation == Direction.Left)
            {
                walkAnimation = _walkRightAnimation;
                flip = true;
            }
            else if (_orientation == Direction.Right)
            {
                walkAnimation = _walkRightAnimation;
            }
            else // default to down:
            {
                walkAnimation = _walkDownAnimation;
            }

            for (var n = walkAnimation.Length - 1; n >= 0; n--)
            {
                var index = n;
                _animation.Push(spriteBatch =>
                {
                    _isIdling = false;
                    _isWalking = true;

                    var sourceRectangle = new Rectangle(
                        index * _scaledWidth,
                        walkAnimation.Row * _scaledHeight,
                        _scaledWidth,
                        _scaledHeight);

                    var offsetPosition = new Vector2(
                        _mapPosition.X + _scaledOffsetX,
                        _mapPosition.Y + _scaledOffsetY);

                    spriteBatch.Draw(_spritesheet, offsetPosition, sourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                });
            }
        }
    }
}
