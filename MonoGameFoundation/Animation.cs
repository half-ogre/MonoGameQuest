using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameFoundation
{
    /// <summary>
    /// Animates a series of frames on a specified row of a sprite sheet.
    /// </summary>
    public class Animation
    {
        int _currentFrameDuration;
        int _currentFrameIndex;
        readonly Texture2D _spriteSheet;

        /// <summary>
        /// Creates a new Animation instance.
        /// </summary>
        /// <param name="spriteSheet">The source sprite sheet containing the row of frames to animate.</param>
        /// <param name="spriteSheetRow">The row in the sprite sheet containing the frames to animate.</param>
        /// <param name="framePixelWidth">The width in pixels of each frame in the row.</param>
        /// <param name="framePixelHeight">The height in pixel of each frame in the row.</param>
        /// <param name="framesLength">The number of frames in the row.</param>
        /// <param name="frameDuration">The time, in milliseconds, to display each frame.</param>
        /// <param name="flipHorizontally">A flag specifying whether to flip the animation horizontally (e.g., to turn an animation facing the right into a left-facing animation).</param>
        public Animation(
            Texture2D spriteSheet,
            int spriteSheetRow,
            int framePixelWidth,
            int framePixelHeight,
            int framesLength,
            int frameDuration = 0,
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

            if (framesLength > 1 && frameDuration < 1)
                throw new ArgumentException("Animation frame duration must be greater than zero.", "frameDuration");

            _spriteSheet = spriteSheet;

            SpriteSheetRow = spriteSheetRow;
            FramePixelWidth = framePixelWidth;
            FramePixelHeight = framePixelHeight;
            FramesLength = framesLength;
            FrameDuration = frameDuration;
            FlipHorizontally = flipHorizontally;
        }

        /// <summary>
        /// Draw the current frame.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch in which to draw.</param>
        /// <param name="position">The position at which to draw.</param>
        /// <param name="scale">The scale at which to draw.</param>
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
        
        /// <summary>
        /// Get the flag specifying whether to flip the animation horizontally (e.g., to turn an animation facing the right into a left-facing animation).
        /// </summary>
        public bool FlipHorizontally { get; private set; }

        /// <summary>
        /// Gets the time, in milliseconds, to display each frame.
        /// </summary>
        public int FrameDuration { get; private set; }

        /// <summary>
        /// Gets the width in pixels of each frame in the row.
        /// </summary>
        public int FramePixelWidth { get; private set; }

        /// <summary>
        /// Gets the height in pixels of each frame in the row.
        /// </summary>
        public int FramePixelHeight { get; private set; }

        /// <summary>
        /// Gets the number of frames in the row.
        /// </summary>
        public int FramesLength { get; private set; }

        /// <summary>
        /// Resets the animation to the initial frame.
        /// </summary>
        public void Reset()
        {
            _currentFrameDuration = 0;
            _currentFrameIndex = 0;
        }

        public int SpriteSheetRow { get; private set; }

        /// <summary>
        /// Updates the current frame of the animation.
        /// </summary>
        /// <param name="gameTime"></param>
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
