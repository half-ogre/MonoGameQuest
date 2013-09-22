using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class MonoGameQuest : Game
    {
        readonly GraphicsDeviceManager _graphics;

        public MonoGameQuest()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = @"Content";

            Scale = 1;
        }

        protected override void Initialize()
        {
            Map = new Map(this);
            Components.Add(Map);

            Components.Add(new Terrain(this));
            Components.Add(new PlayerCharacter(this));
            Components.Add(new DebugInfo(this));
            
            base.Initialize();
        }

        public Map Map { get; private set; }

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SetScale(_graphics.GraphicsDevice.PresentationParameters);

            base.Update(gameTime);
        }
    }
}
