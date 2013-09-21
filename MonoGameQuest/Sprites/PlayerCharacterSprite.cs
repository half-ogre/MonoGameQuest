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
        readonly ContentManager _contentManager;
        readonly int _unscaledHeight;
        readonly Stack<Action> _movement;
        readonly int _movementLength;
        readonly int _movementSpeed;
        int _movementTimeAtCurrentPosition;
        readonly int _unscaledOffsetX;
        readonly int _unscaledOffsetY;
        Vector2 _position;
        int _scale = 1;
        int _scaledHeight;
        int _scaledOffsetX;
        int _scaledOffsetY;
        int _scaledWidth;
        Texture2D _spriteSheet;
        readonly string _spriteSheetName;
        readonly int _unscaledWidth;

        protected PlayerCharacterSprite(
            ContentManager contentManager,
            string spriteSheetName,
            int height,
            int width,
            int offsetX,
            int offsetY,
            Vector2 position,
            Map map,
            int movementLength,
            int movementSpeed)
        {
            _contentManager = contentManager;
            _spriteSheetName = spriteSheetName;
            _unscaledHeight = _scaledHeight = height;
            _unscaledWidth = _scaledWidth = width;
            _unscaledOffsetX = _scaledOffsetX = offsetX;
            _unscaledOffsetY = _scaledOffsetY = offsetY;
            _position = position;
            Map = map;
            _movementLength = movementLength;
            _movementSpeed = movementSpeed;

            _animations = new Dictionary<Tuple<AnimationType, Direction>, Animation>();
            _movement = new Stack<Action>();
            _spriteSheet = _contentManager.Load<Texture2D>(string.Concat(@"images\1\", _spriteSheetName));

            Orientation = Direction.Down;
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

        public void Animate(AnimationType type)
        {
            Animation animation;

            var key = new Tuple<AnimationType, Direction>(type, Orientation);
            if (!_animations.TryGetValue(key, out animation))
                throw new InvalidOperationException("No animation has been added for the specified type and key.");

            CurrentAnimation = animation;
            CurrentAnimation.Reset();
        }

        public Animation CurrentAnimation { get; protected set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentAnimation != null)
                CurrentAnimation.Draw(spriteBatch);
        }

        public int Height { get { return _scaledHeight; } }

        public bool IsMoving { get { return _movement.Count > 0; } }

        public Map Map { get; private set; }

        public void Move(Direction direction)
        {
            var offsetX = 0f;
            var offsetY = 0f;

            if (direction == Direction.Up)
                offsetY = -1f;
            if (direction == Direction.Down)
                offsetY = 1f;
            if (direction == Direction.Left)
                offsetX = -1f;
            if (direction == Direction.Right)
                offsetX = 1f;

            var newX = _position.X + offsetX;
            var newY = _position.Y + offsetY;

            // don't let the sprite move off the map:
            if (newX < 0 || newX > Map.Width - 1 || newY < 0 || newY > Map.Height - 1)
                return;

            _movementTimeAtCurrentPosition = 0;
            _movement.Clear();

            if (Orientation != direction)
            {
                Orientation = direction;

                // start a new walk animation for the new orientation
                Animate(AnimationType.Walk);
            }

            for (var n = _movementLength - 1; n >= 0; n--)
            {
                _movement.Push(() =>
                {
                    _position = new Vector2(
                        _position.X + (offsetX / _movementLength),
                        _position.Y + (offsetY / _movementLength));
                });
            }
        }

        public int OffsetX { get { return _scaledOffsetX; } }

        public int OffsetY { get { return _scaledOffsetY; } }

        public Direction Orientation { get; protected set; }

        public Vector2 Position { get { return _position; } }

        void SetScale(UpdateContext context)
        {
            _scale = context.MapScale;

            _spriteSheet = _contentManager.Load<Texture2D>(string.Concat(@"images\", _scale, @"\", _spriteSheetName));

            _scaledHeight = _unscaledHeight * _scale;
            _scaledWidth = _unscaledWidth * _scale;

            _scaledOffsetX = _unscaledOffsetX * _scale;
            _scaledOffsetY = _unscaledOffsetY * _scale;
        }

        public Texture2D SpriteSheet { get { return _spriteSheet; } }

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

            if (CurrentAnimation != null)
                CurrentAnimation.Update(context.GameTime);

            // start walking if the sprite is moving and isn't already walking:
            if (wasMoving && (CurrentAnimation == null || CurrentAnimation.Type != AnimationType.Walk))
                Animate(AnimationType.Walk);

            // stop walking if the sprite has stopped moving:
            if (!wasMoving && (CurrentAnimation == null || CurrentAnimation.Type == AnimationType.Walk))
                Animate(AnimationType.Idle);

            // if not moving and not already idling, start idling:
            if (!wasMoving && (CurrentAnimation == null || CurrentAnimation.Type != AnimationType.Idle))
                Animate(AnimationType.Idle);

            if (CurrentAnimation == null)
                Animate(AnimationType.Idle);
        }

        public int Width { get { return _scaledWidth; } }
    }
}
