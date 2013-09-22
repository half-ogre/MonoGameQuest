using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class MonoGameQuest : Game
    {
        int _fpsCounter;
        TimeSpan _fpsElapsedTime = TimeSpan.Zero;
        int _fpsRate;
        readonly GraphicsDeviceManager _graphics;
        Map _map;
        PlayerCharacter _pc;
        bool _showDebugInfo;
        SpriteBatch _spriteBatch;
        SpriteFont _spriteFont;
        Texture2D _pixelForGrid;
        bool _tildeKeyWasPressed;

        public MonoGameQuest()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = @"Content";

            Scale = 1;
        }

        protected override void Draw(GameTime gameTime)
        {
            _fpsCounter++;
            var debugInfo = string.Format("fps: {0}", _fpsRate);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            _map.Draw(_spriteBatch, _pc.Position);
            _pc.Draw(_spriteBatch);

            if (_showDebugInfo)
                DrawGrid(_spriteBatch);

            if (_showDebugInfo)
            {
                _spriteBatch.DrawString(_spriteFont, debugInfo, new Vector2(1, 1), Color.Black, 0, Vector2.Zero, .5f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_spriteFont, debugInfo, new Vector2(0, 0), Color.Yellow, 0, Vector2.Zero, .5f, SpriteEffects.None, 0f);  
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawGrid(SpriteBatch spriteBatch)
        {
            var mapHeight = _map.Height * _map.TileHeight;
            var mapWidth = _map.Width*_map.TileWidth;

            DrawLine(spriteBatch, Vector2.Zero, new Vector2(mapWidth, 0));
            for (var y = 1; y <= _map.DisplayHeight; y++)
            {
                var adjustedY = y*_map.TileHeight;
                DrawLine(spriteBatch, new Vector2(0, adjustedY-1), new Vector2(mapWidth, adjustedY-1));
                DrawLine(spriteBatch, new Vector2(0, adjustedY), new Vector2(mapWidth, adjustedY));
            }

            DrawLine(spriteBatch, Vector2.Zero, new Vector2(0, mapHeight));
            for (int x = 1; x <= _map.DisplayWidth; x++)
            {
                var adjustedX = x * _map.TileWidth;
                DrawLine(spriteBatch, new Vector2(adjustedX-1, 0), new Vector2(adjustedX-1, mapHeight));
                DrawLine(spriteBatch, new Vector2(adjustedX, 0), new Vector2(adjustedX, mapHeight));
            }
        }

        void DrawLine(
            SpriteBatch spriteBatch, 
            Vector2 start, 
            Vector2 end)
        {
            var edge = end - start;
            var angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(
                _pixelForGrid,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(),
                    1),
                null,
                Color.White,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }

        protected override void Initialize()
        {
            _map = new Map(this);
            Components.Add(_map);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pc = new PlayerCharacter(Content, new Vector2(0, 0), _map);

            _pixelForGrid = new Texture2D(GraphicsDevice, 1, 1);
            _pixelForGrid.SetData(new[] { new Color(Color.Yellow.R, Color.Yellow.G, Color.Yellow.B, 32) });

            _spriteFont = Content.Load<SpriteFont>("Consolas");
        }

        public int Scale { get; private set; }

        void SetScale(PresentationParameters presentationParameters)
        {
            if (presentationParameters.BackBufferWidth <= 1500 || presentationParameters.BackBufferHeight <= 870)
                Scale = 2;
            else
                Scale = 3;
        }

        protected override void Update(GameTime gameTime)
        {
            _fpsElapsedTime += gameTime.ElapsedGameTime;
            if (_fpsElapsedTime > TimeSpan.FromSeconds(1))
            {
                _fpsElapsedTime -= TimeSpan.FromSeconds(1);
                _fpsRate = _fpsCounter;
                _fpsCounter = 0;
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var tildeKeyPressed = Keyboard.GetState().IsKeyDown(Keys.OemTilde);
            if (!tildeKeyPressed && _tildeKeyWasPressed)
                _showDebugInfo = !_showDebugInfo;
            _tildeKeyWasPressed = tildeKeyPressed;

            SetScale(_graphics.GraphicsDevice.PresentationParameters);

            var context = new UpdateContext
            {
                GameTime = gameTime,
                Graphics = _graphics,
                KeyboardState = Keyboard.GetState(),
                MapScale = Scale,
                MapTileHeight = _map.TileHeight,
                MapTileWidth = _map.TileWidth
            };

            _pc.Update(context);

            base.Update(gameTime);
        }
    }
}
