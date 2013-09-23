using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Sprites
{
    public abstract class PlayerCharacterSprite : MonoGameQuestDrawableComponent
    {
        readonly Dictionary<Tuple<AnimationType, Direction>, Animation> _animations;
        readonly Stack<Action> _movement;
        readonly int _movementLength;
        readonly int _movementSpeed;
        int _movementTimeAtCurrentPosition;
        readonly Texture2D _spriteSheet;

        protected PlayerCharacterSprite(
            MonoGameQuest game,
            ContentManager contentManager,
            string spriteSheetName,
            int pixelHeight,
            int pixelWidth,
            int pixelOffsetX,
            int pixelOffsetY,
            Vector2 coordinatePosition,
            Map map,
            int movementLength,
            int movementSpeed) : base(game)
        {
            PixelHeight = pixelHeight;
            PixelWidth = pixelWidth;
            PixelOffsetX = pixelOffsetX;
            PixelOffsetY = pixelOffsetY;
            CoordinatePosition = coordinatePosition;
            Map = map;
            _movementLength = movementLength;
            _movementSpeed = movementSpeed;

            _animations = new Dictionary<Tuple<AnimationType, Direction>, Animation>();
            _movement = new Stack<Action>();
            _spriteSheet = contentManager.Load<Texture2D>(string.Concat(@"images\", spriteSheetName));

            DrawOrder = Constants.DrawOrder.Sprites;
            Orientation = Direction.Down;
            UpdateOrder = Constants.UpdateOrder.Sprites;
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

        public Vector2 CoordinatePosition { get; private set; }

        public Animation CurrentAnimation { get; protected set; }

        public override void Draw(GameTime gameTime)
        {
            if (CurrentAnimation == null)
                return;
            
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            CurrentAnimation.Draw(SpriteBatch);

            SpriteBatch.End();
        }

        public bool IsMoving { get { return _movement.Count > 0; } }

        public Map Map { get; private set; }

        public void Move(Direction direction)
        {
            var xDelta = 0f;
            var yDelta = 0f;

            if (direction == Direction.Up)
                yDelta = -1f;
            if (direction == Direction.Down)
                yDelta = 1f;
            if (direction == Direction.Left)
                xDelta = -1f;
            if (direction == Direction.Right)
                xDelta = 1f;

            var newX = CoordinatePosition.X + xDelta;
            var newY = CoordinatePosition.Y + yDelta;

            // don't let the sprite move off the map:
            if (newX < 0 || newX > Map.CoordinateWidth - 1 || newY < 0 || newY > Map.CoordinateHeight - 1)
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
                    CoordinatePosition = new Vector2(
                        CoordinatePosition.X + (xDelta / _movementLength),
                        CoordinatePosition.Y + (yDelta / _movementLength));
                });
            }
        }        

        public Direction Orientation { get; protected set; }

        public int PixelHeight { get; private set; }

        public int PixelOffsetX { get; private set; }

        public int PixelOffsetY { get; private set; }

        public int PixelWidth { get; private set; }

        public Texture2D SpriteSheet { get { return _spriteSheet; } }

        public override void Update(GameTime gameTime)
        {
            // stash these values because they will change during update, 
            // and we need to know the value when the update started.
            var wasMoving = IsMoving;

            if (wasMoving)
            {
                _movementTimeAtCurrentPosition += gameTime.ElapsedGameTime.Milliseconds;

                if (_movementTimeAtCurrentPosition > _movementSpeed / _movementLength)
                {
                    _movementTimeAtCurrentPosition = 0;
                    _movement.Pop()();
                }
            }

            if (CurrentAnimation != null)
                CurrentAnimation.Update(gameTime);

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
    }
}
