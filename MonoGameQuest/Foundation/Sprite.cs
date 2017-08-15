using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameQuest.Foundation
{
    /// <summary>
    /// A sprite to animate via a sprite sheet.
    /// </summary>
    public class Sprite : DrawableGameComponent
    {
        /// <summary>
        /// Creates a new Sprite instance.
        /// </summary>
        /// <param name="game">The global game instance.</param>
        /// <param name="spriteSheet">The sprite sheet containing the sprite's animation frames.</param>
        /// <param name="pixelWidth">The width of the sprite in pixels.</param>
        /// <param name="pixelHeight">The height of the sprite in pixels.</param>
        /// <param name="pixelOffsetX">The sprite's X offset, used to to center the sprite on its location.</param>
        /// <param name="pixelOffsetY">The sprite's Y offset, used to to center the sprite on its location.</param>
        public Sprite(
            Game game,
            Texture2D spriteSheet,
            int pixelWidth,
            int pixelHeight,
            int pixelOffsetX,
            int pixelOffsetY)
            : this(game, spriteSheet, pixelWidth, pixelHeight, pixelOffsetX, pixelOffsetY, Vector2.Zero)
        {
        }
        
        /// <summary>
        /// Creates a new Sprite instance.
        /// </summary>
        /// <param name="game">The global game instance.</param>
        /// <param name="spriteSheet">The sprite sheet containing the sprite's animation frames.</param>
        /// <param name="pixelWidth">The width of the sprite in pixels.</param>
        /// <param name="pixelHeight">The height of the sprite in pixels.</param>
        /// <param name="pixelOffsetX">The sprite's X offset, used to to center the sprite on its location.</param>
        /// <param name="pixelOffsetY">The sprite's Y offset, used to to center the sprite on its location.</param>
        /// <param name="pixelPosition">The sprite position in the display, in pixels.</param>
        protected Sprite(
            Game game,
            Texture2D spriteSheet,
            int pixelWidth,
            int pixelHeight,
            int pixelOffsetX,
            int pixelOffsetY,
            Vector2 pixelPosition) 
            : base(game)
        {
            SpriteSheet = spriteSheet;
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;            
            PixelOffsetX = pixelOffsetX;
            PixelOffsetY = pixelOffsetY;
            PixelPosition = pixelPosition;

            Scale = 1;
        }

        /// <summary>
        /// Calls SpriteBatch.Begin with default arguments. Override if you need to change the arguments to SpriteBatch.Begin.
        /// </summary>
        protected virtual void BeginSpriteBatch()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState ?? BlendState.AlphaBlend, SamplerState ?? SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
        }

        /// <summary>
        /// The <see cref="Microsoft.Xna.Framework.Graphics.BlendState"/> used when <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin()"/> is invoked in <see cref="BeginSpriteBatch()"/>.
        /// </summary>
        /// <remarks>
        /// When null, <see cref="Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend"/> will be used.
        /// </remarks>
        public BlendState BlendState { get; set; }
        
        /// <summary>
        /// The sprite's current animation, which will be used to draw the sprite.
        /// </summary>
        public Animation CurrentAnimation { get; set; }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="gameTime">The game's current time.</param>
        public override void Draw(GameTime gameTime)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException("Cannot draw because SpriteBatch is null.");
            
            if (CurrentAnimation == null)
                return;

            BeginSpriteBatch();

            CurrentAnimation.Draw(SpriteBatch, PixelPosition, Scale);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Initializes the sprite.
        /// </summary>
        /// <remarks>
        /// This creates the SpriteBatch. If you override Initialize, be sure to call base.Initialize to create the SpriteBatch.
        /// </remarks>
        public override void Initialize()
        {
            base.Initialize();

            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// Gets the height of the sprite in pixels.
        /// </summary>
        public int PixelHeight { get; protected set; }

        /// <summary>
        /// Gets the sprite's X offset, whic is used to to center the sprite on its location.
        /// </summary>
        public int PixelOffsetX { get; protected set; }

        /// <summary>
        /// Gets the sprite's Y offset, whic is used to to center the sprite on its location.
        /// </summary>
        public int PixelOffsetY { get; protected set; }

        /// <summary>
        /// Gets the sprite's position in the display, in pixels.
        /// </summary>
        public Vector2 PixelPosition { get; set; }

        /// <summary>
        /// Gets the width of the sprite in pixels.
        /// </summary>
        public int PixelWidth { get; protected set; }

        /// <summary>
        /// The <see cref="Microsoft.Xna.Framework.Graphics.SamplerState"/> used when <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin()"/> is invoked in <see cref="BeginSpriteBatch()"/>.
        /// </summary>
        /// <remarks>
        /// When null, <see cref="Microsoft.Xna.Framework.Graphics.SamplerState.LinearClamp"/> will be used.
        /// </remarks>
        public SamplerState SamplerState { get; set; }

        /// <summary>
        /// The scale at which the sprite should be positioned and drawn.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Gets or sets the SpriteBatch used to draw the sprite.
        /// </summary>
        protected SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Gets or the sprite sheet containing the sprite's animation frames.
        /// </summary>
        public Texture2D SpriteSheet { get; protected set; }

        /// <summary>
        /// Updates the sprite's current animation.
        /// </summary>
        /// <param name="gameTime">The game's current time.</param>
        public override void Update(GameTime gameTime)
        {
            if (CurrentAnimation != null)
                CurrentAnimation.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
