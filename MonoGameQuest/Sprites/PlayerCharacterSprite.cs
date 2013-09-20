using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Sprites
{
    public abstract class PlayerCharacterSprite
    {
        readonly Dictionary<Tuple<AnimationType, Direction>, Animation> _animations; 
        int _animationSpeed;
        readonly Stack<Action<SpriteBatch>> _animationStack;
        int _animationTimeAtCurrentFrame;
        readonly ContentManager _contentManager;
        readonly int _height;
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
            int movementSpeed)
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

            _animations = new Dictionary<Tuple<AnimationType, Direction>, Animation>();
            _animationStack = new Stack<Action<SpriteBatch>>();
            _movement = new Stack<Action>();
            _orientation = Direction.Down;
            _spritesheet = _contentManager.Load<Texture2D>(string.Concat(@"images\1\", _spritesheetName));
        }

        public void AddAnimation(Animation animation)
        {
            if (animation == null)
                throw new ArgumentNullException("animation");

            var key = new Tuple<AnimationType, Direction>(animation.Type, animation.Direction);

            if (_animations.ContainsKey(key))
                throw new ArgumentException("An animation with the specified type and direction has already been added.", "animation");

            _animations.Add(key, animation);
        }
        
        public void Animate(
            Animation animation, 
            Action onFrameStart)
        {
            _animationTimeAtCurrentFrame = 0;
            _animationStack.Clear();

            _animationSpeed = animation.Speed;

            for (var n = animation.Length - 1; n >= 0; n--)
            {
                var index = n;
                _animationStack.Push(spriteBatch =>
                {
                    onFrameStart();

                    var sourceRectangle = new Rectangle(
                        index * _scaledWidth,
                        animation.Row * _scaledHeight,
                        _scaledWidth,
                        _scaledHeight);

                    var offsetPosition = new Vector2(
                        _mapPosition.X + _scaledOffsetX,
                        _mapPosition.Y + _scaledOffsetY);

                    spriteBatch.Draw(
                        texture: _spritesheet, 
                        position: offsetPosition, 
                        sourceRectangle: sourceRectangle, 
                        color: Color.White, 
                        rotation: 0f, 
                        origin: Vector2.Zero, 
                        scale: Vector2.One, 
                        effect: animation.FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
                        depth: 0f);
                });
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAnimating)
            {
                _animationStack.Peek()(spriteBatch);
            }
        }

        public void Idle()
        {
            Animation animation;

            var key = new Tuple<AnimationType, Direction>(AnimationType.Idle, _orientation);
            if (!_animations.TryGetValue(key, out animation))
                throw new InvalidOperationException("No animation has been added for the specified type and key.");

            Animate(animation, () =>
            {
                _isWalking = false;
                _isIdling = true;
            });
        }

        public bool IsAnimating { get { return _animationStack.Count > 0; }}

        public bool IsMoving { get { return _movement.Count > 0; } }

        public void Move(Direction direction)
        {
            _movementTimeAtCurrentPosition = 0;
            _movement.Clear();

            if (_orientation != direction)
            {
                _orientation = direction;

                if (_isWalking)
                {
                    // start a new walk animation for the new orientation
                    Walk();
                }
            }

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
                    _animationStack.Pop();

                    if (_animationStack.Count < 1)
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
            Animation animation;

            var key = new Tuple<AnimationType, Direction>(AnimationType.Walk, _orientation);
            if (!_animations.TryGetValue(key, out animation))
                throw new InvalidOperationException("No animation has been added for the specified type and key.");

            Animate(animation, () =>
            {
                _isIdling = false;
                _isWalking = true;
            });
        }
    }
}
