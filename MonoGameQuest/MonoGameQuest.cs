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
            Display = new Display(this);
            Components.Add(Display);
            
            Map = new Map(this);
            Components.Add(Map);

            Components.Add(new Terrain(this));
            Components.Add(new PlayerCharacter(this));
            Components.Add(new DebugInfo(this));
            
            base.Initialize();
        }

        public Display Display { get; private set; }

        public Map Map { get; private set; }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }
    }
}
