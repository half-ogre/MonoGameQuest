using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameQuest
{
    public class MonoGameQuest : Game
    {
        public MonoGameQuest()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new GraphicsDeviceManager(this); /* this is needed to initialize the graphics display service */
            Content.RootDirectory = @"Content";
        }

        protected override void Initialize()
        {
            Map = new Map(this);
            Components.Add(Map);
            
            Display = new Display(this);
            Components.Add(Display);

            Player = new Player(this);
            Components.Add(Player);
            
            Components.Add(new Terrain(this));
            Components.Add(new DebugInfo(this));
            Components.Add(new Cursor(this));
            Components.Add(new PathfindingBox(this));
            
            base.Initialize();
        }

        public Display Display { get; private set; }

        public Map Map { get; private set; }

        public Player Player { get; private set; }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            var mouse = Mouse.GetState();
            if (mouse.X < 0
                || mouse.Y < 0
                || mouse.X > GraphicsDevice.PresentationParameters.BackBufferWidth
                || mouse.Y > GraphicsDevice.PresentationParameters.BackBufferHeight)
                IsMouseVisible = true;
            else
                IsMouseVisible = false;
        }
    }
}
