using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        readonly GraphicsDeviceManager _graphics;
        Map _map;
        PlayerCharacter _pc;
        int _scale = 1;
        SpriteBatch _spriteBatch;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _map = new Map(Content);
            _pc = new PlayerCharacter(Content, new Vector2(0, 0), _map);
        }

        void SetScale(PresentationParameters presentationParameters)
        {
            if (presentationParameters .BackBufferWidth <= 1500 || presentationParameters.BackBufferHeight <= 870)
                _scale = 2;
            else
                _scale = 3;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SetScale(_graphics.GraphicsDevice.PresentationParameters);

            var context = new UpdateContext
            {
                GameTime = gameTime,
                Graphics = _graphics,
                KeyboardState = Keyboard.GetState(),
                MapScale = _scale,
                MapTileHeight = _map.TileHeight,
                MapTileWidth = _map.TileWidth
            };

            _map.Update(context);
            _pc.Update(context);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            _map.Draw(_spriteBatch, _pc.Position);
            _pc.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
