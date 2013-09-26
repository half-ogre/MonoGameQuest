using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameFoundation;

namespace MonoGameQuest
{
    public abstract class PlayerSprite : Sprite
    {
        readonly Dictionary<Tuple<AnimationType, Direction>, PlayerSpriteAnimation> _animations;
        Vector2 _coordinatePosition;
        readonly Stack<Action> _movement;
        readonly int _movementLength;
        readonly int _movementSpeed;
        int _movementTimeAtCurrentPosition;

        protected PlayerSprite(
            MonoGameQuest game,
            string spriteSheetName,
            int pixelWidth,
            int pixelHeight,
            int pixelOffsetX,
            int pixelOffsetY,
            Vector2 coordinatePosition,
            int movementLength,
            int movementSpeed) 
            : base(
                game,
                game.Content.Load<Texture2D>(string.Concat(@"images\", spriteSheetName)),
                pixelWidth, 
                pixelHeight,
                pixelOffsetX, 
                pixelOffsetY)
        {
            PixelHeight = pixelHeight;
            PixelWidth = pixelWidth;
            PixelOffsetX = pixelOffsetX;
            PixelOffsetY = pixelOffsetY;
            CoordinatePosition = coordinatePosition;

            DrawOrder = Constants.DrawOrder.Sprites;
            UpdateOrder = Constants.UpdateOrder.Sprites;

            Orientation = Direction.Down;
            
            _animations = new Dictionary<Tuple<AnimationType, Direction>, PlayerSpriteAnimation>();
            
            _movement = new Stack<Action>();
            _movementLength = movementLength;
            _movementSpeed = movementSpeed;

            // update the pixel position for the starting coordinate position:
            TranslateCoordinatePositionToPixelPosition();
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

        protected override void BeginSpriteBatch()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }

        public Vector2 CoordinatePosition
        {
            get { return _coordinatePosition; }
            
            private set
            {
                _coordinatePosition = value;
                TranslateCoordinatePositionToPixelPosition();
            }
        }

        public new PlayerSpriteAnimation CurrentAnimation
        {
            get { return base.CurrentAnimation as PlayerSpriteAnimation; }
            protected set { base.CurrentAnimation = value; }
        }

        public new MonoGameQuest Game
        {
            get { return base.Game as MonoGameQuest; }
        }

        public bool IsMoving { get { return _movement.Count > 0; } }

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
            if (newX < 0 || newX > Game.Map.CoordinateWidth - 1 || newY < 0 || newY > Game.Map.CoordinateHeight - 1)
                return;

            _movementTimeAtCurrentPosition = _movementTimeAtCurrentPosition % _movementSpeed;
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

        protected void TranslateCoordinatePositionToPixelPosition()
        {
            float translatedX;
            float translatedY;

            // adjust sprite position to center, unless the sprite is at the map's edge:
            if (CoordinatePosition.X < Game.Display.CoordinateMidpoint.X)
                translatedX = CoordinatePosition.X;
            else if (CoordinatePosition.X > Game.Map.CoordinateTerminus.X - Game.Display.CoordinateMidpoint.X)
                translatedX = Game.Display.CoordinateTerminus.X - (Game.Map.CoordinateTerminus.X - CoordinatePosition.X);
            else
                translatedX = Game.Display.CoordinateMidpoint.X;
            if (CoordinatePosition.Y < Game.Display.CoordinateMidpoint.Y)
                translatedY = CoordinatePosition.Y;
            else if (CoordinatePosition.Y > Game.Map.CoordinateTerminus.Y - Game.Display.CoordinateMidpoint.Y)
                translatedY = Game.Display.CoordinateTerminus.Y - (Game.Map.CoordinateTerminus.Y - CoordinatePosition.Y);
            else
                translatedY = Game.Display.CoordinateMidpoint.Y;

            // adjust sprite position for the specified offset
            translatedX = (translatedX * Game.Map.PixelTileWidth) + PixelOffsetX;
            translatedY = (translatedY * Game.Map.PixelTileHeight) + PixelOffsetY;

            PixelPosition = new Vector2(
                translatedX,
                translatedY);
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Game.Display.Scale;

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

            base.Update(gameTime);
        }
    }
}
