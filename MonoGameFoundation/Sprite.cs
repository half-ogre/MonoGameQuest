using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameFoundation
{
    public abstract class Sprite : DrawableGameComponent
    {
        protected Sprite(
            Game game,
            Texture2D spriteSheet,
            int pixelWidth,
            int pixelHeight,
            int pixelOffsetX,
            int pixelOffsetY)
            : this(game, spriteSheet, pixelWidth, pixelHeight, pixelOffsetX, pixelOffsetY, Vector2.Zero)
        {
        }
        
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

        protected virtual void BeginSpriteBatch()
        {
            SpriteBatch.Begin();
        }
        
        public Animation CurrentAnimation { get; protected set; }

        public override void Draw(GameTime gameTime)
        {
            if (SpriteBatch == null)
                throw new InvalidOperationException("Cannot draw because SpriteBatch is null.");
            
            if (CurrentAnimation == null)
                return;

            BeginSpriteBatch();

            CurrentAnimation.Draw(SpriteBatch, PixelPosition, Scale);

            SpriteBatch.End();
        }

        public override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        public int PixelHeight { get; protected set; }

        public int PixelOffsetX { get; protected set; }

        public int PixelOffsetY { get; protected set; }

        public Vector2 PixelPosition { get; protected set; }

        public int PixelWidth { get; protected set; }

        public float Scale { get; protected set; }

        protected SpriteBatch SpriteBatch { get; set; }

        public Texture2D SpriteSheet { get; protected set; }

        public override void Update(GameTime gameTime)
        {
            if (CurrentAnimation != null)
                CurrentAnimation.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
