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
            _pc = new PlayerCharacter(Content, new Vector2(0, 0));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _map.Update(_graphics);
            _pc.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            _map.Draw(_spriteBatch);
            _pc.Draw(_spriteBatch, _map.TileHeight, _map.TileWidth);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
