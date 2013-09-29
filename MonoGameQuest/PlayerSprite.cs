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

        protected PlayerSprite(
            MonoGameQuest game,
            string spriteSheetName,
            int pixelWidth,
            int pixelHeight,
            int pixelOffsetX,
            int pixelOffsetY,
            Vector2 coordinatePosition) 
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

            _animations = new Dictionary<Tuple<AnimationType, Direction>, PlayerSpriteAnimation>();

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

        public void Animate(AnimationType type, Direction direction)
        {
            PlayerSpriteAnimation animation;

            var key = new Tuple<AnimationType, Direction>(type, direction);
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
            
            set
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
                translatedY) * Game.Display.Scale;
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Game.Display.Scale;

            base.Update(gameTime);
        }
    }
}
