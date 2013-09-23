using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameFoundation
{
    public class Animation
    {
        int _currentFrameDuration;
        int _currentFrameIndex;
        readonly Texture2D _spriteSheet;

        public Animation(
            Texture2D spriteSheet,
            int spriteSheetRow,
            int framePixelWidth,
            int framePixelHeight,
            int framesLength,
            int frameDuration,
            bool flipHorizontally = false)
        {
            if (spriteSheet == null)
                throw new ArgumentNullException("spriteSheet");

            if (spriteSheetRow < 0)
                throw new ArgumentException("Animation sprite sheet row must be at least zero.", "spriteSheetRow");

            if (framePixelWidth < 1)
                throw new ArgumentException("Animation frame width must be greater than zero.", "framePixelWidth");

            if (framePixelHeight < 1)
                throw new ArgumentException("Animation frame height must be greater than zero.", "framePixelHeight");
            
            if (framesLength < 1)
                throw new ArgumentException("Animation frames length must be greater than zero.", "framesLength");

            if (frameDuration < 1)
                throw new ArgumentException("Animation frame duration must be greater than zero.", "frameDuration");

            _spriteSheet = spriteSheet;

            SpriteSheetRow = spriteSheetRow;
            FramePixelWidth = framePixelWidth;
            FramePixelHeight = framePixelHeight;
            FramesLength = framesLength;
            FrameDuration = frameDuration;
            FlipHorizontally = flipHorizontally;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            var sourceRectangle = new Rectangle(
                _currentFrameIndex * FramePixelWidth,
                SpriteSheetRow * FramePixelHeight,
                FramePixelWidth,
                FramePixelHeight);
    
            spriteBatch.Draw(
                texture: _spriteSheet,
                position: position * scale,
                sourceRectangle: sourceRectangle,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: scale,
                effect: FlipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                depth: 0f);
        }
        
        public bool FlipHorizontally { get; private set; }

        public int FrameDuration { get; private set; }

        public int FramePixelWidth { get; private set; }

        public int FramePixelHeight { get; private set; }

        public int FramesLength { get; private set; }

        public void Reset()
        {
            _currentFrameDuration = 0;
            _currentFrameIndex = 0;
        }

        public int SpriteSheetRow { get; private set; }

        public void Update(GameTime gameTime)
        {
            _currentFrameDuration += gameTime.ElapsedGameTime.Milliseconds;

            if (_currentFrameDuration > FrameDuration)
            {
                _currentFrameDuration = 0;

                if (++_currentFrameIndex >= FramesLength)
                    _currentFrameIndex = 0;
            }
        }
    }
}
