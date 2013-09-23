using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest
{
    public abstract class PlayerSprite : MonoGameQuestDrawableComponent
    {
        readonly Dictionary<Tuple<AnimationType, Direction>, PlayerSpriteAnimation> _animations;
        readonly Stack<Action> _movement;
        readonly int _movementLength;
        readonly int _movementSpeed;
        int _movementTimeAtCurrentPosition;
        readonly Texture2D _spriteSheet;

        protected PlayerSprite(
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

            _animations = new Dictionary<Tuple<AnimationType, Direction>, PlayerSpriteAnimation>();
            _movement = new Stack<Action>();
            _spriteSheet = contentManager.Load<Texture2D>(string.Concat(@"images\", spriteSheetName));

            DrawOrder = Constants.DrawOrder.Sprites;
            Orientation = Direction.Down;
            UpdateOrder = Constants.UpdateOrder.Sprites;
        }

        public void AddAnimation(
            AnimationType type, 
            Direction direction, 
            int spriteSheetRow, 
            int framesLength, 
            int frameDuration, 
            bool flipHorizontally = false)
        {
            var key = new Tuple<AnimationType, Direction>(type, direction);

            if (_animations.ContainsKey(key))
                throw new ArgumentException("An animation with the specified type and direction has already been added.");

            var animation = new PlayerSpriteAnimation(
                this,
                type,
                direction,
                spriteSheetRow,
                framesLength,
                frameDuration,
                flipHorizontally);

            _animations.Add(key, animation);
        }

        public void Animate(AnimationType type)
        {
            PlayerSpriteAnimation animation;

            var key = new Tuple<AnimationType, Direction>(type, Orientation);
            if (!_animations.TryGetValue(key, out animation))
                throw new InvalidOperationException("No animation has been added for the specified type and key.");

            CurrentAnimation = animation;
            CurrentAnimation.Reset();
        }

        public Vector2 CoordinatePosition { get; private set; }

        public PlayerSpriteAnimation CurrentAnimation { get; protected set; }

        public override void Draw(GameTime gameTime)
        {
            if (CurrentAnimation == null)
                return;

            float adjustedX;
            float adjustedY;

            var zeroBasedDisplayWidth = Game.Display.CoordinateWidth - 1f;
            var zeroBasedDisplayHeight = Game.Display.CoordinateHeight - 1f;
            var zeroBasedDisplayMidpointX = (Game.Display.CoordinateWidth - 1f) / 2f;
            var zeroBasedDisplayMidpointY = (Game.Display.CoordinateHeight - 1f) / 2f;
            var zeroBasedMapWidth = Game.Map.CoordinateWidth - 1f;
            var zeroBasedMapHeight = Game.Map.CoordinateHeight - 1f;

            // adjust sprite position to center, unless the sprite is at the map's edge:
            if (CoordinatePosition.X < zeroBasedDisplayMidpointX)
                adjustedX = CoordinatePosition.X;
            else if (CoordinatePosition.X > zeroBasedMapWidth - zeroBasedDisplayMidpointX)
                adjustedX = zeroBasedDisplayWidth - (zeroBasedMapWidth - CoordinatePosition.X);
            else
                adjustedX = zeroBasedDisplayMidpointX;
            if (CoordinatePosition.Y < zeroBasedDisplayMidpointY)
                adjustedY = CoordinatePosition.Y;
            else if (CoordinatePosition.Y > zeroBasedMapHeight - zeroBasedDisplayMidpointY)
                adjustedY = zeroBasedDisplayHeight - (zeroBasedMapHeight - CoordinatePosition.Y);
            else
                adjustedY = zeroBasedDisplayMidpointY;

            // adjust sprite position for the specified offset
            adjustedX = (adjustedX * Game.Map.PixelTileWidth) + PixelOffsetX;
            adjustedY = (adjustedY * Game.Map.PixelTileHeight) + PixelOffsetY;

            var adjustedPosition = new Vector2(
                adjustedX,
                adjustedY);
            
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            CurrentAnimation.Draw(SpriteBatch, adjustedPosition, Game.Display.Scale);

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
